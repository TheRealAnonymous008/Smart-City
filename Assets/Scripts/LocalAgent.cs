using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// Constants which specify the action and state space of each local agent
// This is necessary since MLAgents encodes everything in a vector. This simply makes things more 
// Readable 

public enum ActionSpace
{
    MOVE_X = 0, 
    MOVE_Z = 1,
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

    // Configure the Agent's starting state 
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(10, 0.25f, 0);
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
    }

    // Provide Rewards
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            AddReward(10f);
            EndEpisode();
        }
        else if (other.TryGetComponent(out Obstacle obstacle))
        {
            AddReward(-10f);
            EndEpisode();      
        }
    }
}
