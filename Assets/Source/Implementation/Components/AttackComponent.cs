using RocketWorks.Entities;

namespace Implementation.Components
{
    public partial class AttackComponent : IComponent
    {
        public float damage = 1f;
        public EntityReference target;
    }
}
