using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using Kinect.GoogleEarth;

namespace kinect_sdk_example
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;

        public MainWindow()
        {
            // Init window
            // TODO: Make windows not open at all
            InitializeComponent();
            // Fail silently if no kinect
            //if (KinectSensor.KinectSensors.Count == 0)
            //    return;

            kinect = KinectSensor.KinectSensors[0];

            // Add the looper handler to the kinect
            kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);

            // Enable listening for skeletons
            kinect.SkeletonStream.Enable();

            // Start listening
            kinect.Start();
        }

        private void testZoomInAndOutKeys()
        {
            KeyPressEmulator.setKeyPressed(0x21, true);
            Thread.Sleep(5000);
            KeyPressEmulator.setKeyPressed(0x21, false);
            Thread.Sleep(5000);
            KeyPressEmulator.setKeyPressed(0x22, true);
            Thread.Sleep(5000);
            KeyPressEmulator.setKeyPressed(0x22, false);
            Thread.Sleep(5000);
        }

        private void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            Skeleton first = GetFirstSkeleton(e);

            HandsOut handsOut = new HandsOut();
            HandsIn handsIn = new HandsIn();
            RotateLeftOverRight rotateLR = new RotateLeftOverRight();
            RotateRightOverLeft rotateRL = new RotateRightOverLeft();

            // page0, page1, cntrl2, left3, right4, up5, down6
            int[] keys = { 0x22, 0x21, 0xA2, 0x25, 0x27, 0x26, 0x28 };

            foreach (int key in keys)
                KeyPressEmulator.setKeyPressed(key, false);

            // TODO: Changed GesturePartResult to GestureResult
            if (handsOut.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(keys[0], true);
                Console.WriteLine("Detected zoom in");

            }
            else if (handsIn.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(keys[1], true);
                Console.WriteLine("Detected zoom out");
            }
            else if (rotateLR.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(keys[2], true);
                KeyPressEmulator.setKeyPressed(keys[4], true);
                Console.WriteLine("Detected rotate CW");
            }
            else if (rotateRL.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(keys[2], true);
                KeyPressEmulator.setKeyPressed(keys[3], true);
                Console.WriteLine("Detected rotate CCW");
            }
            else
            {
                Console.WriteLine("Nothing detected");
            }



        }

        private Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }

                Skeleton[] allSkeletons = new Skeleton[6];
                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get first skeleton tracked
                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();
                return first;
            }
        }
    }
}
