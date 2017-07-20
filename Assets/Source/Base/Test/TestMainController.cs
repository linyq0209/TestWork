using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMainController : PUIWindow {
	public GameObject mBtn;

	//定义按钮的点击事件
	void Start () {
		UIEventListener.Get(mBtn).onClick = OnClickBtn;
	}

	//按钮点击事件的方法
	protected void OnClickBtn(GameObject go)
	{
		// 打开一个界面
		TestCreateController.Create();
	}
	
}
