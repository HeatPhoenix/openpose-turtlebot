sudo ifconfig eth0 169.254.80.44 broadcast 169.254.255.255 netmask 255.255.0.0
./json_script.sh &
./build/examples/openpose/openpose.bin --model_folder ./models -write_json ./json --display 0 --render_pose 0 --net_resolution 64x32 


