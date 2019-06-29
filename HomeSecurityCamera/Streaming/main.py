import subprocess, json, datetime

try:
    StreamCommand = 'dd if=/dev/video2 bs=1M | cvlc stream:///dev/stdin --sout \'#rtp{sdp=rtsp://:8554/}\' --demux=h264 --h264-fps=24'
    subprocess.Popen(StreamCommand, shell=True)
    with open('/home/pi/time.json', 'w') as TimeFile:
        while True:
            jsonData = json.load(TimeFile)
            jsonData['text_line'] = datetime.datetime.now.strftime('%Y-%m-%d %h:%mm:%s')

            json.dump(jsonData, TimeFile)

            subprocess.Popen("v4l2-ctl --set-ctrl=text_overlay=1")
except Exception as ex:
    print(ex)