using RocketWorks.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks.Entities;

public class ButtonVisualization : UnitySystemBase
{
    private VisualizationComponent visualizer;
    private Group buttonGroup;
    private Group playerGroup;

    private ParticleSystem button1Object;
    private ParticleSystem button2Object;

    private bool button1Active;
    private bool button2Active;

    private float triggerRadius = 1f;

    public override void Initialize(Contexts contexts)
    {
        base.Initialize(contexts);

        buttonGroup = contexts.Input.Pool.GetGroup(typeof(ButtonComponent), typeof(PlayerIdComponent));
        playerGroup = contexts.Main.Pool.GetGroup(typeof(PlayerIdComponent), typeof(VisualizationComponent));

        buttonGroup.OnEntityAdded += OnButton;
        buttonGroup.OnEntityRemoved += OnButtonDisable;

        button1Object = GameObject.Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Button1"));
        button2Object = GameObject.Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Button2"));
    }

    private void OnButton(Entity obj)
    {
        if (obj.GetComponent<ButtonComponent>().id == 1)
        {
            button1Active = true;
            button1Object.transform.position = obj.GetComponent<TransformComponent>().position;
        }
        else
        {
            button2Active = true;
        }
    }

    private void OnButtonDisable(Entity obj)
    {
        if (obj.GetComponent<ButtonComponent>().id == 1)
        {
            button1Active = false;
        }
        else
        {
            button2Active = false;
        }
    }

    public override void Destroy()
    {

    }

    public override void Execute(float deltaTime)
    {
        if (visualizer == null || visualizer.go == null)
            return;
        if (button1Active)
        {
            //button1Object.SetActive(true);
            button1Object.Play();
            button1Active = false;
        }
        else
        {
            //button1Object.SetActive(false);
        }
        if (button2Active)
        {
            if (!button2Object.isPlaying)
                button2Object.Play();
            button2Object.transform.localScale = Vector3.one * triggerRadius;
            button2Object.transform.position = visualizer.go.transform.position;
        } else
        {
            button2Object.Stop(true);
            //button2Object.SetActive(false);
        }
    }

    public void SetPlayer(VisualizationComponent obj)
    {
        visualizer = obj;
    }

    public void SetTrigger(TriggerComponent obj)
    {
        triggerRadius = obj.radius;
    }
}
