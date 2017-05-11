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
        private int[,] tileArray = new int[16, 16] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            Entity tilemap = contexts.Main.Pool.GetObject();

            int[,] parsedMap = new int[tileArray.GetLength(0), tileArray.GetLength(1)];

            for(int x = 0; x < tileArray.GetLength(0); x++)
            {
                for (int y = 0; y < tileArray.GetLength(1); y++)
                {
                    int north = y - 1 < 0 ? 0 : tileArray[x, y - 1];
                    int west = x + 1 >= tileArray.GetLength(0) ? 0 : tileArray[x + 1, y];
                    int south = y + 1 >= tileArray.GetLength(1) ? 0 : tileArray[x, y + 1];
                    int east = x - 1 < 0 ? 0 : tileArray[x - 1, y];

                    parsedMap[x, y] += north * 1;
                    parsedMap[x, y] += west * 2;
                    parsedMap[x, y] += south * 4;
                    parsedMap[x, y] += east * 8;
                }
            }

            tilemap.AddComponent<TransformComponent>();
            Tilemap comp = new Tilemap();
            comp.tiles = parsedMap;
            comp.assetId = "DebugMap";
            comp.tileSize = 2.61f;
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
