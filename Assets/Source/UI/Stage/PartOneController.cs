using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOneController : PUIWindow {

	private const string GUI_NAME = "partOne_panel";
	public GameObject exitBtn;
	
	public static PartOneController Create()
	{
		GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
		obj = GameObject.Instantiate(obj);
		PartOneController ret = obj.GetComponent<PartOneController>();
		return ret;
	}
	protected void OnExitPanel(GameObject go)
	{
        OnExit();
	}

	void Start()
	{
		UIEventListener.Get(exitBtn).onClick = OnExitPanel;
	}
}
