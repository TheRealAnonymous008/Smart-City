using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor;

// Constants which specify the action and state space of each local agent
// This is necessary since MLAgents encodes everything in a vector. This simply makes things more 
// Readable 

public enum ActionKey
{
    IDLE = 0,
    MOVE_X = 1, 
    MOVE_Z = 2,
    INTERACT = 4, 
    DROP = 8,
}

public enum ObservationSpace
{

}

public class LocalAgent : Agent
{
    [SerializeField]
    private float speed = 1f;

    private Vector3 initialPosition; 

    [SerializeField]
    private Environment environment;

    private Inventory inventory = new Inventory();
    private ActionKey currentAction;

    // Miscellaneous 
    private float aliveReward;
    private int collected;

    public delegate void _Event();
    public event _Event OnAgentDeath;

    // Configure the Agent's starting state 
    public override void Initialize()
    {
        base.Initialize();
        initialPosition = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = initialPosition;
        transform.Rotate(Vector3.up, Random.Range(0, 360));
        inventory = new Inventory();
        currentAction = ActionKey.IDLE;
        aliveReward = 0;
        collected = 0;
    }

    // Collect observations from the environment based on the defined sensor information
    public override void CollectObservations(VectorSensor sensor)
    {

    }

    // Act upon each action.
    public override void OnActionReceived(ActionBuffers actions)
    {
        float step = actions.ContinuousActions[0];
        float turn = actions.ContinuousActions[1];

        transform.position += transform.forward * Time.deltaTime * speed * step;
        transform.Rotate(Vector3.up, turn * Time.deltaTime * 180f);

        if (aliveReward < 10)
        {
            AddReward(0.01f);
            aliveReward += 0.01f;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.E))
            SetAction(ActionKey.INTERACT);
        else
            UnsetAction(ActionKey.INTERACT);

        if (Input.GetKey(KeyCode.X))
            SetAction(ActionKey.DROP);
        else
            UnsetAction(ActionKey.DROP);
    }

    public void Kill()
    {
        EndEpisode();
    }

    // Provide Rewards
    // If we require physics, this can be made as OnColliderStay. Otherwise, use OnTriggerStay
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
        {
            if (IsActionSet(ActionKey.INTERACT))
            {
                inventory.Add(resource.Take(5));
                UnsetAction(ActionKey.INTERACT);
            }

            AddReward(resource.Take(resource.Quantity).quantity);
            aliveReward = 0;
            collected += 1;
            if (collected == 6)
            {
                AddReward(100);
            }
        }

        if (other.TryGetComponent(out Obstacle obstacle))
        {
            AddReward(-100f);
            OnAgentDeath?.Invoke();
        }
    }

    private void SetAction(ActionKey key) => currentAction |= key;
    private void UnsetAction(ActionKey key) => currentAction &= ~key;
    private bool IsActionSet(ActionKey key) => (currentAction & key) != 0;
}
