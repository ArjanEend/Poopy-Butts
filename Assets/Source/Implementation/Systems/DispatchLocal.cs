using System;
using RocketWorks.Entities;
using RocketWorks.Systems;

namespace Implementation.Systems
{
    public class DispatchLocal<T, S> : SystemBase where T : IComponent where S : EntityContext
    {
        public Action<T> ComponentUpdated = delegate { };
        public Action<T> EntityRemoved = delegate { };

        private int localUser;
        public DispatchLocal(int userId)
        {
            localUser = userId;
        }

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            contexts.GetContext<S>().Pool.GetGroup(typeof(PlayerIdComponent), typeof(T)).OnEntityAdded += OnComponentUpdate;
            contexts.GetContext<S>().Pool.GetGroup(typeof(PlayerIdComponent), typeof(T)).OnEntityRemoved += OnEntityRemoved;
        }

        private void OnComponentUpdate(Entity obj)
        {
            if(obj.GetComponent<PlayerIdComponent>().id == localUser)
                ComponentUpdated(obj.GetComponent<T>());
        }

        private void OnEntityRemoved(Entity obj)
        {
            if (obj.GetComponent<PlayerIdComponent>().id == localUser)
                EntityRemoved(obj.GetComponent<T>());
        }

        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
        }
    }
}
