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
using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Recognition;

namespace kinect_sdk_example
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;
        string mediaPath;

        public MainWindow()
        {
            // Init window
            // TODO: Make windows not open at all
            this.InitializeComponent();
            mediaPath = Directory.GetCurrentDirectory();
        }

        private void LaunchKinect()
        {
            // Fail silently if no kinect
            if (KinectSensor.KinectSensors.Count == 0)
            {
                Console.WriteLine("Couldn't find a Kinect");
                return;
            }
            
            /*
            #region Speech Reco
            RecognizerInfo ri = GetKinectRecognizer();

            if (null != ri)
            {
                ri.speechEngine = new SpeechRecognitionEngine(ri.Id);

                // Create a grammar from grammar definition XML file.
                using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
                {
                    var g = new Grammar(memoryStream);
                    speechEngine.LoadGrammar(g);
                }

                speechEngine.SpeechRecognized += SpeechRecognized;
                speechEngine.SpeechRecognitionRejected += SpeechRejected;

                speechEngine.SetInputToAudioStream(
                    sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }

            #endregion
             * */

            kinect = KinectSensor.KinectSensors[0];

            // Add the looper handler to the kinect
            kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);

            // Enable listening for skeletons
            kinect.SkeletonStream.Enable();

            // Start listening
            kinect.Start();
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase)
                    && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private void CallCommandLine(string process, string arguments)
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = process;
            pProcess.StartInfo.Arguments = arguments;
            pProcess.StartInfo.UseShellExecute = false;
            Console.WriteLine(process + " " + arguments);
            pProcess.Start();
        }

        private void ClickPan(object sender, System.Windows.RoutedEventArgs e)
        {
            string panFilename = "jingles.mp4";
            CallCommandLine("\"C:\\Program Files\\Windows Media Player\\wmplayer.exe\"", String.Format("/play \"{0}\\{1}\"", mediaPath, panFilename));
        }

        private void ClickZoom(object sender, System.Windows.RoutedEventArgs e)
        {
            string zoomFilename = "jingles.mp4";
            CallCommandLine("\"C:\\Program Files\\Windows Media Player\\wmplayer.exe\"", String.Format("/play \"{0}\\{1}\"", mediaPath, zoomFilename));
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void ClickOkay(object sender, System.Windows.RoutedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("googleearth");
            if (processes.Length == 0)
            {
                CallCommandLine(@"C:\Program Files (x86)\Google\Google Earth\client\googleearth.exe", String.Empty);
            }
            else
            {
                SetForegroundWindow(processes[0].MainWindowHandle);
            }
            LaunchKinect();
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

            Dictionary<BaseGesture, int> gestures = new Dictionary<BaseGesture, int>();
            gestures.Add(new HandsOut(), 0x22);
            gestures.Add(new HandsIn(), 0x21);
            gestures.Add(new PanUp(), 0x26);
            gestures.Add(new PanDown(), 0x28);
            gestures.Add(new PanLeft(), 0x25);
            gestures.Add(new PanRight(), 0x27);

            foreach (BaseGesture gesture in gestures.Keys)
                KeyPressEmulator.setKeyPressed(gestures[gesture], false);

            foreach (BaseGesture gesture in gestures.Keys)
            {
                if (gesture.CheckGesture(first) == GestureResult.Success)
                {
                    KeyPressEmulator.setKeyPressed(gestures[gesture], true);
                    break;
                }
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
