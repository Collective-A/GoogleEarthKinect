using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Kinect.GoogleEarth
{
    class ZoomInit : BaseGesture
	{
        public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }
            
            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipLeft].Position.Y && 
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.Head].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderLeft].Position.Y)        
            {
                if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipRight].Position.Y && 
                    skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.Head].Position.Y &&
                    skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderRight].Position.Y)
                  {
                    return GestureResult.Succeed;
                  }
            }
            
            // hands dropped
            return GestureResult.Fail;
        }
    }

    class ZoomOutInit : BaseGesture
    {
        public GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GestureResult.Fail;
            }

            if (skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.ElbowLeft].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipLeft].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.Head].Position.Y)
            {
               if (skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ElbowRight].Position.X&&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipRight].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.Head].Position.Y) 
                {
                    return GestureResult.Succeed;
                }
            }

            // hands dropped
            return GestureResult.Fail;
        }
    }
}
