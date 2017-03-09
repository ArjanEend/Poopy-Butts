using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public partial class PongComponent : IComponent
    {
        public long toTicks;

        public DateTime LocalTime
        {
            get { return new DateTime().AddMilliseconds(toTicks); }
        }
    }
}
