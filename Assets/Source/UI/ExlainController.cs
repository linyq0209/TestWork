using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlainController : PUIWindow {
   private const string GUI_NAME = "explain_panel";
   public GameObject exParentObj;
   public GameObject exPrefabObj;
   //public GameObject explain_panel;
   public static ExlainController Create()
   {
    
    GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);

    obj = GameObject.Instantiate(obj);

    obj.transform.localPosition = new Vector3(-43,141,0);

    ExlainController ret = obj.GetComponent<ExlainController>();

    return ret;
   }

   
   void Start()
   {    
     CreateChildItem();   
     //UIEventListener.Get(explain_panel).onClick =  OnExitPanel;           
   }

   
    protected void CreateChildItem()
    {      
         TestItemController.Create(exParentObj,exPrefabObj);
         
         exParentObj.GetComponent<UIGrid>().Reposition();
    }

    protected void OnExitPanel(GameObject go)
	{
		//OnExit();
		Debug.Log("23456");
	}
    
   
}
