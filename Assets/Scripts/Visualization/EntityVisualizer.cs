﻿using System;
using RocketWorks.Entities;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class EntityVisualizer : MonoBehaviour, IEntityVisualizer
{
    private bool destroy = false;

    private IComponentVisualizer[] visualizers;
    private IUpdateComponent[] updates;

    private Queue<IComponent> componentQueue;
    private Queue<IComponent> removeQueue;
    private Queue<Entity> initQueue;

    private void Start()
    {
        for (int i = 0; i < initQueue.Count; i++)
        {
            Entity initEntity = initQueue.Dequeue();
            for (int j = 0; j < visualizers.Length; j++)
            {
                visualizers[j].Init(initEntity.Components);
            }
        }
    }

    public void DeInit(Entity entity)
    {
        destroy = true;
    }

    public void Init(Entity entity)
    {
        entity.DestroyEvent += DeInit;

        string ent = JsonConvert.SerializeObject(entity);
        RocketLog.Log(ent);
        RocketLog.Log("" + JsonConvert.DeserializeObject<Entity>(ent).CreationIndex);

        componentQueue = new Queue<IComponent>();
        removeQueue = new Queue<IComponent>();
        initQueue = new Queue<Entity>();

        visualizers = GetComponentsInChildren<IComponentVisualizer>(true);
        updates = GetComponentsInChildren<IUpdateComponent>(true);

        if(visualizers.Length > 0)
            initQueue.Enqueue(entity);

        if (updates.Length > 0)
        {
            entity.CompositionChangeEvent += CompositionChanged;
            entity.CompositionSubtractEvent += ComponentRemoved;
        }
    }

    public void Init(IComponent component)
    {
        componentQueue.Enqueue(component);
    }

    public void CompositionChanged(IComponent comp, Entity entity = null)
    {
        componentQueue.Enqueue(comp);
    }

    public void ComponentRemoved(IComponent comp, Entity entity = null)
    {
        removeQueue.Enqueue(comp);
    }

    private IEnumerator DestroyMe()
    {
        Destroy(gameObject);
        yield return null;
    }

    private void Update()
    {
        if (destroy)
        {
            Destroy(gameObject);
            return;
        }
        if (updates.Length == 0)
            return;
        
        for (int i = 0; i < componentQueue.Count; i++)
        {
            IComponent comp = componentQueue.Dequeue();
            for (int j = 0; j < updates.Length; j++)
            {
                updates[j].OnUpdate(comp);
            }
        }
        for (int i = 0; i < removeQueue.Count; i++)
        {
            IComponent comp = removeQueue.Dequeue();
            for (int j = 0; j < updates.Length; j++)
            {
                updates[j].OnRemove(comp);
            }
        }
    }
}
