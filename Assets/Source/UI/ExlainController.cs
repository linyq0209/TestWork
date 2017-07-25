using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlainController : PUIWindow {
   private const string GUI_NAME = "explain_panel";
   public GameObject exParentObj;
   public GameObject exPrefabObj;
   public Event anyKey;
   
   public static ExlainController Create()
   {
    
    GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);

    obj = GameObject.Instantiate(obj);

    obj.transform.localPosition = new Vector3(-273,136,0);

    ExlainController ret = obj.GetComponent<ExlainController>();

    return ret;
   }

   

   void Start()
   {
     
     CreateChildItem();
     if(Input.GetMouseButton(0))
     {
     	Debug.Log("123");
     	OnExit();
     }
     
     
   }

   void Updata()
   {

   }

    protected void CreateChildItem()
    {
      for(int i=0;i<1;i++)
      {
         TestItemController.Create(exParentObj,exPrefabObj);

      }
         exParentObj.GetComponent<UIGrid>().Reposition();
    }

     

}
