using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public enum ResourceID : ushort
    {
        Cube = 0,
        Unit = 1,
        Turd = 2,
        character = 3,
        Ship = 4,
        Bullet = 5,
    }
    public partial class VisualizationComponent : IComponent
    {
        public ResourceID resourceId = ResourceID.Cube;
    }
}
