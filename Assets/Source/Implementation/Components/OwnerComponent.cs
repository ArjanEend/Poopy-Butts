using RocketWorks.Entities;

namespace Implementation.Components
{
    public partial class OwnerComponent : IComponent
    {
        public EntityReference playerReference;

        private float acceleration = 1100f;
        public float Acceleration
        { get { return acceleration; }
        set { acceleration = value; }
        }
    }
}
