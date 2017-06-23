using BulletSharp;
using Implementation.Components;
using RocketWorks.Entities;
using System;
using System.Collections.Generic;

#if !UNITY_5
namespace Implementation.Physics
{
    public class PlayerIdFilterCallback : OverlapFilterCallback
    {
        public override bool NeedBroadphaseCollision(BroadphaseProxy proxy0, BroadphaseProxy proxy1)
        {
            if (proxy0 == null || proxy1 == null)
                return true;
            CollisionObject ob0 = proxy0.ClientObject as CollisionObject;
            CollisionObject ob1 = proxy1.ClientObject as CollisionObject;
            if(ob0 != null && ob1 != null)
            {
                if (!(ob0.UserObject is Entity && ob1.UserObject is Entity))
                    return true;

                Entity entA = ob0.UserObject as Entity;
                Entity entB = ob1.UserObject as Entity;
                if (entA.HasComponent<OwnerComponent>() && entA.GetComponent<OwnerComponent>().playerReference != null)
                    entA = entA.GetComponent<OwnerComponent>().playerReference;
                if (entB.HasComponent<OwnerComponent>() && entB.GetComponent<OwnerComponent>().playerReference != null)
                    entB = entB.GetComponent<OwnerComponent>().playerReference;
                if (entA.HasComponent<PlayerIdComponent>() && entB.HasComponent<PlayerIdComponent>())
                {
                    if (entA.GetComponent<PlayerIdComponent>().id == entB.GetComponent<PlayerIdComponent>().id)
                        return false;
                }
            }
            return true;
        }
    }
}
#endif