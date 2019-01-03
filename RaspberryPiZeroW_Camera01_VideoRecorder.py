import time
import datetime as dt 
import picamera
import glob
import subprocess
import asyncio

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
	StartStream()
	
	#i = 0
	#for i in range(5):
	while True:
		prevFileName = fileName
		date = dt.datetime.today() - dt.timedelta(hours = 3)
		camera.wait_recording(3600)
		fileName = getCurrentDateToString(False) + '.h264'
		camera.split_recording(pathToFileLocal + fileName, splitter_port = 0)

		Mp4Box(prevFileName)
		DeleteFileAfter24H(pathToFileNas + date.strftime('%d-%m-%Y %H'))

	camera.stop_recording(splitter_port = 0)
	camera.close()
except:
	fileLog.write(getCurrentDateToString(True) + ' --- Error')
finally:
	fileLog.write(getCurrentDateToString(True) + ' --- Finish')
	fileLog.close()
