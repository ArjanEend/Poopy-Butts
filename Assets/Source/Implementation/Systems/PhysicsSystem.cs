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
        private Group triggerGroup;

        private int count = 1;

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);

            configuration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(configuration);

            broadPhase = new DbvtBroadphase();

            world = new DiscreteDynamicsWorld(dispatcher, broadPhase, null, configuration);
            world.PairCache.SetInternalGhostPairCallback(new GhostPairCallback());
            world.Gravity = new Vector3(0f, -10f, 0f);

            CreateGround();

            circleGroup = contexts.Main.Pool.GetGroup(typeof(CircleCollider), typeof(TransformComponent));
            triggerGroup = contexts.Main.Pool.GetGroup(typeof(TriggerComponent), typeof(TransformComponent));
        }

        private void CreateGround()
        {
            var groundShape = new BoxShape(500, .5f, 500);
            //groundShape.InitializePolyhedralFeatures();
            //var groundShape = new StaticPlaneShape(Vector3.UnitY, 1);
            var mat = Matrix.Translation(new Vector3(0f, -.5f, 0f));
            CollisionObject ground = LocalCreateRigidBody(0, mat, groundShape);
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
            rbInfo.LinearDamping = 1.9f;
            rbInfo.AngularDamping = 1.9f;
            RigidBody body = new RigidBody(rbInfo);
            body.ActivationState = ActivationState.DisableDeactivation;

            //body.LinearFactor = new Vector3(1f, 0f, 1f);
            rbInfo.Dispose();
            world.AddRigidBody(body);

            return body;
        }

        public PairCachingGhostObject CreateTrigger(float radius, Matrix transform)
        {
            var shape = new SphereShape(radius);
            PairCachingGhostObject obj = new PairCachingGhostObject();
            obj.CollisionShape = shape;
            obj.WorldTransform = transform;
            obj.CollisionFlags |= CollisionFlags.NoContactResponse;
            world.AddCollisionObject(obj);
            return obj;
        }

        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
            world.StepSimulation(deltaTime);
            world.SetInternalTickCallback(TickCallback);

            List<Entity> newColliders = circleGroup.NewEntities;
            for(int i = 0; i < newColliders.Count; i++)
            {
                var trans = newColliders[i].GetComponent<TransformComponent>();
                var col = newColliders[i].GetComponent<CircleCollider>();
                var shape = new SphereShape(col.radius);
                var mat = Matrix.Translation(new Vector3(trans.position.x, trans.position.y, trans.position.z));

                col.RigidBody = LocalCreateRigidBody(newColliders[i].HasComponent<MovementComponent>() ? 15f : 0f, mat, shape);
                col.RigidBody.UserObject = newColliders[i];
                col.RigidBody.ApplyForce(new Vector3(-5f * count, 0f, 0f), col.RigidBody.CenterOfMassPosition);
            }
            List<Entity> newTriggers = triggerGroup.NewEntities;
            for (int i = 0; i < newTriggers.Count; i++)
            {
                var trans = newTriggers[i].GetComponent<TransformComponent>();
                var col = newTriggers[i].GetComponent<TriggerComponent>();
                var mat = Matrix.Translation(new Vector3(trans.position.x, trans.position.y, trans.position.z));

                col.GhostObject = CreateTrigger(col.radius, mat);
                col.GhostObject.UserObject = newTriggers[i];
            }


            for (int i = 0; i < circleGroup.Count; i++)
            {
                TransformComponent transform = circleGroup[i].GetComponent<TransformComponent>();
                MovementComponent movement = circleGroup[i].GetComponent<MovementComponent>();
                if (movement == null)
                    continue;
                var col = circleGroup[i].GetComponent<CircleCollider>();
                var pos = col.RigidBody.CenterOfMassPosition;
                transform.position = new RocketWorks.Vector3(pos.X, pos.Y, pos.Z);
                col.RigidBody.ApplyCentralForce(new Vector3(movement.acceleration.x, movement.acceleration.y, movement.acceleration.z));
                circleGroup[i].GetComponent<MovementComponent>().velocity = new RocketWorks.Vector3(col.RigidBody.LinearVelocity.X, col.RigidBody.LinearVelocity.Y, col.RigidBody.LinearVelocity.Z);
            }
            for (int i = 0; i < triggerGroup.Count; i++)
            {
                TransformComponent transform = triggerGroup[i].GetComponent<TransformComponent>();
                var col = triggerGroup[i].GetComponent<TriggerComponent>();
                var pos = transform.position;
                var mat = Matrix.Translation(new Vector3(pos.x, pos.y, pos.z));
                col.GhostObject.WorldTransform = mat;
            }
        }

        private void TickCallback(DynamicsWorld world, float timeStep)
        {
            int numManifolds = world.Dispatcher.NumManifolds;
            for (int i = 0; i < numManifolds; i++)
            {
                var contactManifold = world.Dispatcher.GetManifoldByIndexInternal(i);
                var obA = contactManifold.Body0;
                var obB = contactManifold.Body1;

                if (obA is PairCachingGhostObject || obB is PairCachingGhostObject)
                    continue;

                if (!(obA.UserObject is Entity && obB.UserObject is Entity))
                    continue;

                int numContacts = contactManifold.NumContacts;
                for (int j = 0; j < numContacts; j++)
                {
                    var pt = contactManifold.GetContactPoint(j);
                    if (pt.Distance < .0f)
                    {
                        var entity = contexts.Physics.Pool.GetObject();
                        var component = new CollisionComponent();
                        component.a = obA.UserObject as Entity;
                        component.b = obB.UserObject as Entity;
                        entity.AddComponent(component);
                        break;
                    }
                }
            }
        }
    }
}
