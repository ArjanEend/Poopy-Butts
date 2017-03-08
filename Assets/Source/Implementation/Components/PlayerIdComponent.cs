using RocketWorks.Entities;
using System;

public partial class PlayerIdComponent : IComponent {
    public int id;
    public bool isServer;
    public bool IsLocal
    {
        get { return id == 0; }
    }
}
