using RocketWorks.Entities;
using System;

[Serializable]
public class MessageComponent : IComponent {

    public string message;
    public int userId;
    public DateTime timeStamp;

}
