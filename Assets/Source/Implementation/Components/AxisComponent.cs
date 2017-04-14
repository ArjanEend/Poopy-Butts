﻿using System.Collections;
using RocketWorks.Entities;
using RocketWorks;
using System;
using RocketWorks.Networking;

public partial class AxisComponent : IComponent {

    public Vector2 input;
    public DateTime time;

    public AxisComponent(){
        this.time = ServerTimeStamp.ServerNow;
    }
	
}
