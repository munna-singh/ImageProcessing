using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakeOverApi.Models
{
    public class FaceRectangle
    {
        public int top { get; set; }
        public int left { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class HeadPose
    {
        public int pitch { get; set; }
        public double roll { get; set; }
        public double yaw { get; set; }
    }

    public class FacialHair
    {
        public double moustache { get; set; }
        public double beard { get; set; }
        public double sideburns { get; set; }
    }

    public class FaceAttributes
    {
        public HeadPose headPose { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public FacialHair facialHair { get; set; }
        public string glasses { get; set; }
    }

    public class FaceDataRequestDto
    {
        public string faceId { get; set; }
        public FaceRectangle faceRectangle { get; set; }
        public FaceAttributes faceAttributes { get; set; }
    }
}