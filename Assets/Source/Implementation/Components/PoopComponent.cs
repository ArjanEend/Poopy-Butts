using RocketWorks.Entities;

namespace Implementation.Components
{
    public partial class PoopComponent : IComponent
    {
        public float size = .1f;
        public EntityReference playerRef;
    }
}
