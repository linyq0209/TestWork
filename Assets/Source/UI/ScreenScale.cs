using UnityEngine;

public class ScreenScale : MonoBehaviour {

	protected float mScale;
	public float stdAspectRatio = 9.0f/16.0f;


	void Start () 
	{
		Resize();
	}
	

	public void Resize()
	{
		float aspectRatio = Screen.width*1.0f/Screen.height*1.0f;
		mScale = aspectRatio*stdAspectRatio;
		transform.localScale = new Vector3(mScale,mScale,mScale);
	}

}
