import time
import datetime as dt 
import picamera
import os
import glob
import subprocess
import threading
import socket
import json
import firebase_admin
from firebase_admin import db
from firebase_admin import credentials
import io
from PIL import Image

# Wait for Incoming Connection, then Start a Streaming Thread
class StreamManagerThread(object):
	def __init__(self):
		thread = threading.Thread(target=self.run, args=())
		thread.daemon = True
		thread.start()

	def run(self):
		try:
			SendFirebaseLog(0, 'StreamManagerThread Started')
			while True:
				# Wait for an Incoming Connection, then select a splitter_port free to stream.
				print('StreamManagerThread, Waiting for Connection...')
				connection, address = server_socket.accept()
				SendFirebaseLog(0, 'Connection Requested By: ' + str(address[0]))

				for count in range(0, len(connectionArray)):
					if connectionArray[count] != 0:
						connectionNumber = connectionArray[count]
						connectionArray[count] = 0
						break
				
				StreamThread(connection.makefile('wb'), connectionNumber, str(address[0]))
		except Exception as ex:
			SendFirebaseLog(1, ex)
		finally:
			pass

# Stream over the connection passed by until client disconnect
class StreamThread(object):
	def __init__(self, connection, connectionNumber, ipAddress):
		self.connection = connection
		self.connectionNumber = connectionNumber
		self.ipAddress = ipAddress

		thread = threading.Thread(target=self.run, args=())
		thread.daemon = True
		thread.start()

	def run(self):
		try:
			print('Start Streaming...')
			SendFirebaseLog(0, 'Start Streaming')
			camera.start_recording(self.connection, format='h264', splitter_port = self.connectionNumber)
			while True:
				camera.wait_recording(2, splitter_port = self.connectionNumber)
		except Exception as ex:
			SendFirebaseLog(1, str(ex) + ' ' + self.ipAddress)
		finally:
			try:
				camera.stop_recording(splitter_port = self.connectionNumber)
			except:
				#Do Nothing
				pass
			finally:
				self.connection.close()
				connectionArray[self.connectionNumber -1] = self.connectionNumber

class AlertSystem(object):
	def __init__(self):
		self.treshold = 20
		self.sensibility = 20 

		threadAlertSystem = threading.Thread(target=self.run, args=())
		threadAlertSystem.daemon = True
		threadAlertSystem.start()
	
	def run(self):
		image1, buffer1 = self.captureImage()
		while(True):
			image2, buffer2 = captureImage()

			# Count changed pixels
			changedPixels = 0
			for x in range(0, 640):
				for y in range(0, 480):
					# Just check green channel as it's the highest quality channel
					pixdiff = abs(buffer1[x,y][1] - buffer2[x,y][1])
					if pixdiff > threshold:
						changedPixels += 1
		
			# Save an image if pixels changed
			if changedPixels > self.sensibility:
				#Send Mail with Image
				pass

			# Swap comparison buffers
			image1 = image2
			buffer1 = buffer2
	
	def captureImage(self):
		imageData = io.StringIO()
		camera.capture(imageData, splitter_port = 0)
    	imageData.seek(0)
    	im = Image.open(imageData)
    	buffer = im.load()
    	imageData.close()
    	return im, buffer
	

def getCurrentDateToString(isLog):
	if(isLog == True):
		return dt.datetime.now().strftime('%d%m%Y-%H:%M:%S')
	else:
		return dt.datetime.now().strftime('%d-%m-%Y.%H-%M')

def SendFirebaseLog(errorType, errorMessage):
	try:
		print(str(errorMessage))
		refChild.update({getCurrentDateToString(True) : {"ErrorType" : errorType, "ErrorMessage" : str(errorMessage)}})
	except:
		pass

def DeleteFileAfterH(fileName):
	try:
		SendFirebaseLog(0, 'Searching For File')
		fileList = glob.glob(pathToFileNas + "*")
		
		for file in fileList:
			if(file[ : len(file) - 7] == fileName):
				SendFirebaseLog(0, 'Deleting File ' + file)
				os.remove(file)
	except Exception as ex:
		SendFirebaseLog(1, ex)	

def Mp4Box(fileName):
	command = "MP4Box -add {} {} ; rm {}".format(pathToFileLocal + fileName, pathToFileNas + fileName.replace(".h264", ".mp4"), pathToFileLocal + fileName)
	try:
		subprocess.Popen(command, shell=True)
	except Exception as ex:
		SendFirebaseLog(1, ex)

#Program Start
#Global Variable Declaration and Initialization

cred = credentials.Certificate('serviceAccountKey.json')
default_app = firebase_admin.initialize_app(cred, {'databaseURL': 'https://testfirebase-3e9f5.firebaseio.com/'})
#default_app = firebase_admin.initialize_app(cred, {'databaseURL': 'https://homesucuritypp.firebaseio.com/'})

ref = db.reference()
refChild = ref.child('Cam01')
SendFirebaseLog(0, 'Program Start')

connectionArray = [1,2,3]
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(('0.0.0.0', 8000))
server_socket.listen(0)

pathToFileLocal = '/home/pi/Projects/'
pathToFileNas = '/mnt/SecurityCam/Cam01/Recordings/'
fileName = getCurrentDateToString(False) + '.h264'

try:
	SendFirebaseLog(0, 'Camera Initializing')

	camera = picamera.PiCamera(resolution = (1270, 720), framerate = 30)
	camera.rotation = 180
	camera.annotate_text_size = 50
	time.sleep(1)
	
	SendFirebaseLog(0, 'Start Recording')

	camera.start_recording(pathToFileLocal + fileName, splitter_port = 0)
	StreamManagerThread()
	
	while True:
		prevFileName = fileName
		date = dt.datetime.today() - dt.timedelta(hours = 12)
		for count in range(0, 1800):
			camera.annotate_text = getCurrentDateToString(True)
			camera.wait_recording(2, splitter_port = 0)
		
		fileName = getCurrentDateToString(False) + '.h264'
		camera.split_recording(pathToFileLocal + fileName, splitter_port = 0)

		Mp4Box(prevFileName)
		DeleteFileAfterH(pathToFileNas + date.strftime('%d-%m-%Y.%H'))

	camera.stop_recording(splitter_port = 0)
	camera.close()
except Exception as ex:
	SendFirebaseLog(1, ex)
finally:
	SendFirebaseLog(0, 'Program End')