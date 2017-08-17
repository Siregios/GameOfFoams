using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnCollisionEnter(Collision col) {
        if (col.collider.transform.root.CompareTag(Tags.player)) {
            Health h = col.collider.transform.root.GetComponent<Health>();
            h.Damage(10.0f);
        }
    }
}
