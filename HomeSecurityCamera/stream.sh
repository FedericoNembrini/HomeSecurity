#Takes data from the device node specified on 1Megabyte Block and supply data to vlc video streaming.
#dd documentation: http://man7.org/linux/man-pages/man1/dd.1.html
while :
do
    dd if=/dev/uv4l-camera bs=1M status=none conv=noerror iflag=nocache oflag=nocache 2>> /mnt/SecurityCam/Cam01/Logs/dd.log | cvlc stream:///dev/stdin --sout '#rtp{sdp=rtsp://:8554/}' --demux=h264 --h264-fps=24 2>> /mnt/SecurityCam/Cam01/Logs/cvlc.log
    sleep 3m
done