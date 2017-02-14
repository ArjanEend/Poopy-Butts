using UnityEngine;
using System.Collections;
using RocketWorks.Systems;
using RocketWorks.Pooling;
using System;
using RocketWorks.Entities;
using RocketWorks.Grouping;

public class MoveInputSystem : SystemBase
{
    private int playerId;

    private EntityPool pool;
    private Group group;

    public MoveInputSystem(int playerId) : base()
    {
        this.playerId = playerId;
    }

    public override void Initialize(EntityPool pool)
    {
        this.pool = pool;
        this.group = pool.GetGroup(typeof(AxisComponent));

    }

    public override void Execute()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Entity entity = pool.GetObject();
        PlayerIdComponent component = entity.AddComponent(new PlayerIdComponent ());
        component.id = (uint)playerId;
        AxisComponent aComponent = entity.AddComponent(new AxisComponent());
        aComponent.input = input;
    }

    public override void Cleanup()
    {
        for(int i = group.Count - 1; i >= 0; i--)
        {
            group[i].Reset();
        }
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }
}
