using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanelController : PUIWindow {

	private const string GUI_NAME = "stage_panel";
	public GameObject parentObj;
	public GameObject prefabItem1;
	public GameObject prefabItem2;
	public GameObject prefabItem3;
	public GameObject exitBtn;

	public static StagePanelController Create()
	{
		GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
		obj = GameObject.Instantiate(obj);
		StagePanelController ret = obj.GetComponent<StagePanelController>();
		return ret;
	}

	void Start()
	{
		UIEventListener.Get(exitBtn).onClick = OnExitPanel;
	}

	protected void OnExitPanel(GameObject go)
	{
		OnExit();
	}
}

