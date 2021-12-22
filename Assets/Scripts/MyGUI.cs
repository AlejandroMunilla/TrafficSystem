using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGUI : MonoBehaviour
{
    private bool openHelp = false;
    public bool courotineActive = false;
    private int labelWidth;
    private int labelHeight;
    private Rect exitButton;
    private Rect helpButton;
    private Rect helpRect;
    private Rect instantiateHumanoidRect;
    private string helpContent;
    
    // Start is called before the first frame update
    void Start()
    {
        labelWidth = (int)(Screen.width * 0.15f);
        labelHeight = (int)(Screen.height * 0.05f);
        exitButton= new Rect(Screen.width - labelWidth, 0, labelWidth, labelHeight);
        helpButton= new Rect(Screen.width - (2*labelWidth), 0, labelWidth, labelHeight);
        helpRect = new Rect(Screen.width * 0.7f, labelHeight, Screen.width * 0.3f, Screen.height * 0.8f);
        instantiateHumanoidRect= new Rect(Screen.width - (3*labelWidth), 0, labelWidth, labelHeight);
        helpContent = "Red Cars/Humanoid use a randomPath navigation system at intersections. Blue cars a fixed one. Use mouse to control camera. Mouse Wheel to zoom in and out. Click on Instatiate Pedestrians to add more to scene";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI ()
    {
        if (courotineActive == false)
        {
            if (GUI.Button(exitButton, "EXIT"))
            {
                Application.Quit();
            }
            if (GUI.Button(helpButton, "HELP"))
            {
                openHelp = !openHelp;
            }
            if (GUI.Button(instantiateHumanoidRect, "NEW HUMANOID"))
            {
                courotineActive = true;
                GetComponent<InstantiateHumanoid>().enabled = true;
            }

            if (openHelp == true)
            {
                GUI.Label(helpRect, helpContent);
            }
        }
        



    }
}
