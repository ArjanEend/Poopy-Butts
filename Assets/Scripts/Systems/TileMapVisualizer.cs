using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Entities;
using RocketWorks.Grouping;
using RocketWorks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TileMapVisualizer : UnitySystemBase
{
    private Group tilemapGroup;

    public override void Initialize(Contexts contexts)
    {
        tilemapGroup = contexts.Main.Pool.GetGroup(typeof(Tilemap));
    }

    public override void Destroy()
    {
    }

    public override void Execute(float deltaTime)
    {
        List<Entity> newEntities = tilemapGroup.NewEntities;
        for (int i = 0; i < newEntities.Count; i++)
        {
            Tilemap comp = newEntities[i].GetComponent<Tilemap>();
            comp.go = Instantiate<GameObject>(Resources.Load<GameObject>(comp.assetId));
            Debug.Log(comp.go);
            comp.go.name += " Entity: " + newEntities[i].CreationIndex;
            if (newEntities[i].GetComponent<TransformComponent>() != null)
            {
                Vector2 pos = newEntities[i].GetComponent<TransformComponent>().position;
                comp.go.transform.position = new Vector3(pos.x, 0f, pos.y);
            }
            int[,] tileArray = comp.tiles;
            int[,] parsedMap = new int[tileArray.GetLength(0), tileArray.GetLength(1)];

            for (int y = 0; y < tileArray.GetLength(0); y++)
            {
                for (int x = 0; x < tileArray.GetLength(1); x++)
                {
                    int north = y - 1 < 0 ? 0 : tileArray[y - 1, x];
                    int west = x + 1 >= tileArray.GetLength(1) ? 0 : tileArray[y, x + 1];
                    int south = y + 1 >= tileArray.GetLength(0) ? 0 : tileArray[y + 1, x];
                    int east = x - 1 < 0 ? 0 : tileArray[y, x - 1];

                    parsedMap[y, x] += south * 1;
                    parsedMap[y, x] += west * 2;
                    parsedMap[y, x] += north * 4;
                    parsedMap[y, x] += east * 8;
                }
            }

            for (int y = 0; y < parsedMap.GetLength(0); y++)
            {
                for(int x = 0; x < parsedMap.GetLength(1); x++)
                {
                    Transform targetChild = comp.go.transform.GetChild(parsedMap[y,x]);
                    Transform copy = Instantiate(targetChild, comp.go.transform);
                    copy.gameObject.SetActive(true);
                    copy.transform.localPosition = new Vector3(x * comp.tileSize, copy.transform.localPosition.y, y * comp.tileSize);
                }
            }

        }
    }
}
