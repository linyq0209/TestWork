using System;
using System.Collections;
using UnityEngine;

public class PUIBaseWidget :  MonoBehaviour
{
	public const int ENTER_ACTION = 0;
	public const int SHOW_ACTION = 1;
	public const int HIDE_ACTION = 2;
	public const int EXIT_ACTION = 3;	
	public const int POST_LOAD_ACTION = 4;
	public const int ACTION_COUNT = 5;


	public const int POST_ENTER = 0x01;
	public const int POST_LOAD = 0x02;



	public struct PUIWidgetId
	{
		public int id ;
		public string name;
		public string widgetName;
	}

	public class PUIWidgetData
	{
		public bool isInvaild = false;
		public object data;
	}

	public UITweener openAnim;
	public UITweener closeAnim;

	protected Action[] callbacks = new Action[ACTION_COUNT];

	protected PUIWidgetId mId;
	protected PUIWidgetData mWidgetData;
	protected bool mHasEnter = false;
	protected bool mHasExit = false;
	protected bool mIsLoadOnStart = true;
	protected bool mHasLoad = false;
	protected bool mIsLoadImmediately = true;
	protected IEnumerator mLoadImpl = null;
	protected Action mLoadImmediately = null;
	protected bool mHasInit = false;
	protected float delayDestroy = -1f;

	protected int winState = 0;

	public  void Init()
	{
		if(!mHasInit)
		{
			mHasInit = true;
			InitImpl();
		}
	}
	protected virtual void InitImpl()
	{

	}

	public void SetPanelAnimation(UITweener open,UITweener close)
	{
		openAnim = open;
		closeAnim = close;
		if(openAnim != null)
		{
			openAnim.AddOnFinished(OnEnter);
		}
		if(closeAnim != null)
		{
			closeAnim.AddOnFinished(OnExit);
		}
	}

	protected void Awake()
	{
		if(openAnim != null)
		{
			openAnim.AddOnFinished(OnEnter);
		}
		if(closeAnim != null)
		{
			closeAnim.AddOnFinished(OnExit);
		}
		Init();
	}

	protected void Start()
	{
		if(!mHasEnter)
		{
			Enter();
		}
		if(mIsLoadOnStart)
		{
			Load();
		}
		//PDebug.Log(Time.realtimeSinceStartup + "____ create" + name);
	}

	protected virtual void OnDestroy()
	{
		if(!mHasExit)
		{
			if(callbacks[EXIT_ACTION] != null)
			{
				callbacks[EXIT_ACTION]();
			}
		}
		//Debug.Log(Time.realtimeSinceStartup + "____destory" + name);
	}

	public void AddAction(Action callback, int actionType)
	{
		if(actionType >= 0 && actionType < ACTION_COUNT)
		{
			callbacks[actionType] += callback;
		}
	}

	public void RemoveAction(Action callback, int actionType)
	{
		if(actionType >= 0 && actionType < ACTION_COUNT)
		{
			callbacks[actionType] -= callback;
		}
	}

	protected void PlayTweenGroup(UITweener tweener,bool forward = true)
	{
		if(tweener != null)
		{
			tweener.Play(forward);
			int groupId = tweener.tweenGroup;
			if(groupId != 0)
			{

				UITweener[] coms = tweener.gameObject.GetComponents<UITweener>();
				if(coms.Length <= 1)
				{
					return;
				}
				foreach(var item in coms)
				{
					if(item != tweener && groupId == item.tweenGroup)
					{
						item.Play(forward);
					}
				}
			}
		}
	}


	// base actions:
	public virtual void Enter(bool ignoreAnim = false)
	{
		mHasEnter = true;
		if( !ignoreAnim && openAnim != null)
		{
			//openAnim.PlayForward();
			Debug.Log("-------open animation-----");
			PlayTweenGroup(openAnim);
		}
		else
		{
			OnEnter();
		}
	}

	protected virtual void OnEnter()
	{
		if(callbacks[ENTER_ACTION] != null)
		{
			callbacks[ENTER_ACTION]();
		}
		winState |=  1 << 0;
	}

	public virtual void Show()
	{
		OnShow();
	}

	protected virtual void OnShow()
	{
		gameObject.SetActive(true);
		if(callbacks[SHOW_ACTION] != null)
		{
			callbacks[SHOW_ACTION]();
		}
	}

	public virtual void Hide()
	{
		OnHide();
	}

	protected virtual void OnHide()
	{
		gameObject.SetActive(false);
		if(callbacks[HIDE_ACTION] != null)
		{
			callbacks[HIDE_ACTION]();
		}
	}

	public virtual void Exit(bool ignoreAnim = false)
	{
		if(mHasExit)
		{
			return;
		}
		mHasExit = true;
		if(!ignoreAnim && closeAnim != null)
		{
			//closeAnim.PlayForward();
			PlayTweenGroup(closeAnim);
		}
		else
		{
			OnExit();
		}
	}

	protected virtual void OnExit()
	{
		DestoryImpl();
		if(callbacks[EXIT_ACTION] != null)
		{
			callbacks[EXIT_ACTION]();
		}
	}


	public void SetWinState(int state)
	{
		winState = state;
	}

	public int GetWinState()
	{
		return winState;
	}

	public void SetLoadHanlder(IEnumerator coroutine)
	{
		mLoadImpl = coroutine;
	}

	public void SetLoadHanlder(Action callback)
	{
		mLoadImmediately = callback;
	}

	public void SetIsLoadImmediately(bool flag)
	{
		mIsLoadImmediately = false;
	}

	public void SetIsLoadOnStart(bool flag)
	{
		mIsLoadOnStart = false;
	}

	public virtual void Load()
	{
		if(!mHasLoad)
		{
			mHasLoad = true;
			if(mIsLoadImmediately)
			{
				LoadImmediately();
				OnPostLoad();
			}
			else
			{
				StartCoroutine(LoadInCoroutine());
			}
		}
	}

	protected virtual IEnumerator LoadInCoroutine()
	{
		if(mLoadImpl != null)
		{
			yield return StartCoroutine(mLoadImpl);
		}
		OnPostLoad();
	}

	protected virtual void LoadImmediately()
	{
		if(mLoadImmediately != null)
		{
			mLoadImmediately();
		}
	}
	
	protected virtual  void OnPostLoad()
	{
		SetWidgetId();
		if(callbacks[POST_LOAD_ACTION] != null)
		{
			callbacks[POST_LOAD_ACTION]();
		}
		winState |= 1 << 1;
	}
	
	public void SetDelayDestroy(float t)
	{
		this.delayDestroy = t;
	}

	public void SetData(PUIWidgetData data)
	{
		mWidgetData = data;
	}

	public PUIWidgetData GetData(bool isCreate = false)
	{
		if(isCreate && mWidgetData == null)
		{
			mWidgetData = new PUIWidgetData();
		}
		return mWidgetData;
	}

	public void SetWigetName(string widgetName)
	{
		mId.widgetName = widgetName;
	}

	protected void SetWidgetId()
	{
		mId.id = gameObject.GetInstanceID();
		mId.name = gameObject.name;
	}

	protected void DestoryImpl()
	{
		transform.parent = null;
		transform.localScale = new Vector3(0,0,0);
		if(delayDestroy > 0)
		{
			GameObject.Destroy(gameObject, delayDestroy);
		}
		else
		{
			GameObject.Destroy(gameObject);
		}

	}

}
 