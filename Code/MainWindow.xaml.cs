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

            const int VK_PG1 = 0x22;
            const int VK_PG2 = 0x21;
            const int VK_CNTL = 0xA2;
            const int VK_LFT = 0x25;
            const int VK_RGT = 0x27;
            const int VK_UP = 0x26;
            const int VK_DWN = 0x28;

            int[] keys = { VK_PG1, VK_PG2, VK_CNTL, VK_LFT, VK_RGT, VK_UP, VK_DWN };

            foreach (int key in keys)
                KeyPressEmulator.setKeyPressed(key, false);

            // TODO: Changed GesturePartResult to GestureResult
            if (handsOut.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(VK_PG1, true);
                //Console.WriteLine("Detected zoom in");

            }
            else if (handsIn.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(VK_PG2, true);
                //Console.WriteLine("Detected zoom out");
            }
            else if (rotateLR.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(VK_CNTL, true);
                KeyPressEmulator.setKeyPressed(VK_RGT, true);
                //Console.WriteLine("Detected rotate CW");
            }
            else if (rotateRL.CheckGesture(first) == GestureResult.Succeed)
            {
                KeyPressEmulator.setKeyPressed(VK_CNTL, true);
                KeyPressEmulator.setKeyPressed(VK_LFT, true);
                //Console.WriteLine("Detected rotate CCW");
            }
            else
            {
                //Console.WriteLine("Nothing detected");
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
