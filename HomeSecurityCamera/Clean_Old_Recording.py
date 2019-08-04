import glob, os, time, json, subprocess, datetime as dt

# Location of Project Path
__location__ = os.path.realpath(os.path.join(os.getcwd(), os.path.dirname(__file__))) + os.sep

try:
    with open(__location__ + 'settings.json', 'r') as settingsFile:
        settings = json.load(settingsFile)
    
    while True:
        fileList = glob.glob(settings['PathToNas'] + "*")
        currentDateTime = dt.datetime.now()

        for file in fileList:
            fileName = ''
            
            if file.endswith('.mp4'):
                fileName = file[:-4]
            
            fileNameDateTime = dt.datetime.strptime(fileName, '%d-%m-%Y_%H-%M-%S')
            
            if fileNameDateTime <= currentDateTime - dt.timedelta(hours = 24):
                os.remove(file)
        
        if os.path.ismount(settings['PathToNas']):
            fileList = glob.glob(settings['PathToLocalRecordings'] + '*')
            fileList = sorted(fileList)
            del fileList[-1]
            for file in fileList:
                subprocess.Popen('mv' + settings['PathToLocalRecordings'] + file + ' ' + settings['PathToNasRecordings'], shell=True)

        time.sleep(3600)
except Exception as ex:
    print (ex)
finally:
    pass