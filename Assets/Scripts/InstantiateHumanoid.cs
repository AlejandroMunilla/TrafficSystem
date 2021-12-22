using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateHumanoid : MonoBehaviour
{

    private bool working = true;
    private bool displayWarning = false;
    private int labelWidth;
    private int labelHeight;
    private Rect helpRect;
    private Rect warningRect;
    private string tooltip;
    private string warningTip;
    private MyGUI myGUI;
    private GameObject pedestrian;
    private enum State
    {
        Seq01,
        Seq02,
        Seq03,
        Seq04
    }
    private State state;


    private void Awake()
    {
        myGUI = GetComponent<MyGUI>();
        GameObject pedestrian = (GameObject)Resources.Load("Ethan");
    }

    private void OnEnable ()
    {
        labelWidth = (int)(Screen.width * 0.08f);
        labelHeight = (int)(Screen.height * 0.05f);
        helpRect = new Rect(Screen.width * 0.7f, labelHeight, Screen.width * 0.3f, Screen.height * 0.8f);
        warningRect = new Rect(Screen.width * 0.25f, labelHeight, Screen.width * 0.50f, Screen.height * 0.15f);
        tooltip = "Left Click on a suitable pedestrian area to instantiate a new pedestrian. Or Right Click to cancel this action";
        warningTip = "This area is not suitable to instantiate a pedestrian. Try in a pedestrian area";
        StartCoroutine("FSM");
    }
        
    private IEnumerator FSM()
    {
        while (working)
        {
            switch(state)
            {
                case State.Seq01:
                    yield return new WaitForSeconds(0);
                    Seq01();
                    break;

                case State.Seq02:
                    yield return new WaitForSeconds(0.01f);
                    break;


                case State.Seq03:
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq04:
                    yield return new WaitForSeconds(0.01f);
                    break;

            }
        }
    }

    private void OnGUI()
    {
        GUI.Label(helpRect, tooltip);

        if (displayWarning)
        {
            GUI.Label (warningRect, warningTip);
        }
    }

    private void Seq01 ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                Debug.Log(hit.point);
                Transform tile = hit.transform;
                Debug.Log(hit.transform.name);
                if (tile.Find ("Right") != null)
                {
                    InstatiatePedestrian(hit.point,tile);
                }
                else
                {
                    Invoke("TurnOffWarning", 4);
                }
              
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("End");
            StopCoroutine("FSM");
            myGUI.courotineActive = false;
            this.enabled = false;
        }
    }

    private void InstatiatePedestrian (Vector3 position, Transform tile)
    {
        
        pedestrian = Instantiate((GameObject)Resources.Load("Ethan"),tile.Find("Right").position, Quaternion.identity);
        pedestrian.GetComponent<PedestrianAI>().target = tile.Find("Right");
        pedestrian.SetActive(true);
    }

    private void TurnOffWarning ()
    {
        displayWarning = false; 
    }

    private void OnDisable()
    {
        StopCoroutine("FSM");
    }
}
