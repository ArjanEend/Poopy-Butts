using UnityEngine;
using System.Collections;
using RocketWorks.Systems;
using RocketWorks.Pooling;
using System;
using RocketWorks.Entities;
using RocketWorks.Grouping;

using Vector2 = RocketWorks.Vector2;
using Implementation.Components;
using RocketWorks.Networking;

public class MoveInputSystem : SystemBase
{
    private int playerId;

    private EntityPool pool;
    private Group axisGroup;
    private Group buttonGroup;
    private Group playerGroup;

    private Vector2 prevInput;

    private Entity button1Entity;
    private Entity button2Entity;

    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    public MoveInputSystem(int playerId) : base()
    {
        //tickRate = .1f;
        this.playerId = playerId;
    }

    public override void Initialize(Contexts contexts)
    {
        this.pool = contexts.Input.Pool;
        this.axisGroup = contexts.Input.Pool.GetGroup(typeof(AxisComponent));
        buttonGroup = contexts.Input.Pool.GetGroup(typeof(ButtonComponent));
        playerGroup = contexts.Main.Pool.GetGroup(typeof(PlayerIdComponent), typeof(MovementComponent), typeof(TransformComponent));

    }

    public override void Execute(float deltaTime)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float dist = 0f;
            plane.Raycast(ray, out dist);
            Vector3 pos = ray.GetPoint(dist);

            button1Entity = pool.GetObject();
            button1Entity.AddComponent<TransformComponent>().position = pos;
            button1Entity.AddComponent<PlayerIdComponent>().id = playerId;
            ButtonComponent comp = new ButtonComponent();
            comp.id = 1;
            button1Entity.AddComponent(comp);
        }
        else if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float dist = 0f;
            plane.Raycast(ray, out dist);
            Vector3 pos = ray.GetPoint(dist);
            button1Entity.AddComponent<TransformComponent>().position = pos;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            button1Entity.Reset();
        }
        if(Input.GetButtonDown("Fire2"))
        {
            button2Entity = pool.GetObject();
            button2Entity.AddComponent<PlayerIdComponent>().id = playerId; ButtonComponent comp = new ButtonComponent();
            comp.id = 2;
            button2Entity.AddComponent(comp);
        } else if (Input.GetButtonUp("Fire2"))
        {
            button2Entity.Reset();
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).Normalized();

        if (input == prevInput || Vector2.Distance(prevInput, input) < .3f)
            return;
        prevInput = input;

        Entity entity = pool.GetObject();
        PlayerIdComponent component = entity.AddComponent(new PlayerIdComponent());
        component.id = playerId;
        AxisComponent aComponent = new AxisComponent();
        aComponent.input = input;
        entity.AddComponent(aComponent);

        for (int i = 0; i < playerGroup.Count; i++)
        {
            if (playerGroup[i].GetComponent<PlayerIdComponent>().id == playerId)
            {
                //At half server speed
                playerGroup[i].GetComponent<MovementComponent>().velocity = new Vector3(input.x, 0f, input.y).normalized * .8f;
                playerGroup[i].IsLocal = true;
            }
        }
    }

    public override void Cleanup()
    {
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }
}
