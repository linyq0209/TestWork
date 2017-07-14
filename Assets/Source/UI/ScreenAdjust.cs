using UnityEngine;
using System.Collections.Generic;

public class ScreenAdjust : MonoBehaviour 
{

	public Vector2 specialSize1 = new Vector2(1024,768);

	public float stdHeight = 640f;

	public float maxError = 0.1f;

	protected List<Vector2> specialSizes_ = new List<Vector2>();

	public float scale_;

	public float curWidth_;

	public float curHeight_;

	protected float stdUseHeight_;

	void  Start ()
	{
		specialSizes_.Add(specialSize1);
		stdUseHeight_ = stdHeight;

		float aspectRatio = Screen.width*1.0f/Screen.height*1.0f;
		foreach(Vector2 specialSize in specialSizes_)
		{
			float specialRatio = specialSize.x/specialSize.y;
			float offset = Mathf.Abs(aspectRatio-specialRatio);
	
			if (offset < maxError)
			{
				AdjustScale(Screen.height*1.0f/specialSize.y);
				stdUseHeight_ = specialSize.y;
				return;
			}
		}

		//use standard scale
		AdjustScale(Screen.height*1.0f/stdHeight);

		
	}

	private void  AdjustScale ( float scale  )
	{
		scale_ = scale;
		transform.localScale = new Vector3(scale,scale,scale);
	}

	public float GetCurStdHeight ()
	{
		return stdUseHeight_;
	}

	public float  GetScale ()
	{
		return scale_;
	}
}
