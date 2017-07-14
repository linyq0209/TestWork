using System.Collections.Generic;
using UnityEngine;


public class PUIManager : PMonoSingleton<PUIManager>
{

	public const int TOP_DEPTH = 798 ;
	public float incZInterval = -500;

	public int 	depthAdded = 10;
	public int 	startDepth = 0;

	public int curDepth = 0;
	public float currentZ = 0;

	public Camera camera2D;
	public Camera camera3D;


	protected Transform mAnchor;
	protected UICamera  mUICamera;
	protected SortedList<int, PUIWindow> mWindowStack = new SortedList<int, PUIWindow>();
	//protected Dictionary<string, PUIWindow> mWinContainer = new Dictionary<string, PUIWindow>();

	protected PUIWindow mCurrentWindow;


	public T CreateWindow<T> (string windowName)  where T : PUIWindow
	{
		GameObject win = ResourceManager.GetInstance().GetPrefab(windowName);
		win = GameObject.Instantiate(win);
		win.name = windowName;
		T controller = win.GetComponent<T>();
		controller.Init();
		//mWinContainer.Add(windowName, controller);
		return controller;
	}

	public PUIManager()
	{
		mIsNotDestroyOnLoad = false;
	}

	protected override void Init()
	{
		curDepth = startDepth;
		if(camera2D == null)
		{
			mUICamera =	 GameObject.FindObjectOfType<UICamera>();
			if(mUICamera != null)
			{
				camera2D = mUICamera.gameObject.GetComponent<Camera>();
			}	
		}else
		{
			mUICamera = camera2D.GetComponent<UICamera>();
		}
		if(camera3D == null)
		{
			camera3D = Camera.main;
		}
	}

	public UICamera GetUICamera()
	{
		return camera2D.GetComponent<UICamera>();
	}

	public GameObject GetUIAnchor()
	{
		if(mAnchor == null)
		{
			mAnchor = camera2D.transform;
		}
		return mAnchor.gameObject;
	}

	public int GetWindowCount()
	{
		return mWindowStack.Count;
	}

	public PUIWindow GetTopWindow()
	{
		if(mWindowStack.Count > 0)
		{
			return mWindowStack.Values[0];
		}
		return null;
	}

	public bool CreateTopLayer(Transform topLayer)
	{
		if(camera2D == null)
		{
			return false;
		}

		topLayer.parent = camera2D.transform;
		topLayer.localScale = new Vector3(1, 1, 1);
        NGUITools.AdjustDepth(topLayer.gameObject, TOP_DEPTH );
        topLayer.GetComponent<UIPanel>().depth = TOP_DEPTH;

        Vector3 localPos = topLayer.localPosition;
        localPos.z = currentZ;
        topLayer.localPosition = new Vector3(0,0, currentZ);
        return true;
	}

	public void PopUIWindow(PUIWindow win)
	{
		if(!mWindowStack.ContainsValue(win))
		{
			return;
		}

		PUIWindow topWin = GetTopWindow();

		mWindowStack.Remove(-win.GetPanel().depth );
		
		if (topWin == win )
		{
			if (topWin.isWith3D)
			{
				SetCurrentZ(topWin.GetZValue());
			}
			topWin = GetTopWindow();
			if (topWin != null)
			{
				//topWin.EnableTouch();
				topWin.OnFrontground();
				if(win.isHidePre)
				{	
					topWin.Show();
				}
				curDepth = topWin.GetPanel().depth;
				Enable3DCamera(!topWin.isClose3DUI);
			}
			else
			{
				curDepth = startDepth;
			}

		}else
		{
			// ajust the depth:
			// var values = mWindowStack.Values;
			// int j = values.Count-1;
			// int i = j-1;
			// for(; i >= 0 && j >= 0; i--,j--)
			// {
			// 	var a = values[i].GetDepth();
			// 	var b = values[j].GetDepth();
			// 	if()
			// 	{

			// 	}
			// }
		}
	}

	public void PushUIWindow(PUIWindow win)
	{
		AjustTopDepth();
		win.transform.parent = GetUIAnchor().transform;
		win.transform.localScale = Vector3.one;
		curDepth += depthAdded;
		win.SetDepth(curDepth );
		var top = GetTopWindow();
		if(top != null)
		{
			if(win.isHidePre)
			{
				top.Hide();
			}
			top.OnBackground();
		}
		//win.OnFrontground();
		//DisableTopTouch();
		mWindowStack.Add(-curDepth,win);

		// Vector3 localPos = win.transform.localPosition;
		// localPos.z = currentZ;
		// win.transform.localPosition =  new Vector3(0, 0, currentZ);
		win.SetZValue(currentZ);
		if (win.isWith3D)
		{
			IncCurrentZ();
		}
		Enable3DCamera(!win.isClose3DUI);
	}

	protected void AjustTopDepth()
	{
		if(mWindowStack.Count > 1)
		{
			var a = mWindowStack.Values[0];
			var b = mWindowStack.Values[1];
			int depth = a.GetDepth();
			int tmp = b.GetDepth();
			if((depth - tmp) > depthAdded)
			{
				
				depth = tmp + depthAdded;
				mWindowStack.Values[0].SetDepth(depth);
				Debug.Log(depth + "--- tmp" + depth);
				mWindowStack.RemoveAt(0);
				mWindowStack.Add(-depth, a);
				curDepth = depth;
				EventNotificationCenter.GetInstance().Broadcast(BrowdCastEvent.AJUST_TOP_DEPTH);
			}
		}
	
	}

	protected void DisableTopTouch()
	{
		PUIWindow topWin = GetTopWindow();
		if(topWin != null)
		{
			topWin.DisableTouch();
		}
	}

	public float GetCurrentZ()
	{
		return currentZ;
	}
	protected void SetCurrentZ(float zValue)
	{
		currentZ = zValue;
	}

	protected void IncCurrentZ()
	{
		currentZ += incZInterval;
	}

	public void Enable3DCamera(bool flag)
	{
		if(camera3D != null)
		{
			camera3D.enabled = flag;
		}
	}

	public bool ShouldProcessTouch(Vector2 position )  
	{  
	    Ray ray = camera2D.ScreenPointToRay( position );  
	    bool touchUI = Physics.Raycast( ray, float.PositiveInfinity);  
	    return !touchUI;  
	}  
}
