using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
	[SerializeField] Animator anim;
	private Rigidbody2D rb;
	public int speed;
    // Start is called before the first frame update
    void Start()
    {
		rb = this.GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown ("Fire1")) {
			anim.SetTrigger ("lungeT");
		} else if (Input.GetButtonDown ("Fire2")) {
			anim.SetTrigger ("stabT");
		} else if (Input.GetButtonDown ("Fire3")) {
			anim.SetTrigger ("throwT");
		} else if (Input.GetAxis ("Horizontal") > 0) {
			anim.SetTrigger ("walkForward");
		} else if (Input.GetAxis ("Horizontal") < 0) {
			anim.SetTrigger ("walkBackward");
		}
		else if (Input.GetAxis ("Horizontal") == 0) {
			anim.SetTrigger ("stopwalkingT");
		}
    }
	void OnTriggerEnter2D (Collider2D other){
		other.gameObject.SendMessage ("Hit");

	}
}
