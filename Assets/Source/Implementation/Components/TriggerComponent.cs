using BulletSharp;
using RocketWorks.Entities;

namespace Implementation.Components
{
    public partial class TriggerComponent : IComponent
    {
        public float radius;
        private GhostObject ghostObject;
        public GhostObject GhostObject
        {
            get { return ghostObject; }
            set { ghostObject = value; }
        }
    }
}
