using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class log : MonoBehaviour
{
	[SerializeField] Animator anim;

	public void Hit(){
		anim.SetTrigger ("hit");
		Debug.Log ("Hit");
	}
}
