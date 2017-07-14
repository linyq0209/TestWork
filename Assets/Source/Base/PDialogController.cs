using System;
using UnityEngine;

public class PDialogController : PUIWindow 
{
	public const string DEFAULT_NAME = "dialog_panel";
	public UILabel textLabel;
	public UILabel titleLabel;

	public UILabel[] extraLabel;

	public GameObject confirmBtn;
	public GameObject cancelBtn;

	public bool isAutoClose = true;

	protected Action confirmCallback;
	protected Action cancelCallback;



	public static PDialogController Create(GameObject prefabObj, Action confirmCallback = null, Action cancelCallback = null)
	{
		var obj = GameObject.Instantiate(prefabObj);
		obj.name = prefabObj.name;
		var controller = obj.GetComponent<PDialogController>();
		if(controller != null)
		{
			controller.confirmCallback = confirmCallback;
			controller.cancelCallback = cancelCallback;
		}
		return controller;
	}


	public static PDialogController Create(GameObject prefabObj, string text, Action confirmCallback = null, Action cancelCallback = null)
	{
	
		var controller =  Create(prefabObj, confirmCallback, cancelCallback);
		if(controller != null)
		{
			controller.Refresh(text);
		}
		return controller;
	}

	public static PDialogController Create( string text, Action confirmCallback = null, Action cancelCallback = null)
	{
		GameObject obj = ResourceManager.GetInstance().GetPrefab(DEFAULT_NAME);
		return Create(obj, text, confirmCallback, cancelCallback);
	}

	protected override void InitImpl()
	{
		base.InitImpl();

		if(confirmBtn != null)
		{
			UIEventListener.Get(confirmBtn).onClick = OnClickConfirm;
			
		}
		if(cancelBtn != null)
		{
			UIEventListener.Get(cancelBtn).onClick = OnClickCancel;
		}
	}

	public void SetCallBack(Action confirmCallback , Action cancelCallback)
	{
		this.confirmCallback = confirmCallback;
		this.cancelCallback = cancelCallback;
	}

	public void RefreshExtra(string[] content)
	{
		int maxcount = content.Length > extraLabel.Length ? extraLabel.Length : content.Length;
		for(int i = 0; i < maxcount; i++)
		{
			extraLabel[i].text = content[i] == null ? extraLabel[i].text : content[i];
		}
	}

	public void Refresh(string text)
	{
		if(textLabel != null)
		{
			textLabel.text = text == null ? textLabel.text : text;
		}
	}	

	public void Refresh(string title, string text)
	{
		if(titleLabel != null)
		{
			titleLabel.text = title == null ? titleLabel.text : title;
		}
		Refresh(text);
	}

	protected void OnClickConfirm(GameObject obj)
	{
		if(confirmCallback != null)
		{
			confirmCallback();
		}
		if(isAutoClose)
		{
			Exit();
		}
	}

	protected void OnClickCancel(GameObject obj)
	{
		if(cancelCallback != null)
		{
			cancelCallback();
		}
		Exit();
	}
}
