using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collet : PUIWindow {
	public GameObject tweenBtn;
	public GameObject tweenObj;
	public GameObject settingBtn;
	public GameObject bagBtn;
	public GameObject chBtn;
	
	protected TweenScale tween;
	bool isOpen = false;

	void Start(){
		
		tween = tweenObj.GetComponent<TweenScale>();
		UIEventListener.Get(bagBtn).onClick = OnBag;
		UIEventListener.Get(tweenBtn).onClick = OnTween;
		UIEventListener.Get(settingBtn).onClick = OnSetting;    
		UIEventListener.Get(chBtn).onClick = OnCharater;	
          	
	}

	void Updata(){
		if(Input.GetButtonDown("Fire1"))
       {
       	Debug.Log("123456");
       	//OnExit();
       } 
	}

	protected void OnBag(GameObject go)
	{
		BagPanelController.Create();
	}

	protected void OnSetting(GameObject go)
	{
		SetUpController.Create();
	}  

    protected void OnCharater(GameObject go)
    {
    	CharaterPanelController.Create();
    }

	protected void OnTween(GameObject go)
	{
		if(isOpen)
		{
			tween.PlayForward();
		}  
		else
		{
			tween.PlayReverse();
		}
		isOpen = !isOpen;
	}

	
    
    

}
