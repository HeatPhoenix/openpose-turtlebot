# openpose-turtlebot
A configuration combining the Turtlebot3 with a Jetson Nano for pose estimation and gesture control

There are several folders at the base of the trunk. One for each of the computer systems involved in the project.
* Jetson Nano
	Handles the computer vision and machine learning aspect of the project.
* Raspbian 
	Handles the interpreting of the poses.
* OpenCR
	Handles the ROS (Robot Operating System) tasks.
* Ubuntu16.04
	Acts as the x86_64 host computer.
 

The Parser subfolder/project in the Raspbian folder is for parsing .json files that are output by OpenPose, written in C#/Mono
