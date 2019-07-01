import os, json, time, subprocess, datetime as dt

# Location of Project Path
__location__ = os.path.realpath(os.path.join(os.getcwd(), os.path.dirname(__file__))) + os.sep

try:
    with open(__location__ + 'settings.json', 'r') as settingsFile:
        settings = json.load(settingsFile)
    
    with open(settings['PathToLocalProject'] + 'text.json', 'r') as timeFile:
        timeFileJson = json.load(timeFile)
    
    while True:
        timeFileJson[0]['text_line'] = dt.datetime.now().strftime('%d-%m-%Y %H:%M:%S')
        with open(settings['PathToTemp'] + 'text.json', 'w') as timeFile:
            json.dump(timeFileJson, timeFile)
        
        subprocess.Popen('v4l2-ctl --set-ctrl=text_overlay=1 --device=/dev/uv4l-camera', shell=True)
        
        time.sleep(1)
except Exception as ex:
    print(ex)
finally:
    pass