using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Kinect.GoogleEarth
{

    class PanUp : BaseGesture
    {


        override public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }

            if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ShoulderRight].Position.Y * BaseGesture.PAN_PADDING_SCALE &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipLeft].Position.Y)
            {
                return GestureResult.Success;
            }

            // hands dropped
            return GestureResult.Fail;
        }
    }

    class PanDown : BaseGesture
    {
        override public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }

            if (skeleton.Joints[JointType.HandRight].Position.Y * BaseGesture.PAN_PADDING_SCALE < skeleton.Joints[JointType.ShoulderRight].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipLeft].Position.Y && 
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipRight].Position.Y)
            {
                return GestureResult.Success;
            }

            // hands dropped
            return GestureResult.Fail;
        }
    }

    class PanRight : BaseGesture
    {
        override public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }

            if (skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ShoulderRight].Position.X * BaseGesture.PAN_PADDING_SCALE &&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipRight].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipLeft].Position.Y) 
            {
                return GestureResult.Success;
            }

            // hands dropped
            return GestureResult.Fail;
        }
    }

    class PanLeft : BaseGesture
    {
        override public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }

            if (skeleton.Joints[JointType.HandRight].Position.X * BaseGesture.PAN_PADDING_SCALE < skeleton.Joints[JointType.ShoulderRight].Position.X &&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipRight].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.ShoulderLeft].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipLeft].Position.Y)
            {
                return GestureResult.Success;
            }

            // hands dropped
            return GestureResult.Fail;
        }
    }
}
