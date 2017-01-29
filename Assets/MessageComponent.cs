using RocketWorks.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MessageComponent : IComponent {

    public string message;
    public int userId;
    public DateTime timeStamp;

}
