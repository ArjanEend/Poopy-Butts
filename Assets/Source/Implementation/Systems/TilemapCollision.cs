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
            objectGroup = contexts.Main.Pool.GetGroup(typeof(TransformComponent), typeof(CircleCollider));
        }

        public override void Destroy()
        {
        }

        private void HandleCollision(TransformComponent transform, CircleCollider collider, Tilemap map)
        {
            //Get all corners

            Vector2 max = transform.position + collider.radius;
            Vector2 min = transform.position - collider.radius;

            Vector2 topRight = new Vector2(max.x, max.y);
            Vector2 topLeft = new Vector2(min.x, max.y);
            Vector2 bottomRight = new Vector2(max.x, min.y);
            Vector2 bottomLeft = new Vector2(min.x, min.y);
            
            Vector2 position = transform.position;

            Vector2 currentTile = transform.position / map.tileSize;

            //Seperate axis theorem
            {
                Vector2 right = topRight;
                Vector2 left = topLeft;

                if (HandlePoint(right, map) || HandlePoint(left, map))
                {
                    position.y = (currentTile.y + map.tileSize) - (max.y);
                    //bodyPosition.y -= speedY;
                }
            }
            {
                Vector2 right = bottomRight;
                Vector2 left = bottomLeft;

                if (HandlePoint(right, map) || HandlePoint(left, map))
                {
                    position.y = (currentTile.y - map.tileSize) + (min.y);
                    //bodyPosition.y -= speedY;
                }
            }
            /*{
                Vector2 right = topRight;
                Vector2 left = bottomRight;

                if (HandlePoint(right) || HandlePoint(left))
                {
                    bodyPosition.x = (currentTile.x + tileSize) - (bounds.extents.x + center.x);
                    //bodyPosition.x -= speedX;
                }
            }
            {
                Vector2 right = topLeft;
                Vector2 left = bottomLeft;

                if (HandlePoint(left) || HandlePoint(right))
                {
                    bodyPosition.x = (currentTile.x) + (bounds.extents.x - center.x);
                    //bodyPosition.x -= speedX;
                }
            }*/


            transform.position = position;
        }

        private bool HandlePoint(Vector2 point, Tilemap map)
        {
            Vector2 tileIndex = point / map.tileSize;
            if (InBounds(tileIndex, map) && map.tiles[(int)tileIndex.x, (int)tileIndex.y] != 0)
            {
                return true;
            }
            return false;
        }

        private bool InBounds(Vector2 pos, Tilemap map)
        {
            return InBounds((int)pos.x, (int)pos.y, map);
        }

        private bool InBounds(int x, int y, Tilemap map)
        {
            return x > -1 && y > -1 && x < map.tiles.GetLength(0) && y < map.tiles.GetLength(1);
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
                    CircleCollider objectCollider = objectGroup[i].GetComponent<CircleCollider>();

                    //Vector2 relativePos = objectTrans.position - tileTrans.position;
                    //Vector2 tilePos = relativePos / tilemap.tileSize;

                    HandleCollision(objectTrans, objectCollider, tilemap);
                }
            }
        }
    }
}
