using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ResourceTypes
{
    FOOD,
    WATER,
    WOOD,
    STONE,
    ORE,
    CLOTH
}

public class Resource : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ResourceTypes type;

    [SerializeField]
    private int quantity = 25; 

    public int Take(int q)
    {
        int returned = Mathf.Min(quantity, q);
        quantity -= returned;

        return returned;
    }
}
