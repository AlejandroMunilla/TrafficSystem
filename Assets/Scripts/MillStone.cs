using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillStone : MonoBehaviour
{

    public GameObject nextMillStones;
    public List<GameObject> millStones = new List<GameObject>();
    private bool debuggingMode = false;                                   //Turn true this only for developer, testing mode to see markers on game 
                                                                         //and lines conecting markers. 


    public void Awake ()
    {
        if (debuggingMode == true)
        {
            DebuggingMode();
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
      
    }


    //Use this only for developpers. 
    private void DebuggingMode ()
    {
       
        Debug.DrawLine(transform.position, nextMillStones.transform.position, Color.blue, Mathf.Infinity);

        if (millStones.Count > 0)
        {
            foreach (GameObject go in millStones)
            {
                Debug.DrawLine(transform.position, go.transform.position, Color.red, Mathf.Infinity);
            }
        }
    }

}
