using UnityEngine;
using System.Collections;
using RocketWorks.Systems;
using RocketWorks.Pooling;
using System;
using RocketWorks.Entities;
using RocketWorks.Grouping;
using System.Collections.Generic;

public class MoveUpdateSystem : SystemBase
{
    private int playerId;

    private EntityPool pool;
    private Group inputGroup;
    private Group playerGroup;

    private Dictionary<int, Vector2> playerBindings = new Dictionary<int, Vector2>();

    public MoveUpdateSystem() : base()
    {
    }

    public override void Initialize(EntityPool pool)
    {
        this.pool = pool;
        this.inputGroup = pool.GetGroup(typeof(AxisComponent), typeof(PlayerIdComponent));
        this.playerGroup = pool.GetGroup(typeof(PlayerIdComponent), typeof(TestComponent));
    }

    public override void Execute()
    {
        int idComp = pool.GetIndexOf(typeof(PlayerIdComponent));
        int axisComp = pool.GetIndexOf(typeof(AxisComponent));
        int testComp = pool.GetIndexOf(typeof(TestComponent));
        for(int i = 0; i < inputGroup.Count; i++)
        {
            int id = inputGroup[i].GetComponent<PlayerIdComponent>(idComp).id;
            if (!playerBindings.ContainsKey(id))
                playerBindings.Add(id, inputGroup[i].GetComponent<AxisComponent>(axisComp).input);
            else
                playerBindings[id] = inputGroup[i].GetComponent<AxisComponent>(axisComp).input;
        }

        for(int i = 0; i < playerGroup.Count; i++)
        {
            int id = playerGroup[i].GetComponent<PlayerIdComponent>(idComp).id;

            if(playerBindings.ContainsKey(id))
            {
                TestComponent test = playerGroup[i].GetComponent<TestComponent>(testComp);
                Vector3 pos = test.visuals.position;

                pos += (Vector3)playerBindings[id] * 20f * Time.deltaTime;

                test.visuals.position = pos;
            }
        }
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }
}
