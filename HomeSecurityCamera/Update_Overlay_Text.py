import os, json, time, subprocess, datetime as dt

hasSettingsLoaded = False

# Location of Project Path
__location__ = os.path.realpath(os.path.join(os.getcwd(), os.path.dirname(__file__))) + os.sep

#Load SettingFile
try:
    with open(__location__ + 'settings.json', 'r') as settingsFile:
        settings = json.load(settingsFile)
        hasSettingsLoaded = True
except Exception as ex:
    hasSettingsLoaded = False

#If Settings File Has Loaded Start Program
if hasSettingsLoaded:
    try:
        with open(settings['PathToLocalProject'] + 'text.json', 'r') as timeFile:
            timeFileJson = json.load(timeFile)
        
        temperatureCounter = 0

        while True:
            timeFileJson[0]['text_line'] = dt.datetime.now().strftime('%d-%m-%Y %H:%M:%S')
            
            if temperatureCounter == 10:
                temperature = os.popen("vcgencmd measure_temp").readline()
                temperature = temperature.replace('temp=', '')
                temperature = temperature.replace('\n', '')
                timeFileJson[1]['text_line'] = temperature
                temperatureCounter = 0
            
            with open(settings['PathToTemp'] + 'text.json', 'w') as timeFile:
                json.dump(timeFileJson, timeFile)
            
            subprocess.Popen('v4l2-ctl --set-ctrl=text_overlay=1 --device=/dev/uv4l-camera', shell=True)
            temperatureCounter = temperatureCounter + 1
            time.sleep(0.5)
    except Exception as ex:        
        with open(settings['PathToLogs'] + 'Update_Overlay_Text.log', 'a') as logFile:
            logFile.write(str(ex) + '\n')
    finally:
        pass