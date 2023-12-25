using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public List<LocalAgent> Agents { get; private set; }

    [SerializeField]
    private Environment environment;

    public void Awake()
    {
        Agents = new List<LocalAgent>();
        foreach (LocalAgent agent in GetComponentsInChildren<LocalAgent>())
        {
            Agents.Add(agent);
            agent.OnAgentDeath += ResetEnvironment;
        }
    }

    public void FixedUpdate()
    {
        if (environment.ResourceCount() <= 0)
        {
            ResetEnvironment();
        }
    }

    public void ResetEnvironment()
    {
        foreach(LocalAgent agent in Agents)
        {
            agent.Kill();
        }

        environment.ResetEnvironment();
    }
}
