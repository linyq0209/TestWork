using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float moveSpeed = 1.0f;
    
	void Updata()
	{
        //Debug.Log("Horizontal value:"+Input.GetAxis("Horizontal") );
        Debug.Log("0909");
		Vector3 direction = Input.GetAxis("Horizontal")*transform.right + 
		                    Input.GetAxis("Vertical")*transform.forward;

		transform.position = transform.position + moveSpeed*direction*Time.deltaTime;		
	}
}
