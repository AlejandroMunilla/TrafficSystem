using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianAI : MonoBehaviour
{
    private bool activeVehicle = true;
    public Transform target;
    private Transform latestMillStone;
    public Crossing crossing;
    private NavMeshAgent nav;
    private Animator anim;
    [SerializeField]
    private bool randomPath = false;


    public enum State
    {
        StopCrossing,
        Move
    }

    public State state;

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Move;
        StartCoroutine("FSM");
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        latestMillStone = target;
    }


    private void Start()
    {

    }



    private IEnumerator FSM()
    {
        while (activeVehicle)
        {
            switch (state)
            {
                case State.Move:
                    Move();
                    yield return new WaitForSeconds(0.1f);
                    break;

                case State.StopCrossing:
                    StopCrossing();
                    yield return new WaitForSeconds(0.1f);
                    break;
            }
        }
    }


    private void Move()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 0.6)
        {
            ChooseNextTarget();
        }

        if (nav != null)
        {
            if (target != null)
            {
                nav.destination = target.position;
                anim.SetFloat("Forward", 0.4f);
            }
        }
    }

    //In case we wanted to add later traffic lights. 
    public void ChangeToStopCrossing()
    {
        nav.isStopped = true;
        state = State.StopCrossing;
    }

    private void StopCrossing()
    {
        Debug.Log("Stopping" + "/" + crossing.pedestrianCrossing);

        if (crossing.pedestrianCrossing == false)
        {
            state = State.Move;
            nav.isStopped = false;
            Debug.Log("Resume");
        }
    }

    private void ChooseNextTarget()
    {
        latestMillStone = target;

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
          
                target = target.gameObject.GetComponent<MillStone>().millStones[randomNo].transform;
            }
        }

    }
}
