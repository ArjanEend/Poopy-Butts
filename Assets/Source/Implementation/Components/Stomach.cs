using RocketWorks.Entities;
using System.Collections;
using System.Collections.Generic;

public partial class Stomach : IComponent
{
    public List<EntityReference> pickups = new List<EntityReference>();	
}
