using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerCollider : MonoBehaviour
{

    private VehicleAI vAI;
    private GameObject junction;
    // Start is called before the first frame update
    void Start()
    {
        vAI = transform.root.GetComponent<VehicleAI>();
    }


    private void OnTriggerEnter (Collider col)
    {


        
        GameObject go = col.gameObject;
        
        if (go.tag == "Crossing")
        {
            
            if (go.GetComponent<Crossing>() != null)
            {
                Crossing crossing =  go.GetComponent<Crossing>();
                if (crossing.pedestrianCrossing == true )
                {
              //      Debug.Log("Crossing Pedestrian true");
                    vAI.crossing = crossing;
                    vAI.ChangeToStopCrossing();
                }
            }
        }
        if (go.tag == "Junction")
        {



            if (vAI.activeJunction == null)
            {
                vAI.activeJunction = go;
                junction = go;
                JunctionController jc = junction.GetComponent<JunctionController>();
                jc.vehicleList.Add(transform.parent.gameObject);
                CheckJunction(go);
            }
         
        }

      

        if (go.tag == "Vehicle")
        {
       
            vAI.frontCar = go;
            
        }
    }


    private void OnTriggerStay (Collider col)
    {

        GameObject go = col.gameObject;

        if (go.tag == "Vehicle")
        {
            vAI.frontCar = go;

        }
        if (go.tag == "Junction")
        {
            if (vAI.activeJunction == null)
            {

                if (vAI.activeJunction == null)
                {
                    vAI.activeJunction = go;
                }
            }

            if (vAI.state != VehicleAI.State.StopAtJunction)
            {
                CheckJunction(go);
            }

        }        
    }

    private void OnTriggerExit (Collider col)
    {
        GameObject go = col.gameObject;
        if (go.tag == "Junction")
        {
            JunctionController jc = go.GetComponent<JunctionController>();
            jc.vehicleList.Remove(transform.parent.gameObject);
            //     Debug.Log(go.name + "/removed");
            CancelInvoke("ContinueCheckingVehicles");
            Invoke("RemoveActiveJunction", 8);
        }
    }



    private void CheckJunction (GameObject go)
    {
        junction = go;
 
        if (vAI.activeJunction == go)
        {
            JunctionController jc = junction.GetComponent<JunctionController>();

            if (jc.higherPriority.Count > 0)
            {
                foreach (GameObject go2 in jc.higherPriority)
                {

                    JunctionController jcPriority = go2.GetComponent<JunctionController>();

                    if (jcPriority.vehicleList.Count > 0)
                    {
                        if (vAI.state != VehicleAI.State.StopAtJunction)
                        {
                            vAI.ChangeStopAtJunction(jcPriority, jc);
                        }                   

                    }
                }

            }
        }

    }

    private void RemoveActiveJunction ()
    {
     //   Debug.Log(transform.parent.name + "/Remove Active Junciton");
        vAI.activeJunction = null;
    }


}
