using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class AgentController : Agent
{

    [SerializeField] private Transform target; 
    [SerializeField] private float movespeed = 4f;                 

    private Rigidbody rb;


    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }


    public override void OnEpisodeBegin(){
        //Agent
        transform.localPosition = new Vector3(Random.Range(-4f,4f) , 0.3f , Random.Range(-4f,4f) );         //random placement 
        
        //Pellet
        target.localPosition = new Vector3(Random.Range(-4f,4f) , 0.3f , Random.Range(-4f,4f) );            //random placement
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions){
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        rb.MovePosition(transform.position + transform.forward * moveForward * movespeed * Time.deltaTime);
        transform.Rotate(0f,moveRotate * movespeed, 0f, Space.Self);
        /*
        Vector3 velocity = new Vector3(moveX, 0f, moveZ);
        velocity = velocity.normalized * Time.deltaTime * movespeed;


        transform.localPosition += velocity;
        */
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");                                  //for A and D
        continuousActions[1] = Input.GetAxisRaw("Vertical");                                    //for W and S
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Pellet"){
            AddReward(5f);
            EndEpisode();
        }
        if(other.gameObject.tag == "Wall"){
            AddReward(-1f);
            EndEpisode();
        }
    }
}
