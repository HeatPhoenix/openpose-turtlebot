files=(./json/)
while :
do
	for f in ./json/*.json; 
	do	
		if [ "$f" != "./json/*.json" ];
		then
		echo "Processing from $f"
		sshpass -p "edalab" scp ./json/* pi@169.254.118.6:~/json/
		echo "Transferred files"
		rm ./json/*.json
		fi
	done
	
	sleep 0.01s
done
