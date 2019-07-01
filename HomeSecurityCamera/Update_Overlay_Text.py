import json, datetime as dt, time, subprocess

try:
    timeFile = open('/home/pi/HomeSecurityCamera/text.json', 'r')
    timeFileJson = json.load(timeFile)
    timeFile.close()
    
    while True:
        timeFileJson[0]['text_line'] = dt.datetime.now().strftime('%d-%m-%Y %H:%M:%S')
        timeFile = open('/tmp/text.json', 'w')
        json.dump(timeFileJson, timeFile)
        timeFile.close()
        subprocess.Popen('v4l2-ctl --set-ctrl=text_overlay=1 --device=/dev/uv4l-camera', shell=True)
        time.sleep(1)
except Exception as ex:
    print(ex)
finally:
    timeFile.close()