import datetime as dt, glob, json, os, socket, subprocess, threading, time

import firebase_admin
from firebase_admin import credentials, db
import MailService
import numpy as np
import picamera
import picamera.array

# Return the SplitterPort that is not being used
def CalculateVideoPortFree():
	for count in range(0, len(connectionArray)):
		if connectionArray[count] != 0:
			splitter_port = connectionArray[count]
			connectionArray[count] = 0
			return splitter_port

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
				connection, address = server_socket.accept()
				SendFirebaseLog(0, 'Connection Requested By: ' + str(address[0]))
				
				StreamThread(connection.makefile('wb'), CalculateVideoPortFree(), str(address[0]))
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

# Analyze and Detect Movement
class MotionDetector(picamera.array.PiMotionAnalysis):
    def __init__(self, camera, handler):
        super(MotionDetector, self).__init__(camera)
        self.handler = handler
        self.first = True

    # Method called after each frame is ready for processing.
    def analyse(self, a):
        a = np.sqrt(
            np.square(a['x'].astype(np.float)) +
            np.square(a['y'].astype(np.float))
        ).clip(0, 255).astype(np.uint8)
        # If there are 50 vectors detected with a magnitude of 60 We consider movement to be detected.
        if (a > 60).sum() > 50:
            if self.first:
                self.first = False
                return
            self.handler.motion_detected()

# Handle Movement Detected by Taking a Capture and sending Email
class MotionHandler:
	def __init__(self, camera, post_capture_callback=None):
		self.camera = camera
		self.callback = post_capture_callback
		self.detected = False
		self.working = False

		self.captureName = 'movement_temp.jpeg'
		self.mailService = MailService.eMailService()

	def motion_detected(self):
		if not self.working:
			self.detected = True

	def tick(self):
		if self.detected:
			self.working = True
			self.detected = False

			splitterPort = CalculateVideoPortFree()
			self.camera.capture(self.captureName, format='jpeg', splitter_port = splitterPort)
			connectionArray[splitterPort -1] = splitterPort

			self.mailService.SendMail(self.captureName, 'Alert')
			os.remove(self.captureName)
			self.working = False

def getCurrentDateToString(isLog):
	if(isLog == True):
		return dt.datetime.now().strftime('%d%m%Y-%H:%M:%S')
	else:
		return dt.datetime.now().strftime('%d-%m-%Y.%H-%M')

def SendFirebaseLog(errorType, errorMessage):
	try:
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
default_app = firebase_admin.initialize_app(cred, {'databaseURL': 'https://homesucuritypp.firebaseio.com/'})

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

	handler = MotionHandler(camera)

	camera.start_recording(pathToFileLocal + fileName, splitter_port = 0, motion_output = MotionDetector(camera, handler))
	StreamManagerThread()

	while True:
		whileDate1 = dt.datetime.today()
		whileDate2 = whileDate1 + dt.timedelta(hours = 1)

		prevFileName = fileName
		date = dt.datetime.today() - dt.timedelta(hours = 12)
		while(whileDate1 < whileDate2):
			camera.annotate_text = getCurrentDateToString(True)
			if dt.datetime.today().hour < 6:
				handler.tick()
		
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