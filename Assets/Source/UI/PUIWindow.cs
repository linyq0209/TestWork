using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class PUIWindow : PUIBaseWidget
{
	public bool isWith3D = false;
	public bool isHidePre = false;
	public bool isClose3DUI = false;
	public bool isMaskTouchDown = true;
	public bool isShowMask = true;



	protected UIPanel mPanel;
	protected BoxCollider[] mBoxColliders;

	protected BoxCollider mBoxCollider;

	protected bool mIsBaseWindow = false;

	protected float mZValue = 0;
	

	protected override void InitImpl()
	{
		SetBoxCollider();
		mPanel = gameObject.GetComponent<UIPanel>();
		PUIManager.GetInstance().PushUIWindow(this);
		AddAction(OnExitImpl, EXIT_ACTION);
	}

	protected void OnExitImpl()
	{
		PUIManager instance = PUIManager.GetInstance(false);
		if(instance != null)
		{
			instance.PopUIWindow(this);
		}
	}

	public virtual void OnFrontground()
	{
	}

	public virtual void OnBackground()
	{
		
	}

	

	public bool IsBaseWindow()
	{
		return mIsBaseWindow;
	}

	public UIPanel GetPanel()
	{
		return mPanel;
	}

	public void SetZValue(float z)
	{
		mZValue = z;
		Vector3 localPos = transform.localPosition;
		localPos.z = z;
		transform.localPosition =  new Vector3(0, 0, z);
	}

	public float GetZValue()
	{
		return mZValue;
	}

	public void SetDepth(int curDepth)
	{
		NGUITools.AdjustDepth(gameObject,curDepth);
		mPanel.depth = curDepth;
	}

	public int GetDepth()
	{
		return mPanel.depth;
	}


	public void EnableTouch()
	{
		HandleEnable(true);
	}

	public void DisableTouch()
	{
		HandleEnable(false);
	}

	protected void HandleEnable(bool isEnable)
	{
		if (!isEnable)
		{
			mBoxColliders = transform.GetComponentsInChildren<BoxCollider>();
			foreach(BoxCollider boxCollider in mBoxColliders)
			{
				boxCollider.enabled = isEnable;
			}
		}
		else
		{
			if (mBoxColliders!=null)
			{
				foreach(BoxCollider boxCollider in mBoxColliders)
				{
					if (boxCollider!=null)
					{
						boxCollider.enabled = isEnable;
					}
				}
			}
		}
	}

	protected void SetBoxCollider()
	{
		if(mBoxCollider == null && isMaskTouchDown)
		{
			GameObject obj = null;
			UIWidget widget = null;
			if(isShowMask)
			{
				GameObject maskSprite = ResourceManager.GetInstance().GetPrefab("mask_ui_sprite");
				obj = NGUITools.AddChild(gameObject, maskSprite);
				widget = obj.GetComponent<UIWidget>();
			}else
			{
				obj = NGUITools.AddChild(gameObject);
				widget = obj.AddComponent<UIWidget>();
			}
			
			obj.name = "puiwindow_common_boxcollider";
			widget.depth = -100;
			mBoxCollider = obj.AddComponent<BoxCollider>();
			mBoxCollider.size = new Vector3(80000, 80000, 2);
			 
		}

	}

	public void SetMaskShow(bool flag)
	{
		var sprite = mBoxCollider.gameObject.GetComponent<UISprite>();
		if(sprite != null)
		{
			sprite.enabled = flag;
		}
	
	}
}