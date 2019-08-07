#!/usr/bin/env python
import rospy
import sys
from geometry_msgs.msg import Twist

# Starts a new node
rospy.init_node('demo', anonymous=True)
velocity_publisher = rospy.Publisher('cmd_vel', Twist, queue_size=10)
vel_msg = Twist()

distance = 1
speed = 1

#while (velocity_publisher.get_num_connections() < 1):
#    print("Waiting")

vel_msg.linear.x = 0.1
#while True:    
#    print(vel_msg)
#    velocity_publisher.publish(vel_msg) 
#    print("published");

#Since we are moving just in x-axis
vel_msg.linear.y = 0
vel_msg.linear.z = 0
vel_msg.angular.x = 0
vel_msg.angular.y = 0
vel_msg.angular.z = 0



while not rospy.is_shutdown():
    

    #Setting the current time for distance calculus
    t0 = rospy.Time.now().to_sec()
    current_distance = 0

    #Loop to move the turtle in an specified distance
    while(current_distance < distance):
        #Publish the velocity
        velocity_publisher.publish(vel_msg) 
        #Takes actual time to velocity calculus
        t1=rospy.Time.now().to_sec()
        #Calculates distancePoseStamped
        current_distance= speed*(t1-t0)
    #After the loop, stops the robot
    vel_msg.linear.x = 0
    #Force the robot to stop
    velocity_publisher.publish(vel_msg)
    sys.exit();
