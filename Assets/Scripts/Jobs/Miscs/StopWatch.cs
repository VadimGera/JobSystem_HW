#nullable enable
using System;


namespace Jobs.Miscs
{
    public class StopWatch
    {
        public TimeSpan Launch(Action action)
        {
            var start = DateTime.Now;
            action();
            return DateTime.Now - start;
        }
    }
}