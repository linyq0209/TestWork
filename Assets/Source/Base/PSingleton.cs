
public abstract class PSingleton<T> where T : PSingleton<T>, new ()
{
	protected static T sInstance ;

	public static T GetInstance(bool isAutoCreate = true)
	{
		if(isAutoCreate && sInstance == null)
		{
			sInstance = new T();
			sInstance.Init();
		}
		return sInstance;
	}

	public static void Release()
	{
		if(sInstance != null)
		{
			sInstance.ReleaseImple();
			sInstance = null;
		}
	}

	protected virtual void Init()
	{

	}


	protected virtual void ReleaseImple()
	{

	}
} 