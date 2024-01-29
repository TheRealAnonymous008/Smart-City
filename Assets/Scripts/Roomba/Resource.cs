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
    public Vector3 position;
    public Vector3 scale; 
}

public class Resource : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ResourceType type;
    [SerializeField]
    private int quantity;
     

    public ResourceType Type { get { return type; } set { type = value; } }
    public int Quantity { get { return quantity; } set { quantity = value; } }

    public Resource(ResourceType type, int quantity)
    {
        Type = type; 
        Quantity = quantity;
    }

    public ResourceInfo Take(int q)
    {
        int returned = Mathf.Min(Quantity, q);
        Quantity -= returned;

        if (Quantity == 0)
        {
            GameObject.Destroy(gameObject);
        }

        return new ResourceInfo() { quantity = returned, type = Type };
    }

    public ResourceInfo GetInfo()
    {
        return new ResourceInfo() { quantity = Quantity, type = Type , position = transform.localPosition, scale = transform.localScale};
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
        //Handles.Label(transform.position, this.ToString(), new GUIStyle { 
        //    fontSize = 10
        //});
    }

    public override string ToString()
    {
        return Type.ToString() + ": " + Quantity.ToString();
    }
}
