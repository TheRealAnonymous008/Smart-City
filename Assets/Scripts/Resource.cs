using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ResourceType
{
    FOOD,
    WATER,
    WOOD,
    STONE,
    ORE,
    CLOTH
}

public struct ResourceInfo
{
    public ResourceType type;
    public int quantity;
}

public class Resource : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ResourceType type;
    [SerializeField]
    private int quantity;
     

    public ResourceType Type { get { return type; } private set { type = value; } }
    public int Quantity { get { return quantity; } private set { quantity = value; } }

    public Resource(ResourceType type, int quantity)
    {
        Type = type; 
        Quantity = quantity;
    }

    public ResourceInfo Take(int q)
    {
        int returned = Mathf.Min(Quantity, q);
        Quantity -= returned;

        return new ResourceInfo() { quantity = returned, type = Type}; 
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource otherResource))
        {
            // Merge resources 
            Debug.Log("Hello world");
            if (otherResource.GetInstanceID() < GetInstanceID() && otherResource.Type == Type)
            {
                Quantity += otherResource.Quantity;
                Destroy(otherResource.gameObject);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, this.ToString(), new GUIStyle { 
            fontSize = 10
        });
    }

    public override string ToString()
    {
        return Type.ToString() + ": " + Quantity.ToString();
    }
}
