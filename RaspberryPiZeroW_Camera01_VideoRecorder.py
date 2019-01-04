import time
import datetime as dt 
import picamera
import glob
import subprocess
import asyncio
import threading
import socket

class StreamThread(object):
	def __init__(self, interval=2):
		self.interval = interval

		thread = threading.Thread(target=self.run, args=())
		thread.daemon = True
		thread.start()

	def run(self):
		while True:
		    # Do something
			connection = server_socket.accept()[0].makefile('wb')
			print("Connection Recivied")
			try:
				fileLog.write(getCurrentDateToString(True) + " --- Start Streaming")
				camera.start_recording(connection, format='h264', splitter_port = 1)
				print("Camera Streaming")
				camera.wait_recording(3600, splitter_port = 1)
			except:
				fileLog.write(getCurrentDateToString(True) + " --- Error in Streaming\n")
			finally:
				try:
					camera.stop_recording(splitter_port = 1)
					connection.close()
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

def DeleteFileAfter24H(fileName):
	try:
		fileLog.write(' --- Searching for file\n')
		fileList = glob.glob(pathToFileNas + "*")
		
		for file in fileList:
			if(file[ : len(file) - 8] == fileName[ : len(fileName)]):
				fileLog.write(getCurrentDateToString(True) + ' --- Deleting File\n')
				os.remove(file)
	except:
		fileLog.write(getCurrentDateToString(True) + ' --- Error Delete\n')
	finally:
		return

def Mp4Box(fileName):
	command = "MP4Box -add {} {} ; rm {}".format(pathToFileLocal + fileName, pathToFileNas + fileName.replace(".h264", ".mp4"), pathToFileLocal + fileName)
	try:
		subprocess.Popen(command, shell=True)
	except:
		fileLog.write(getCurrentDateToString(True) + " --- Error Mp4Box\n")

#Program Start
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(("0.0.0.0", 8000))
server_socket.listen(0)

fileLog = open('/home/pi/Projects/log/log.txt', 'a')
fileLog.write(getCurrentDateToString(True) + ' --- Initializing\n')

pathToFileLocal = '/home/pi/Projects/'
pathToFileNas = '/mnt/SecurityCam/Cam01/Recordings/'
fileName = getCurrentDateToString(False) + '.h264'

try:
	fileLog.write(getCurrentDateToString(True) + ' --- Camera Initializing\n')
	camera = picamera.PiCamera(resolution = (1270, 720), framerate = 30)
	camera.rotation = 180
	time.sleep(1)
	
	fileLog.write(getCurrentDateToString(True) + ' --- Start Recording\n')
	camera.start_recording(pathToFileLocal + fileName, splitter_port = 0)
	StreamThread()
	
	while True:
		prevFileName = fileName
		date = dt.datetime.today() - dt.timedelta(hours = 3)
		camera.wait_recording(3600, splitter_port = 0)
		fileName = getCurrentDateToString(False) + '.h264'
		print("test")

		camera.split_recording(pathToFileLocal + fileName, splitter_port = 0)

		Mp4Box(prevFileName)
		DeleteFileAfter24H(pathToFileNas + date.strftime('%d-%m-%Y %H'))

	camera.stop_recording(splitter_port = 0)
	camera.close()
except:
	fileLog.write(getCurrentDateToString(True) + ' --- Error\n')
finally:
	fileLog.write(getCurrentDateToString(True) + ' --- Finish\n')
	fileLog.close()
