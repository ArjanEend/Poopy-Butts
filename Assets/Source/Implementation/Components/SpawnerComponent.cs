using RocketWorks.Entities;
using System.Collections.Generic;

namespace Implementation.Components
{
    public partial class SpawnerComponent : IComponent
    {
        public float interval = 5f;
        public float lastTime = 0f;

        public float influence = 15f;

        private Dictionary<Entity, float> influences;
        public Dictionary<Entity, float> Influences
        {
            get {
                if (influences == null)
                    influences = new Dictionary<Entity, float>();
                return influences;
            }
        }
    }
}
