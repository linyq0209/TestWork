using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlainController : PUIWindow {
    private const string GUI_NAME = "explain_panel";
    public GameObject exitBtn;
   
    public static ExlainController Create(GameObject parent)
    {

      GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);

      obj = GameObject.Instantiate(obj);
      
      obj.transform.position = parent.transform.position;

      ExlainController ret = obj.GetComponent<ExlainController>();

      return ret;
    }

    void Start()
    {    
        UIEventListener.Get(exitBtn).onClick = OnExitPanel;       
    }

    protected void OnExitPanel(GameObject go)
    {
      Destroy(gameObject);
    }
}
