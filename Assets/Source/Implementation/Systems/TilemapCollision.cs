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

        private Vector2 HandleCollision(TransformComponent transform, CircleCollider collider, Tilemap map)
        {
            //Get all corners
            Vector2 position = transform.position;

            Vector2 max = position + collider.radius * .5f;
            Vector2 min = position - collider.radius * .5f;

            Vector2 topRight = new Vector2(max.x, max.y);
            Vector2 topLeft = new Vector2(min.x, max.y);
            Vector2 bottomRight = new Vector2(max.x, min.y);
            Vector2 bottomLeft = new Vector2(min.x, min.y);
            
            Vector2 currentTile = transform.position / map.tileSize;
            currentTile.x = Mathf.RoundToInt(currentTile.x);
            currentTile.y = Mathf.RoundToInt(currentTile.y);
            Vector2 tilePos = currentTile * map.tileSize;
            
            if (HandlePoint(transform.position, map))
            {
                RocketLog.Log("Player inside of occoupied tile, this shouldn't happen!");
            }

            float ySolve = position.y;
            float xSolve = position.x;
            //Seperate axis theorem
            {
                Vector2 right = topRight;
                Vector2 left = topLeft;

                if (HandlePoint(right, map) || HandlePoint(left, map))
                {
                    ySolve = (tilePos.y) - ((map.tileSize * -.5f) + (collider.radius * .51f));
                }
            }
            {
                Vector2 right = bottomRight;
                Vector2 left = bottomLeft;

                if (HandlePoint(right, map) || HandlePoint(left, map))
                {
                    ySolve = (tilePos.y) + ((map.tileSize * -.5f) + (collider.radius * .51f));
                }
            }
            max = position + collider.radius * .5f;
            min = position - collider.radius * .5f;

            topRight = new Vector2(max.x, max.y);
            topLeft = new Vector2(min.x, max.y);
            bottomRight = new Vector2(max.x, min.y);
            bottomLeft = new Vector2(min.x, min.y);
            {
                Vector2 top = topRight;
                Vector2 bottom = bottomRight;

                if (HandlePoint(top, map) || HandlePoint(bottom, map))
                {
                    xSolve = (tilePos.x) - ((map.tileSize * -.5f) + (collider.radius * .51f));
                }
            }
            {
                Vector2 top = topLeft;
                Vector2 bottom = bottomLeft;

                if (HandlePoint(top, map) || HandlePoint(bottom, map))
                {
                    xSolve = (tilePos.x) + ((map.tileSize * -.5f) + (collider.radius * .51f));
                }
            }

            float xDiff = Mathf.Abs(position.x - xSolve);
            float yDiff = Mathf.Abs(position.y - ySolve);

            if (xSolve < ySolve)
                position.x = xSolve;
            else
                position.y = ySolve;

            if(Vector2.Distance(position, transform.position) > map.tileSize * 2f)
            {
                RocketLog.Log("Weird translation happening");
            }

            return position;
        }

        private bool HandlePoint(Vector2 point, Tilemap map)
        {
            Vector2 tileIndex = point / map.tileSize;
            if (InBounds(tileIndex, map) && map.tiles[Mathf.RoundToInt(tileIndex.y), Mathf.RoundToInt(tileIndex.x)] == 0)
            {
                return true;
            }
            return false;
        }

        private bool InBounds(Vector2 pos, Tilemap map)
        {
            return InBounds(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), map);
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
                    CircleCollider objectCollider = objectGroup[i].GetComponent<CircleCollider>();

                    objectTrans.position -= tileTrans.position;
                    
                    objectTrans.position = tileTrans.position + HandleCollision(objectTrans, objectCollider, tilemap);
                }
            }
        }
    }
}
