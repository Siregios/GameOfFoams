using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour {

    Animator test;

	// Use this for initialization
	void Start () {
        test = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            test.SetBool("Run", true);
        }
        else
        {
            test.SetBool("Run", false);
        }
	}
}
