using BulletSharp;
using RocketWorks.Entities;
using System.Collections;
using System.Collections.Generic;

namespace PoopyButts.Components
{
    public partial class CircleCollider : IComponent
    {
        public float radius;

        public CircleCollider()
        {

        }

        public CircleCollider(float radius = .05f)
        {
            this.radius = radius;
        }

        private RigidBody rigidBody;
        public RigidBody RigidBody 
        {
            get { return rigidBody; }
            set { rigidBody = value; }
        }
    }
}
