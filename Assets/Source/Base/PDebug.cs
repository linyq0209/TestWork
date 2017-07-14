using UnityEngine;

public class PDebug
{
	public static bool isDebug = true;

	public static void Log(string content)
	{
		if(isDebug)
		{
			Debug.Log(content);
		}
		
	}
	public static void Break()
	{
		if(isDebug)
		{
			Debug.Break();
		}
	}
}