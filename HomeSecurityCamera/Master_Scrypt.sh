#Master_Script
#Script used to start all the other script/program

#Create Camera Driver Node
uv4l --sched-rr --driver raspicam --n --device-name uv4l-camera --framerate 24 --encoding=h264 --width=1280 --height=720 --rotation=180 --extension-presence=0 --text-overlay --text-filename=/tmp/text.json

python3 /home/pi/HomeSecurityCamera/Update_Overlay_Text.py &

su pi /home/pi/HomeSecurityCamera/Stream.sh &

sleep 5

su pi /home/pi/HomeSecurityCamera/Record.sh &

python3 /home/pi/HomeSecurityCamera/Clean_Old_Recording.py &