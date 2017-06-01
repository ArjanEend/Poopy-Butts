using RocketWorks.Entities;

public interface IComponentVisualizer
{
    void Init(params IComponent[] components);
    void Init(IComponent component);
}

public interface IUpdateComponent
{
    void OnUpdate(IComponent component);
}

public interface IComponentVisualizer<T> : IComponent where T : IComponent
{
    void Init(T component);
}

public interface IUpdateComponent<T> : IUpdateComponent where T : IComponent
{
    void OnUpdate(T component);
}