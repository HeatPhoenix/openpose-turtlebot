using System;
using System.IO;
using SimpleJSON;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace parser
{
	class MainClass
	{
		static String[] parts = new string[26]{"Nose", "Neck", 
			"RShoulder", "RElbow", "RWrist", "LShoulder", "LElbow", "LWrist", 
			"MidHip", "RHip", "RKnee", "RAnkle", "LHip", "LKnee", "LAnkle", 
			"REye", "LEye", "REar", "LEar", 
			"LBigToe", "LSmallToe", "LHeel", "RBigToe", "RSmallToe", "RHeel", 
			"Background"};

		const string savedJSON = "test.json";
		static bool onlyShoulderElbow = false;

		const int threshold = 10;

		static int shoulder_x, shoulder_y;
		static int elbow_x, elbow_y;
		
		public static void Main (string[] args)
		{
			if (args.Length > 1) {//filename + anything
				//print only shoulder and elbow
				onlyShoulderElbow = true;
			} else if (args.Length == 0) {//nothing
				onlyShoulderElbow = true;
				poseCheckLoop ();
			}

			if (args [0] != null) {//filename
				readSinglePose (args[0]);
			}
		}

		public static void readSinglePose(string filename)
		{
			Console.WriteLine ("Body part: X - Y - Confidence");
			string json = File.ReadAllText("json/" + filename);
			JSONNode n = JSON.Parse (json);
			printJSON (n, true);	
			Console.WriteLine ("Angle: " + calculateAngle ());
		}

		public static double calculateAngle ()
		{
			float xDiff = shoulder_x - elbow_x;
			float yDiff = shoulder_y - elbow_y;
			double angle = Math.Atan2 (yDiff, xDiff) * 180.0 / Math.PI;
			Console.WriteLine ("Calculated angle: " + angle);
			if ((shoulder_x == 0 && shoulder_y == 0) || (elbow_x == 0 && elbow_y == 0))
				return 999;
			else
				return angle;
		}

		public static void poseCheckLoop()
		{			
			while (true) {
				string[] filename = Directory.GetFiles("json/");
				if (filename.Length > 0) {
					Console.WriteLine (filename[0]);
					string json = File.ReadAllText(filename[0]);
					JSONNode n = JSON.Parse (json);
					if (n == null) {
						Console.WriteLine (filename[0] + " is empty");
						File.Delete (filename [0]);
						continue;
					}
					printJSON (n, true);
					isWithinRangeWithThreshold (calculateAngle (), 0);//0 degrees to arm at straight angle
					File.Delete (filename[0]);
				}
				Thread.Sleep (1);
			}
		}

		static bool isWithinRangeWithThreshold(double value, double target)
		{
			value = Math.Abs (value); target = Math.Abs (target);
			if(value <= (target + threshold) && value >= (target - threshold))
			{
				Console.WriteLine ("Target angle reached");
				Process.Start ("/bin/bash", "rosrun demo move.py");
			}
			return true;
		}

		//     {0,  "Nose"},
		//     {1,  "Neck"},
		//     {2,  "RShoulder"},
		//     {3,  "RElbow"},
		//     {4,  "RWrist"},
		//     {5,  "LShoulder"},
		//     {6,  "LElbow"},
		//     {7,  "LWrist"}
		//     {8,  "MidHip"},
		//     {9,  "RHip"},
		//     {10, "RKnee"},
		//     {11, "RAnkle"},
		//     {12, "LHip"},
		//     {13, "LKnee"},
		//     {14, "LAnkle"},
		//     {15, "REye"},
		//     {16, "LEye"},
		//     {17, "REar"},
		//     {18, "LEar"},
		//     {19, "LBigToe"},
		//     {20, "LSmallToe"},
		//     {21, "LHeel"},
		//     {22, "RBigToe"},
		//     {23, "RSmallToe"},
		//     {24, "RHeel"},
		//     {25, "Background"}
		public static void printJSON(JSONNode n, bool print)
		{
			for (int i = 0; i < 25; i++) {
				if (onlyShoulderElbow && (i == 2 || i == 3)) {
					if(print)
						Console.WriteLine (parts [i] + ": " + n ["people"] [0] ["pose_keypoints_2d"] [i*3 + 0] + " " + n ["people"] [0] ["pose_keypoints_2d"] [i*3 + 1] + " " + n ["people"] [0] ["pose_keypoints_2d"] [i*3 + 2]); 
					if (i == 2) {
						shoulder_x = n ["people"] [0] ["pose_keypoints_2d"] [i * 3 + 0];
						shoulder_y = n ["people"] [0] ["pose_keypoints_2d"] [i * 3 + 1];
					}

					if (i == 3) {
						elbow_x = n ["people"] [0] ["pose_keypoints_2d"] [i * 3 + 0];
						elbow_y = n ["people"] [0] ["pose_keypoints_2d"] [i * 3 + 1];
					}
				} else if (onlyShoulderElbow) {
					continue;
				} else {
					if(print)
						Console.WriteLine (parts [i] + ": " + n ["people"] [0] ["pose_keypoints_2d"] [i*3 + 0] + " " + n ["people"] [0] ["pose_keypoints_2d"] [i*3 + 1] + " " + n ["people"] [0] ["pose_keypoints_2d"] [i*3 + 2]); 
				}
			}
		}


	}
}
