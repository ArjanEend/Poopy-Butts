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
        tilemapGroup = contexts.Main.Pool.GetGroup(typeof(TileMapVisualizer));
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
            comp.go.name += " Entity: " + newEntities[i].CreationIndex;
            if (newEntities[i].GetComponent<TransformComponent>() != null)
            {
                Vector2 pos = newEntities[i].GetComponent<TransformComponent>().position;
                comp.go.transform.position = new Vector3(pos.x, 0f, pos.y);
            }

            for(int x = 0; x < comp.tiles.GetLength(0); x++)
            {
                for(int y = 0; y < comp.tiles.GetLength(1); y++)
                {
                    Transform targetChild = comp.go.transform.GetChild(comp.tiles[x,y]);
                    Transform copy = Instantiate(targetChild, comp.go.transform);
                    copy.transform.localPosition = new Vector3(x, 0f, y);
                }
            }

        }
    }
}
