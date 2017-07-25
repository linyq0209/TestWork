using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemController : MonoBehaviour {

	public static TestItemController Create(GameObject parent,GameObject child)
	{
        if(!child.GetComponent<UIEventListener>())
        
        {
        	child.AddComponent<UIEventListener>();
        }

		GameObject obj = NGUITools.AddChild(parent,child);
                        
		obj.SetActive(true);
		
		TestItemController ret = obj.GetComponent<TestItemController>();
		
		return ret;
		
	}


}
