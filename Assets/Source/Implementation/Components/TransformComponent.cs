using RocketWorks;
using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public partial class TransformComponent : IComponent, IEstimatable<TransformComponent>
    {
        public Vector2 position;

        public void Estimate(TransformComponent against, float deltaTime)
        {
            Vector2 delta = against.position - position;
            if (delta.Magnitude() > 1f)
                position = against.position;
            //position += delta * deltaTime;
        }

        public void Estimate(object against, float deltaTime)
        {
            Estimate((TransformComponent)against, deltaTime);
        }
    }
}
