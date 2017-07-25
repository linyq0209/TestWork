using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlainController : PUIWindow {
   private const string GUI_NAME = "explain_panel";
   public GameObject exParentObj;
   public GameObject exPrefabObj;
   
   public static ExlainController Create()
   {
    
    GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);

    obj = GameObject.Instantiate(obj);

    ExlainController ret = obj.GetComponent<ExlainController>();

    return ret;
   }

   void Start()
   {
     
 


   }

    protected void ChildCreate(GameObject go)
    {
      for(int i=0;i<15;i++)
      {
         TestItemController.Create(exParentObj,exPrefabObj);

      }
         exParentObj.GetComponent<UIGrid>().Reposition;
    }

     protected void OnExitPanel(GameObject go)
     {
     	OnExit();
     }

}
