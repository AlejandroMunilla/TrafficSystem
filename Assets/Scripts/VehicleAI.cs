using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class VehicleAI : MonoBehaviour
{
    private bool activeVehicle = true;
    public bool randomPath = false;
    public Transform target;
    public Crossing crossing;
    private JunctionController jc;
    private JunctionController ownJc;
    public Transform triggerCollider;
    public float maxSpeed = 8;
    public float currentSpeed = 8;
    private NavMeshAgent nav;
    private GameObject front;
    public GameObject activeJunction = null;
    public GameObject frontCar = null;
    float safetyDistance;
    private Rigidbody rb;

    public enum State
    {
        StopCrossing,
        Move,
        StopAtJunction,
        StopLine
    }

    public State state;

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Move;
        StartCoroutine("FSM");
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        front = transform.Find("Front").gameObject;
        safetyDistance = currentSpeed * 0.12f;
     //   Debug.Log(safetyDistance);
        front.transform.localPosition = new Vector3(0, 0, safetyDistance);
        if (gameObject.tag != "Vehicle")
        {
            gameObject.tag = "Vehicle";
        }
    }


    private void Start()
    {

    }



    private IEnumerator FSM ()
    {
        while (activeVehicle)
        {
            switch (state)
            {
                case State.Move:
                    Move();
                    yield return new WaitForSeconds(0.02f);
                    break;

                case State.StopCrossing:
                    StopCrossing();
                    yield return new WaitForSeconds(0.1f);
                    break;

                case State.StopAtJunction:
                    StopAtJuntion();
                    yield return new WaitForSeconds(0.2f);
                    break;

                case State.StopLine:
                    StopLine();
                    yield return new WaitForSeconds(0.1f);
                    break;
            }                
        }
    }


    private void Move ()
    {        
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 2.5f)        
        {
            ChooseNextTarget();
        }

        if (nav != null)
        {
            if (target != null)
            {
                nav.destination = target.position;
           //     Debug.Log("Working");
            }
        }

        
        if (frontCar != null)
        {
            float distanceToCar = Vector3.Distance(transform.position, frontCar.transform.position);
        //    Debug.Log (distanceToCar + "/" + gameObject.name);
            if (distanceToCar < 8 )
            {
                nav.isStopped = true;
            }
            
            else if (distanceToCar < safetyDistance + 10)
            {
                nav.speed = maxSpeed * 0.75f;
            }

            else
            {
                nav.speed = maxSpeed;
                nav.isStopped = false;
                frontCar = null;
            }
        }


    }

    public void ChangeToStopCrossing ()
    {
        nav.isStopped = true;
        state = State.StopCrossing;
    }

    private void StopCrossing ()
    {
    //    Debug.Log("Stopping" + "/" + crossing.pedestrianCrossing);
        
        if (crossing.pedestrianCrossing == false)
        {
            state = State.Move;
            nav.isStopped = false;
      //      Debug.Log("Resume");
        }
    }

    public void ChangeStopAtJunction (JunctionController junctionController, JunctionController ownJunctionCntroller)
    {
 //      Debug.Log("ChangedtoJunction");
        jc = junctionController;                                                //It is needed for the Couroutine to work on Funcion "StopAtJuntion"
        ownJc = ownJunctionCntroller;
        state = State.StopAtJunction;
        nav.speed = maxSpeed * 0.3f;
    }

    private void StopAtJuntion ()
    {
        Transform stopTransform = ownJc.transform.Find("Stop");
        float distanceToStop = Vector3.Distance(transform.position, stopTransform.position);


        if (distanceToStop > 3)
        {
            nav.isStopped = false;
            nav.destination = stopTransform.position;    
        }
        else
        {
      //      Debug.Log("Junction");
            nav.isStopped = true;
            bool safeToContinue = true;

            
            foreach (GameObject go2 in ownJc.higherPriority)
            {
       //         Debug.Log(transform.root.name + "/" + go2.name + "/" + jc.name);

                JunctionController jcPriority = go2.GetComponent<JunctionController>();
                if (jcPriority.vehicleList.Count > 0)
                {
                    safeToContinue = false;
                }
            }

            if (safeToContinue)
            {
                //    rb.constraints = RigidbodyConstraints.None;
                Invoke("JunctionToMove", 3);
            }
        }    

    }

    private void JunctionToMove ()
    {
        nav.isStopped = false;
        nav.speed = maxSpeed;
        nav.destination = target.position;
        state = State.Move;
    }

    public void ChangeStopLine (GameObject carToCheck)
    {
        nav.isStopped = true;
        frontCar = carToCheck;
        state = State.StopLine;
    }

    private void StopLine ()
    {
        nav.isStopped = true;
        float distanceToFrontCar = Vector3.Distance(front.transform.position, frontCar.transform.position);
        if (distanceToFrontCar > ( safetyDistance * 1.1f))
        {
            nav.isStopped = false;
            state = State.Move;
        }
    }


    private void ChooseNextTarget ()
    {
        if (randomPath == false || target.gameObject.GetComponent<MillStone>().millStones.Count == 0)
        {
            target = target.gameObject.GetComponent<MillStone>().nextMillStones.transform;
    //        Debug.Log(target.name);
        }
        else
        {
            if (target.gameObject.GetComponent<MillStone>().millStones.Count > 0)
            {

                int randomNo = Random.Range(0, target.gameObject.GetComponent<MillStone>().millStones.Count);
                //              Debug.Log(randomNo + "/" + target.gameObject.GetComponent<MillStone>().millStones.Count);
                target = target.gameObject.GetComponent<MillStone>().millStones[randomNo].transform;
            }
        }
    }


}
