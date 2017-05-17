using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RocketWorks.Grouping;
using RocketWorks;

namespace Implementation.Systems
{
    public class TilemapCollision : SystemBase
    {
        private Group objectGroup;
        private Group tileGroup;

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);

            tileGroup = contexts.Main.Pool.GetGroup(typeof(TransformComponent), typeof(Tilemap));
            objectGroup = contexts.Main.Pool.GetGroup(typeof(TransformComponent), typeof(CircleCollider), typeof(MovementComponent));
        }

        public override void Destroy()
        {
        }

        private Vector2 HandleCollision(TransformComponent transform, MovementComponent movement, float deltaTime, CircleCollider collider, Tilemap map)
        {
            //Get all corners
            Vector2 position = transform.position;
            float ySolve = 0f;
            float xSolve = 0f;

            //position.y += movement.velocity.y * deltaTime;

            Vector2 max = position + collider.radius * .5f;
            Vector2 min = position - collider.radius * .5f;

            Vector2 topRight = new Vector2(max.x, max.y);
            Vector2 topLeft = new Vector2(min.x, max.y);
            Vector2 bottomRight = new Vector2(max.x, min.y);
            Vector2 bottomLeft = new Vector2(min.x, min.y);

            Vector2 diff = HandlePoint(topRight, map, position, collider.radius);
            if (diff.x > diff.y)
                position.y -= diff.y;
            else
                position.x -= diff.x;
            diff = HandlePoint(topLeft, map, position, collider.radius);
            if (diff.x > diff.y)
                position.y -= diff.y;
            else
                position.x -= diff.x;
            diff = HandlePoint(bottomRight, map, position, collider.radius);
            if (diff.x > diff.y)
                position.y -= diff.y;
            else
                position.x -= diff.x;
            diff = HandlePoint(bottomLeft, map, position, collider.radius);
            if (diff.x > diff.y)
                position.y -= diff.y;
            else
                position.x -= diff.x;

            if (Vector2.Distance(position, transform.position) > map.tileSize * 2f)
            {
                RocketLog.Log("Weird translation happening");
            }

            return position;
        }

        private Vector2 HandlePoint(Vector2 point, Tilemap map, Vector2 center, float radius)
        {
            Vector2 returnPos = Vector2.zero;
            Vector2 tileIndex = point / map.tileSize;
            tileIndex.x = Mathf.FloorToInt(tileIndex.x);
            tileIndex.y = Mathf.FloorToInt(tileIndex.y);
            Vector2 tilePos = (tileIndex * map.tileSize) + (map.tileSize * .5f);
            if (InBounds(tileIndex, map) && map.tiles[Mathf.FloorToInt(tileIndex.y), Mathf.FloorToInt(tileIndex.x)] == 1)
            {
                returnPos.x = ((map.tileSize * .5f) + (radius * .5f)) - (tilePos.x - center.x);
                returnPos.y = ((map.tileSize * .5f) + (radius * .5f)) - (tilePos.y - center.y);
            }
            return returnPos;
        }

        private bool InBounds(Vector2 pos, Tilemap map)
        {
            return InBounds(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), map);
        }

        private bool InBounds(int x, int y, Tilemap map)
        {
            return x > -1 && y > -1 && x < map.tiles.GetLength(1) && y < map.tiles.GetLength(0);
        }

        public override void Execute(float deltaTime)
        {
            for(int i = 0; i < tileGroup.Count; i++)
            {
                TransformComponent tileTrans = tileGroup[i].GetComponent<TransformComponent>();
                Tilemap tilemap = tileGroup[i].GetComponent<Tilemap>();
                for(int j = 0; j < objectGroup.Count; j++)
                {
                    TransformComponent objectTrans = objectGroup[i].GetComponent<TransformComponent>();
                    MovementComponent movement = objectGroup[i].GetComponent<MovementComponent>();
                    CircleCollider objectCollider = objectGroup[i].GetComponent<CircleCollider>();

                    objectTrans.position -= tileTrans.position;
                    
                    objectTrans.position = tileTrans.position + HandleCollision(objectTrans, movement, deltaTime, objectCollider, tilemap);
                }
            }
        }
    }
}
