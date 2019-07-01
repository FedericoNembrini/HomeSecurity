import glob, os
import datetime as dt

PathFileToNas = '/mnt/SecurityCam/Cam01/Recordings/'

try:
    fileList = glob.glob(PathFileToNas + "*")
    
    currentDateTime = dt.datetime.now()

    for file in fileList:
        fileName = ''
        
        if file.endswith('.mp4'):
            fileName = file[:-4]
        
        fileNameDateTime = dt.datetime.strptime(fileName, '%d-%m-%Y_%H-%M-%S')
        
        if fileNameDateTime <= currentDateTime - dt.timedelta(hours = 24):
            os.remove(file)
except Exception as ex:
    print (ex)
finally:
    pass