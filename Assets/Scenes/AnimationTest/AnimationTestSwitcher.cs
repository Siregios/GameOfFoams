using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestSwitcher : MonoBehaviour {

    public GameObject[] objects;
    int activeObj = 0;
    Camera activeCam;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < objects.Length; ++i) {
            objects[i].SetActive(i == 0);
            objects[i].transform.GetChild(0).gameObject.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            objects[activeObj].transform.GetChild(0).gameObject.SetActive(false);
            objects[activeObj++].SetActive(false);
            if (activeObj >= objects.Length) {
                activeObj = 0;
            }
            objects[activeObj].transform.GetChild(0).gameObject.SetActive(true);
            objects[activeObj].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
	}
}
