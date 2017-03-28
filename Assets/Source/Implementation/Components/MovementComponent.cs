using RocketWorks;
using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public partial class MovementComponent : IComponent, IEstimatable<MovementComponent>
    {
        public Vector2 velocity;
        public float friction;
        public Vector2 acceleration;

        public void Estimate(MovementComponent against, float deltaTime)
        {
            Vector2 delta = against.velocity - velocity;
            if (delta.Magnitude() > .5f)
            {
                velocity = against.velocity;
                velocity += delta * deltaTime;
            }
        }

        public void Estimate(object against, float deltaTime)
        {
            Estimate((MovementComponent)against, deltaTime);
        }
    }
}
