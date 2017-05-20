using RocketWorks;
using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public partial class TransformComponent : IComponent, IEstimatable<TransformComponent>
    {
        public Vector3 position;

        public void Estimate(TransformComponent against, float deltaTime, bool local)
        {
            Vector3 delta = against.position - position;
            if (delta.Magnitude() > .1f || !local)
            {
                position = Vector3.Lerp(position, against.position, deltaTime * 10f * delta.Magnitude());
                //position += delta * deltaTime * .5f;
            }
        }

        public void Estimate(object against, float deltaTime, bool local)
        {
            Estimate((TransformComponent)against, deltaTime, local);
        }
    }
}
