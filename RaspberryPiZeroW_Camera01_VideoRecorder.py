import time
import datetime as dt 
import picamera
import glob
import subprocess
import asyncio
import threading
import socket
import firebase_admin
from firebase_admin import db
from firebase_admin import credentials

class StreamManagerThread(object):
	def __init__(self, interval = 2):
		self.interval = interval

		thread = threading.Thread(target=self.run, args=())
		thread.daemon = True
		thread.start()

	def run(self):
		try:
			while True:
				# Wait for an Incoming Connection, then select a splitter_port free to stream.
				print('StreamManagerThread, Waiting for Connection...')
				serverSocketConnection = server_socket.accept()
				print('Connection Requested by ' + serverSocketConnection[1])
				SendFirebaseLog(0, 'Connection Requested By: ' + serverSocketConnection[1])
				connection = serverSocketConnection[0].makefile('wb')
				
				for count in range(0, len(connectionArray)):
					if connectionArray[count] != 0:
						connectionNumber = connectionArray[count]
						connectionArray[count] = 0
						break
				
				StreamThread(connection, connectionNumber)
		except Exception as ex:
			SendFirebaseLog(1, ex)
		finally:
			connection.close()

# Stream over the connection passed by until client disconnect
class StreamThread(object):
	def __init__(self, connection, connectionNumber):
		self.interval = 2
		self.connection = connection
		self.connectionNumber = connectionNumber

		thread = threading.Thread(target=self.run, args=())
		thread.daemon = True
		thread.start()

	def run(self):
		try:
			print('StreamThread')
			SendFirebaseLog(0, 'Start Streaming')
			camera.start_recording(self.connection, format='h264', splitter_port = self.connectionNumber)
			while True:
				camera.wait_recording(2, splitter_port = self.connectionNumber)
				pass
		except Exception as ex:
			SendFirebaseLog(1, ex)
		finally:
			try:
				camera.stop_recording(splitter_port = self.connectionNumber)
				self.connection.close()
				connectionArray[self.connectionNumber -1] = self.connectionNumber
			except:
				#Do Nothing
				pass
			finally:
				#Do Nothing
				pass

		time.sleep(self.interval)

def getCurrentDateToString(isLog):
	if(isLog == True):
		return dt.datetime.now().strftime('%d-%m-%Y.%H-%M-%S')
	else:
		return dt.datetime.now().strftime('%d-%m-%Y.%H-%M')

def SendFirebaseLog(errorType, errorMessage):
	try:
		refChild.update({getCurrentDateToString(True) : {"ErrorType" : errorType, "ErrorMessage" : errorMessage}})
	except Exception as ex:
		pass

def DeleteFileAfter24H(fileName):
	try:
		SendFirebaseLog(0, 'Searching For File')
		fileList = glob.glob(pathToFileNas + "*")
		
		for file in fileList:
			if(file[ : len(file) - 8] == fileName[ : len(fileName)]):
				SendFirebaseLog(0, 'Deleting File ' + file)
				os.remove(file)
	except Exception as ex:
		SendFirebaseLog(1, ex)	
	return

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
	time.sleep(1)
	
	SendFirebaseLog(0, 'Start Recording')

	camera.start_recording(pathToFileLocal + fileName, splitter_port = 0)
	StreamManagerThread()
	
	while True:
		prevFileName = fileName
		date = dt.datetime.today() - dt.timedelta(hours = 3)
		camera.wait_recording(3600, splitter_port = 0)
		fileName = getCurrentDateToString(False) + '.h264'

		camera.split_recording(pathToFileLocal + fileName, splitter_port = 0)

		Mp4Box(prevFileName)
		DeleteFileAfter24H(pathToFileNas + date.strftime('%d-%m-%Y %H'))

	camera.stop_recording(splitter_port = 0)
	camera.close()
except Exception as ex:
	SendFirebaseLog(1, ex)
finally:
	SendFirebaseLog(0, 'Program End')