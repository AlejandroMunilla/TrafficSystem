///This class holds the info of the connected millstones, the pathfinding system. 
///There are two options; 1) a set path on the millstones for cars and pedestrians to follow the same route
///A truly AI random chosen system. Pedestrian and cars randomly choose a path when there is more than one option 
///Alejandro Munilla, Dec 27, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillStone : MonoBehaviour
{

    public GameObject nextMillStones;
    public List<GameObject> millStones = new List<GameObject>();
    public bool debuggingMode = false;                                   //Turn true this only for developer, testing mode to see markers on game 
                                                                         //and lines conecting markers. 


    public void Awake ()
    {
        debuggingMode = false;
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
