using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private List<ResourceInfo> resources = new List<ResourceInfo>();
    [SerializeField]
    private GameObject resourcePrefab; 

    public void Awake()
    {
        foreach(Resource r in GetComponentsInChildren<Resource>())
        {
            resources.Add(r.GetInfo());
        }
    }

    public void ResetEnvironment()
    {
        foreach(Resource r in GetComponentsInChildren<Resource>())
        {
            Destroy(r.gameObject);
        }

        foreach(ResourceInfo r in resources)
        {
            GameObject obj = Instantiate(resourcePrefab, transform);
            Resource resource = obj.GetComponent<Resource>();
            resource.Quantity = r.quantity;
            resource.Type = r.type;
            obj.transform.localPosition = r.position;
            obj.transform.localScale = r.scale;
        }
    }
}
