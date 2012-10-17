using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Kinect.GoogleEarth
{
    class RotateLeftOverRight : BaseGesture
    {
        override public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }

            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ElbowLeft].Position.Y && // left hand higher than left elbow
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ShoulderLeft].Position.Y && // left hand higher than left shoulder
                skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ShoulderLeft].Position.X) // left hand further out than left shoulder
            {
                if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ElbowRight].Position.Y && // right hand lower than right elbow
                    skeleton.Joints[JointType.ElbowRight].Position.Y < skeleton.Joints[JointType.ShoulderRight].Position.Y && // right hand lower than right shoulder
                    skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ShoulderRight].Position.X) // right hand further out than right shoulder
                {
                    return GestureResult.Succeed;
                }
            }

            // hands dropped
            return GestureResult.Fail;
        }
    }

    class RotateRightOverLeft : BaseGesture
    {
        override public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }

            if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ElbowRight].Position.Y && // right hand higher than right elbow
                    skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ShoulderRight].Position.Y && // right hand higher than right shoulder
                    skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ShoulderRight].Position.X) // right hand further out than right shoulder
            {
                if (skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ElbowLeft].Position.Y && // left hand lower than left elbow
                    skeleton.Joints[JointType.ElbowLeft].Position.Y < skeleton.Joints[JointType.ShoulderLeft].Position.Y && // left hand lower than left shoulder
                    skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ShoulderLeft].Position.X) // left hand further out than left shoulder
                {
                    return GestureResult.Succeed;
                }
            }

            // hands dropped
            return GestureResult.Fail;
        }
    }

}