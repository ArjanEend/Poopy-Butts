using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public partial class PingComponent : IComponent
    {
        public long ticks;
        public DateTime LocalTime
        {
            get { return new DateTime().AddMilliseconds(ticks); }
        }
    }
}
