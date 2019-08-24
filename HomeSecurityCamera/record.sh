# Takes Video Input from Url, Peform a Copy Only (Demux and Mix) of the Stream Into 1 Hours Video File.
while :
do
    #ffmpeg -r 24 -i rtsp://localhost:8554/ -c copy -map 0 -f segment -strftime 1 -segment_time 2700 -segment_format mp4 -r 24 /home/pi/HomeSecurityCamera/Recordings/%d-%m-%Y_%H-%M-%S.mp4 2>> /mnt/SecurityCam/Cam01/Logs/ffmpeg.log
    ffmpeg -r 24 -i rtsp://localhost:8554/ -c copy -map 0 -f segment -strftime 1 -segment_time 2700 -segment_format mp4 -r 24 /mnt/SecurityCam/Cam01/Recordings/%d-%m-%Y_%H-%M-%S.mp4 2>> /mnt/SecurityCam/Cam01/Logs/ffmpeg.log    
    sleep 3m
done