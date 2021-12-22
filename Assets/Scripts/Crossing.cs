using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Crossing : MonoBehaviour
{

    public List<GameObject> pedestrianList = new List<GameObject>();
    public bool pedestrianCrossing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter (Collider col)
    {
        if (col.tag == "NPC")
        {
            pedestrianList.Add(col.gameObject);
            pedestrianCrossing = true;
      //      Debug.Log("Pedestrian");
        }
    }

    private void OnTriggerExit (Collider col)
    {
        if (col.tag == "NPC")
        {
            pedestrianList.Remove (col.gameObject);
      //      Debug.Log(pedestrianList.Count);
            if (pedestrianList.Count == 0)
            {
                pedestrianCrossing = false;
            }
        }
    }
}
