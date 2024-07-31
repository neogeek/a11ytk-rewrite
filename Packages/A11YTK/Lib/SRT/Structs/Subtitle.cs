using System;

namespace A11YTK.SRT
{

    [Serializable]
    public struct Subtitle
    {

        public int id;

        public double startTime;

        public double endTime;

        public string text;

    }

}
