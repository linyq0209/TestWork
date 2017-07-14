using UnityEngine;

public abstract class PMonoSingleton<T> : MonoBehaviour where T : PMonoSingleton<T> 
{
	protected static T sInstance ;

	protected bool mIsNotDestroyOnLoad = true;

	public static T GetInstance(bool isAutoCreate = true)
	{
		if(sInstance == null && isAutoCreate)
		{
			sInstance = FindObjectOfType<T>();
			if(FindObjectsOfType<T>().Length > 1)
			{
				// Debug.Log();
			}

			if(sInstance == null)
			{
				string name = typeof(T).Name;
				GameObject obj = GameObject.Find(name);
				if(obj == null)
				{
					obj = new GameObject(name);
					sInstance = obj.AddComponent<T>();
				}
				sInstance = obj.GetComponent<T>();
			}
		}
		return sInstance;
	}

	protected virtual void Awake()
	{
		if(sInstance == null)
		{
			sInstance = gameObject.GetComponent<T>();
			Init();
		}
		else if(sInstance == this)
		{

		}
		if(mIsNotDestroyOnLoad)
		{
			DontDestroyOnLoad(gameObject);
		}
		
	}

	protected virtual void OnDestroy()
	{
		sInstance = null;
	}

	protected virtual void Init()
	{
	}
}