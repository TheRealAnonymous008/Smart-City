using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

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

    [SerializeField]
    private Transform targetTransform;

    private Inventory inventory = new Inventory();

    private ActionKey currentAction;

    // Configure the Agent's starting state 
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(10, 0.25f, 0);
        inventory = new Inventory();
        currentAction = ActionKey.IDLE;
    }

    // Collect observations from the environment based on the defined sensor information
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }

    // Act upon each action.
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed;
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

    // Provide Rewards
    // If we require physics, this can be made as OnColliderStay. Otherwise, use OnTriggerStay
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource) && IsActionSet(ActionKey.INTERACT))
        {
            Debug.Log("Success!");
            inventory.Add(resource.Take(5));
            UnsetAction(ActionKey.INTERACT);
        }

        if (other.TryGetComponent(out Obstacle obstacle))
        {
            AddReward(-10f);
            EndEpisode();
        }
    }

    private void SetAction(ActionKey key) => currentAction |= key;
    private void UnsetAction(ActionKey key) => currentAction &= ~key;
    private bool IsActionSet(ActionKey key) => (currentAction & key) != 0;
}
