using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisingTrap : MonoBehaviour {

    public bool isup;
    public int timeforup;
    public int setuptime;
    public float rateforup = 1f;
    //Transform gate;
	// Use this for initialization
	void Start () {

        StartCoroutine("preup");
      //  StartCoroutine("up");

    }

    // Update is called once per frame
    void Update () {

       


    }
    IEnumerator preup()
    {
        yield return new WaitForSeconds(setuptime);
        StartCoroutine ("up");
    }
    IEnumerator up()
    {
        StartCoroutine("uptime");

        while (isup == true)
        {

            this.transform.Translate(0, rateforup , 0);
            yield return null;

        }
        while (isup == false)
        {

            this.transform.Translate(0, 0 , 0);
            yield return null;

        }
    }


    IEnumerator uptime()
    {
        yield return new WaitForSeconds(timeforup);

        isup = false;




    }

}
