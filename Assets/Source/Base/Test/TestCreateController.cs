using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreateController : PUIWindow {
	private const string GUI_NAME = "test_create_panel";
	public GameObject parentObj;
	public GameObject prefabObj;

	public static TestCreateController Create()
	{
		//根据名字获取到GameObject
		GameObject obj = ResourceManager.GetInstance().GetPrefab(GUI_NAME);
		//根据得到的GameObject创建出一个对象
		obj = GameObject.Instantiate(obj);
		//获取这个对象的这个脚本
		TestCreateController ret = obj.GetComponent<TestCreateController>();
		return ret;
	}

	void Start()
	{
		CreateChildItem();
	}
	//创建子物体
	protected void CreateChildItem()
	{
		//循环创建子物体
		for(int i=0;i<30;i++)
		{
			TestItemController.Create(parentObj,prefabObj);
		}
		parentObj.GetComponent<UIGrid>().Reposition();
	}
}
