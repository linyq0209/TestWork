using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpController : PUIWindow {
	private const string GUI_NAME = "setUp_panel";	
	public GameObject closeBtn;
	public static SetUpController Create()
	{
		GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
		obj = GameObject.Instantiate(obj);
		SetUpController ret = obj.GetComponent<SetUpController>();
		return ret;
	}

	void Start()
	{
		//transform.localPosition = new Vector3(18,300,0);
		UIEventListener.Get(closeBtn).onClick = OnExitPanel;
	}

	protected void OnExitPanel(GameObject go)
	{
		OnExit();
	}


}
