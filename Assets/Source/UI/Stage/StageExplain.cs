using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageExplain : PUIWindow {

	private const string GUI_NAME = "stagePart1_panel";
	public GameObject exitBtn;
	
	public static StageExplain Create()
	{
		GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
		obj = GameObject.Instantiate(obj);
		StageExplain ret = obj.GetComponent<StageExplain>();
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
