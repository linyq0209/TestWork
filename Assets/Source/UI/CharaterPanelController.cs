using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterPanelController : PUIWindow {
    private const string GUI_NAME = "charater_panel";
	public GameObject chParentObj;
	public GameObject chPrefabObj;	
	public GameObject chExitBtn;

		
	public static CharaterPanelController Create()
	{
		GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
		
		obj = GameObject.Instantiate(obj);

        obj.transform.localPosition = new Vector3(-579,37,0);

		CharaterPanelController ret = obj.GetComponent<CharaterPanelController>();
	    return ret; 
	}
    
    
	void Start()
	{                     
        UIEventListener.Get(chExitBtn).onClick = OnExitPanel;
       
        CreateChildItem();
     } 
    
    
    protected void CreateChildItem()
    {
    	for(int i=0;i<15;i++)
    	{
    		TestItemController.Create(chParentObj,chPrefabObj);
    	}
    	chParentObj.GetComponent<UIGrid>().Reposition();    	    	    	                
    }

    protected void OnExitPanel(GameObject go)
	{
		
		OnExit();

	}



}

