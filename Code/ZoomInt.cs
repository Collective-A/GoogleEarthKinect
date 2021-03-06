﻿using System;
using Microsoft.Kinect;

namespace Kinect.GoogleEarth
{
    class ZoomInit : BaseGesture
	{
        GestureResult CheckGesture(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return GesturePartResult.Fail;
            }
            
            if (skeleton.Joints[JointType.HandLeft].Position.X == skeleton.Joints[JointType.ElbowLeft].Position.X)
            {
                if (skeleton.Joints[JointType.HandRight].Position.X == skeleton.Joints[JointType.ElbowRight].Position.X)
                {
                    return GesturePartResult.Succeed;
                }
            }

            // hands dropped
            return GesturePartResult.Fail;
        }
    }
	}
}
