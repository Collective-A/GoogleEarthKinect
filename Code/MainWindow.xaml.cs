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
            if (KinectSensor.KinectSensors.Count == 0)
                return;

            kinect = KinectSensor.KinectSensors[0];

            // From Japanese base project
            // - - - - -
            //kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(handler_SkeletonFrameReady);
            // - - - - -

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

            ZoomInit zoomInit = new ZoomInit();

            // TODO: Changed GesturePartResult to GestureResult
            if (zoomInit.CheckGesture(first) == GestureResult.Succeed)
            {

                //Do something

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

        
        /**TODO: Delete Japanese code 
         * 
         */
        //void handler_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e) {
        //    SkeletonFrame temp = e.OpenSkeletonFrame();
        //    if (temp != null)
        //    {
        //        Skeleton[] skeletonData = new Skeleton[temp.SkeletonArrayLength];
        //        temp.CopySkeletonDataTo(skeletonData);
        //        SkelPoints.Text = "";
        //        foreach (Skeleton skeleton in skeletonData)
        //        {
        //            if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
        //            {
        //                foreach (Joint joint in skeleton.Joints)
        //                {
        //                    SkelPoints.Text += joint.JointType.ToString() + "\t";
        //                    SkelPoints.Text += joint.Position.X + "\t";
        //                    SkelPoints.Text += joint.Position.Y + "\t";
        //                    SkelPoints.Text += joint.Position.Z + "\n";

        //                }
        //            }
        //        }
        //    }
        //}
    }
}
