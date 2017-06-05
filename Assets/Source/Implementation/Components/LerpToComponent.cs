using System;
using RocketWorks;
using RocketWorks.Entities;

namespace Implementation.Components
{
    public partial class LerpToComponent : IComponent, IEstimatable<LerpToComponent>
    {
        public Vector3 position = Vector3.zero;
        public Vector3 velocity = Vector3.zero;

        public void Estimate(LerpToComponent against, float deltaTime, bool local)
        {
            Vector3 delta = against.position - position;
            //against.position += delta * deltaTime;
            if (delta.Magnitude() > .01f || !local)
            {
                position = against.position + delta * .5f;
            }
            if(position.y > 50f)
            {
                RocketLog.Log("This entity is very high", this);
            }
            velocity = against.velocity;
        }

        public void Estimate(object against, float deltaTime, bool local)
        {
            Estimate((LerpToComponent)against, deltaTime, local);
        }
    }

}
