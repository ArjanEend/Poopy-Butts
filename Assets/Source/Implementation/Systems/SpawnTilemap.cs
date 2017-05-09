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
                    if (y - 1 < 0 || y + 1 > tileArray.GetLength(1) || x - 1 < 0 || x + 1 > tileArray.GetLength(0))
                        continue;

                    if (tileArray[x, y] == 1)
                    {
                        parsedMap[x + 1, y] += 1;
                        parsedMap[x, y + 1] += 2;
                        parsedMap[x - 1, y] += 3;
                        parsedMap[x, y - 1] += 4;
                    }
                }
            }
            

            tilemap.AddComponent<TransformComponent>();
            tilemap.AddComponent<Tilemap>().tiles = parsedMap;
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Execute(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
