using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExitController : PUIWindow {

    public GameObject  explain_panel; 
    
    public void OnExitPanel(GameObject go)
    {
    	
    	explain_panel.SetActive(false);
    	//Destroy(explain_panel);
    }
	
	
	
	
}
