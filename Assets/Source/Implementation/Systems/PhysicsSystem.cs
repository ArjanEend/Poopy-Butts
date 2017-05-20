using RocketWorks.Systems;
using BulletSharp;
using System;
using PoopyButts.Components;
using Implementation.Components;
using RocketWorks.Grouping;
using System.Collections.Generic;
using RocketWorks.Entities;
#if UNITY_5||UNITY_RUNTIME
using BulletSharp.Math;
#endif

namespace Implementation.Systems
{
    public class PhysicsSystem : SystemBase
    {
        private CollisionConfiguration configuration;
        private CollisionDispatcher dispatcher;
        private DbvtBroadphase broadPhase;
        private DynamicsWorld world;
        private Group circleGroup;

        private int count = 1;

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);

            configuration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(configuration);

            broadPhase = new DbvtBroadphase();

            world = new DiscreteDynamicsWorld(dispatcher, broadPhase, null, configuration);
            world.Gravity = new Vector3(0f, -10f, 0f);

            CreateGround();

            circleGroup = contexts.Main.Pool.GetGroup(typeof(CircleCollider), typeof(TransformComponent), typeof(MovementComponent));
        }

        private void CreateGround()
        {
            var groundShape = new BoxShape(500, 1, 500);
            //groundShape.InitializePolyhedralFeatures();
            //var groundShape = new StaticPlaneShape(Vector3.UnitY, 1);
            
            CollisionObject ground = LocalCreateRigidBody(0, Matrix.Identity, groundShape);
            ground.UserObject = "Ground";
        }

        public RigidBody LocalCreateRigidBody(float mass, Matrix startTransform, CollisionShape shape)
        {
            //rigidbody is dynamic if and only if mass is non zero, otherwise static
            bool isDynamic = (mass != 0.0f);

            Vector3 localInertia = Vector3.Zero;
            if (isDynamic)
                shape.CalculateLocalInertia(mass, out localInertia);

            //using motionstate is recommended, it provides interpolation capabilities, and only synchronizes 'active' objects
            DefaultMotionState myMotionState = new DefaultMotionState(startTransform);

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, myMotionState, shape, localInertia);
            rbInfo.Friction = 2f;
            rbInfo.LinearDamping = 1.9f;
            rbInfo.AngularDamping = 1.9f;
            RigidBody body = new RigidBody(rbInfo);
            rbInfo.Dispose();
            world.AddRigidBody(body);

            return body;
        }

        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
            world.StepSimulation(deltaTime);

            List<Entity> newColliders = circleGroup.NewEntities;
            for(int i = 0; i < newColliders.Count; i++)
            {
                var trans = newColliders[i].GetComponent<TransformComponent>();
                var col = newColliders[i].GetComponent<CircleCollider>();
                var shape = new SphereShape(col.radius);
                var mat = Matrix.Translation(new Vector3(trans.position.x + (.5f * count++), 5f, trans.position.y));

                col.RigidBody = LocalCreateRigidBody(15f, mat, shape);
                col.RigidBody.RollingFriction = 5f;
                col.RigidBody.Friction = 5f;
                col.RigidBody.ApplyForce(new Vector3(-15f * count, 0f, 0f), col.RigidBody.CenterOfMassPosition);
            }

            for(int i = 0; i < circleGroup.Count; i++)
            {
                TransformComponent transform = circleGroup[i].GetComponent<TransformComponent>();
                MovementComponent movement = circleGroup[i].GetComponent<MovementComponent>();
                var col = circleGroup[i].GetComponent<CircleCollider>();
                var pos = col.RigidBody.WorldTransform.Origin;
                transform.position = new RocketWorks.Vector2(pos.X, pos.Z);
                col.RigidBody.ApplyCentralForce(new Vector3(movement.acceleration.x, 0f, movement.acceleration.y));
                circleGroup[i].GetComponent<MovementComponent>().velocity = new RocketWorks.Vector2(col.RigidBody.LinearVelocity.X, col.RigidBody.LinearVelocity.Z);
            }
        }
    }
}
