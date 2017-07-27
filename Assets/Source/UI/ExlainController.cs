using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlainController : MonoBehaviour {
   private const string GUI_NAME = "explainItem";
   private const string GUI_HEAD = "head";
   public GameObject onExitBtn;
   public GameObject exPrefabObj;
   public GameObject head;
   
   
   public static ExlainController Create ()
   {
    //GameObject exParentObj = ResourceManager.GetInstance().GetPrefab(GUI_HEAD);
    GameObject exPrefabObj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
    GameObject obj = NGUITools.AddChild(head,exPrefabObj);
    exPrefabObj.transform.parent = head.transform;
    ExlainController ret = obj.GetComponent<ExlainController>();
    return ret;
   }

   void Start()
   {    
     UIEventListener.Get(onExitBtn).onClick = OnExitPanel;         
   }

    protected void OnExitPanel(GameObject go)
	{
		Destroy(gameObject);
	}
}
