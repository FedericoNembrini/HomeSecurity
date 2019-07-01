#Takes data from the device node specified on 1Megabyte Block and supply data to vlc video streaming.
dd if=/dev/uv4l-camera bs=1M | cvlc stream:///dev/stdin --sout '#rtp{sdp=rtsp://:8554/}' --demux=h264 --h264-fps=24
