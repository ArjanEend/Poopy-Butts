using System;
using RocketWorks;
using RocketWorks.Entities;

namespace Implementation.Components
{
    public partial class LerpToComponent : IComponent, IEstimatable<LerpToComponent>
    {
        public Vector3 position = Vector3.zero;
        public Vector2 velocity = Vector2.zero;

        public void Estimate(LerpToComponent against, float deltaTime, bool local)
        {
            Vector3 delta = against.position - position;
            if (delta.Magnitude() > .1f || !local)
            {
                position = against.position + delta * .5f;
            }

            velocity = against.velocity;
        }

        public void Estimate(object against, float deltaTime, bool local)
        {
            Estimate((LerpToComponent)against, deltaTime, local);
        }
    }

}
