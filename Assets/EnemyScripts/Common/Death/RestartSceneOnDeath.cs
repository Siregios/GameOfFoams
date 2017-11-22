using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSceneOnDeath : MonoBehaviour, IDeathAction {

	// Use this for initialization
	void Start () {
		
	}

    public void Die() {
        // todo: add canvas with black background and lerping and fading and timeslow and stuff
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
