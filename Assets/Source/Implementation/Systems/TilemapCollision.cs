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

            position.y += movement.velocity.y * deltaTime;

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
                //RocketLog.Log("Player inside of occoupied tile, this shouldn't happen!");
            }
            
            //Seperate axis theorem
            if(movement.velocity.y > 0f)
            {
                Vector2 right = topRight;
                Vector2 left = topLeft;

                if (HandlePoint(right, map))
                {
                    ySolve = SolveYAxis(right, map.tileSize, collider.radius);
                    movement.velocity.y = 0f;
                } else 
                if (HandlePoint(left, map))
                {
                    ySolve = SolveYAxis(left, map.tileSize, collider.radius);
                    movement.velocity.y = 0f;
                }
            }
            else if(movement.velocity.y < 0f)
            {
                Vector2 right = bottomRight;
                Vector2 left = bottomLeft;

                if (HandlePoint(right, map))
                {
                    ySolve = SolveYAxis(right, map.tileSize, collider.radius);
                    movement.velocity.y = 0f;
                }
                else
                if (HandlePoint(left, map))
                {
                    ySolve = SolveYAxis(left, map.tileSize, collider.radius);
                    movement.velocity.y = 0f;
                }
            }

            position.y -= movement.velocity.y * deltaTime;
            position.x += movement.velocity.x * deltaTime;

            max = position + collider.radius * .5f;
            min = position - collider.radius * .5f;

            topRight = new Vector2(max.x, max.y);
            topLeft = new Vector2(min.x, max.y);
            bottomRight = new Vector2(max.x, min.y);
            bottomLeft = new Vector2(min.x, min.y);

            if (movement.velocity.x > 0f)
            {
                Vector2 top = topRight;
                Vector2 bottom = bottomRight;

                if (HandlePoint(top, map) || HandlePoint(bottom, map))
                {
                    xSolve = (map.tileSize + collider.radius * .5f) - (currentTile.x - min.x);
                    movement.velocity.x = 0f;
                }
            }
            else if (movement.velocity.x < 0f)
            {
                Vector2 top = topLeft;
                Vector2 bottom = bottomLeft;

                if (HandlePoint(top, map) || HandlePoint(bottom, map))
                {
                    xSolve = (map.tileSize + collider.radius * .5f) - (currentTile.x - min.x);
                    movement.velocity.x = 0f;
                }
            }

            //float xDiff = Math.Min(.2f, Mathf.Abs(position.x - xSolve));
            //float yDiff = Math.Min(.2f, Mathf.Abs(position.y - ySolve));
            if(Math.Abs(xSolve) > Math.Abs(ySolve))
            {
                position.y -= ySolve;
            } else
            {
                position.x -= xSolve;
            }

            if(Vector2.Distance(position, transform.position) > map.tileSize * 2f)
            {
                RocketLog.Log("Weird translation happening");
            }

            return position;
        }

        private float SolveXAxis(Vector2 point, float tileSize, float mySize)
        {
            float returnValue = 0f;
            Vector2 tile = point / tileSize;
            tile.x = Mathf.RoundToInt(tile.x);
            tile.y = Mathf.RoundToInt(tile.y);
            
            returnValue = (-tileSize + mySize) - (tile.x - (point.x - mySize * .5f));

            return returnValue;
        }

        private float SolveYAxis(Vector2 point, float tileSize, float mySize)
        {
            float returnValue = 0f;
            Vector2 tile = point / tileSize;
            tile.x = Mathf.RoundToInt(tile.x);
            tile.y = Mathf.RoundToInt(tile.y);

            returnValue = (tileSize + mySize) - (tile.y - (point.y - mySize * .5f));

            return returnValue;
        }

        private bool HandlePoint(Vector2 point, Tilemap map)
        {
            Vector2 tileIndex = point / map.tileSize;
            if (InBounds(tileIndex, map) && map.tiles[Mathf.RoundToInt(tileIndex.y), Mathf.RoundToInt(tileIndex.x)] == 1)
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
                    MovementComponent movement = objectGroup[i].GetComponent<MovementComponent>();
                    CircleCollider objectCollider = objectGroup[i].GetComponent<CircleCollider>();

                    objectTrans.position -= tileTrans.position;
                    
                    objectTrans.position = tileTrans.position + HandleCollision(objectTrans, movement, deltaTime, objectCollider, tilemap);
                }
            }
        }
    }
}
