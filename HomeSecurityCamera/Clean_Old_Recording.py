import glob, os, time
import datetime as dt
import subprocess

PathFileToNas = '/mnt/SecurityCam/Cam01/Recordings/'

try:
    while True:
        fileList = glob.glob(PathFileToNas + "*")
        
        currentDateTime = dt.datetime.now()

        for file in fileList:
            fileName = ''
            
            if file.endswith('.mp4'):
                fileName = file[:-4]
            
            fileNameDateTime = dt.datetime.strptime(fileName, '%d-%m-%Y_%H-%M-%S')
            
            if fileNameDateTime <= currentDateTime - dt.timedelta(hours = 24):
                os.remove(file)
        
        if(os.path.ismount('/mnt/SecurityCam/Cam01/')):
            fileList = glob.glob('/home/pi/HomeSecurityCamera/Recordings/*')
            fileList = sorted(fileList)
            del fileList[-1]
            for file in fileList:
                subprocess.Popen('mv /home/pi/HomeSecurityCamera/Recordings/' + file + ' /mnt/SecurityCam/Cam01/Recordings/', shell=True)

        time.sleep(3600)
except Exception as ex:
    print (ex)
finally:
    pass