using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToPlay : MonoBehaviour {

	void Start () {
		UIEventListener.Get(gameObject).onClick = OnClickItem;
	}

	protected void OnClickItem(GameObject go)
	{
		PartOneController.Create();
	}
	
}
