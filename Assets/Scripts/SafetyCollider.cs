using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyCollider : MonoBehaviour
{
    private VehicleAI vAI;

    void Start()
    {
        vAI = transform.root.GetComponent<VehicleAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        if (go.tag == "Vehicle")
        {
            Debug.Log("Safety");
            vAI.frontCar = go;

        }
    }


}
