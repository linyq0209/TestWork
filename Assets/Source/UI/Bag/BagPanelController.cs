using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanelController : PUIWindow {
	private const string GUI_NAME = "bag_panel";
	public GameObject parentObj;
	public GameObject prefabObj;
	public GameObject exitBtn;
	public static BagPanelController Create()
	{
		GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
		obj = GameObject.Instantiate(obj);
		BagPanelController ret = obj.GetComponent<BagPanelController>();
		return ret;
	}

	void Start()
	{
		UIEventListener.Get(exitBtn).onClick = OnExitPanel;
		CreateChildItem();
	}

	protected void CreateChildItem()
	{
		for(int i=0;i<30;i++)
		{
			TestItemController.Create(parentObj,prefabObj);
		}
		parentObj.GetComponent<UIGrid>().Reposition();
	}

	protected void OnExitPanel(GameObject go)
	{
		OnExit();
	}
}
