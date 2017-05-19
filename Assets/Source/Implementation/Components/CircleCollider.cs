using BulletSharp;
using RocketWorks.Entities;
using System.Collections;
using System.Collections.Generic;

namespace PoopyButts.Components
{
    public partial class CircleCollider : IComponent
    {
        public float radius;

        private RigidBody rigidBody;
        public RigidBody RigidBody 
        {
            get { return rigidBody; }
            set { rigidBody = value; }
        }
    }
}
