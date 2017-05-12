using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Entities;
using RocketWorks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Systems
{
    public class SpawnTilemap : SystemBase
    {
        private int[,] tileArray = new int[4, 16] {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            Entity tilemap = contexts.Main.Pool.GetObject();
            

            tilemap.AddComponent<TransformComponent>();
            Tilemap comp = new Tilemap();
            comp.tiles = tileArray;
            comp.assetId = "DebugMap";
            comp.tileSize = 1.5f;
            tilemap.AddComponent(comp);
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Execute(float deltaTime)
        {
        }
    }
}
