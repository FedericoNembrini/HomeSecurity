#Takes data from the device node specified on 1Megabyte Block and supply data to vlc video streaming.
while :
do
    dd if=/dev/uv4l-camera bs=1M | cvlc stream:///dev/stdin --sout '#rtp{sdp=rtsp://:8554/}' --demux=h264 --h264-fps=24
    sleep 3m
done
