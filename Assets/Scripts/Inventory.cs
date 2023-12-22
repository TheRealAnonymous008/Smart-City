using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Inventory 
{
    private Dictionary<ResourceType, int> resources = new();

    public Inventory()
    {
    
    }

    public void Add(ResourceInfo info)
    {
        if (resources.ContainsKey(info.type))
        {
            resources[info.type] += info.quantity;
        } else
        {
            resources.Add(info.type, info.quantity);
        }
    }

    public Resource Drop(ResourceType type, int quantity)
    {
        if (resources.ContainsKey(type))
        {
            throw new System.Exception("Trying to unequip a resource that is not equipped");
        }
        else
        {
            int q = Mathf.Min(quantity, resources[type]);
            resources[type] -= q;
            return new Resource(type, q);
        }
    }
}
