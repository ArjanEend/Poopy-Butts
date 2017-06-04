using RocketWorks.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks.Entities;

public class ButtonVisualization : SystemBase
{
    private VisualizationComponent visualizer;
    private Group buttonGroup;
    private Group playerGroup;

    private GameObject button1Object;
    private GameObject button2Object;

    private bool button1Active;
    private bool button2Active;

    public override void Initialize(Contexts contexts)
    {
        base.Initialize(contexts);

        buttonGroup = contexts.Input.Pool.GetGroup(typeof(ButtonComponent), typeof(PlayerIdComponent));
        playerGroup = contexts.Main.Pool.GetGroup(typeof(PlayerIdComponent), typeof(VisualizationComponent));

        buttonGroup.OnEntityAdded += OnButton;
        buttonGroup.OnEntityRemoved += OnButtonDisable;

        button1Object = GameObject.Instantiate(Resources.Load<GameObject>("Button1"));
        button2Object = GameObject.Instantiate(Resources.Load<GameObject>("Button2"));
    }

    private void OnButton(Entity obj)
    {
        if (obj.GetComponent<ButtonComponent>().id == 1)
            button1Active = true;
        else
            button2Active = true;
        
    }

    private void OnButtonDisable(Entity obj)
    {
        //visualizer.go.GetComponent<EntityVisualizer>().CompositionChanged(obj.GetComponent<ButtonComponent>());
    }

    public override void Destroy()
    {

    }

    public override void Execute(float deltaTime)
    {
        
    }

    public void SetPlayer(VisualizationComponent obj)
    {
        visualizer = obj;
    }
}
