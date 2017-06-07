using System;
using RocketWorks;
using RocketWorks.Entities;

namespace Implementation.Components
{
    public partial class LerpToComponent : IComponent, IEstimatable<LerpToComponent>
    {
        public LerpToComponent()
        {
            position = Vector3.zero;
            velocity = Vector3.zero;
        }

        public Vector3 position = Vector3.zero;
        public Vector3 velocity = Vector3.zero;

        public void Estimate(LerpToComponent against, float deltaTime, bool local)
        {
            Vector3 delta = against.position - position;
            //against.position += delta * deltaTime;
            if (delta.Magnitude() > .5f && !local)
            {
                position = against.position;
            }
            else if (delta.Magnitude() > .1f || !local)
            {
                position = against.position; //+ delta * deltaTime;
            }
            if(Mathf.Abs(position.y) > 1f)
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
