using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// base class for bosses with some helper functions
public class BossBase : MonoBehaviour {

    // prob reference to camera later
    protected AudioSource source;
    protected NavMeshAgent agent;
    protected int playerLayer;

    protected Transform player;
    //int playerLayer;

    protected virtual void Start() {
        //playerLayer = 1 << LayerMask.NameToLayer(Tags.Layers.Player);

        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO) {
            player = playerGO.transform;
        } else {
            Debug.LogWarning("No player found in boss script PROB GONNA BE ERRORS");
        }

        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

    }


    // focus on a transforms position for an amount of time
    // turns at a rate degreesPerSec (not sure how well this works)
    protected IEnumerator FocusRoutine(Transform target, float time, float degreesPerSec = 360.0f) {
        while (time > 0.0f) {
            Vector3 dir = (target.position - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, degreesPerSec * Time.deltaTime);
            yield return 0;
        }
        //dir = (target.position - transform.position).normalized;
        //transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.z));
    }

    // look at a position at a rate of degreesPerSec
    // ends once you are looking at it
    protected IEnumerator LookAtRoutine(Vector3 target, float degreesPerSec = 360.0f) {
        Quaternion startRot = transform.rotation;
        Vector3 dir = (target - transform.position).normalized;
        float angle = Vector3.Angle(dir, transform.forward);
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.z));
        float t = 0.0f;
        while (t < 1.0f) {
            transform.rotation = Quaternion.Slerp(startRot, lookRot, t);
            float r = (angle < 1.0f) ? degreesPerSec : degreesPerSec / angle;
            t += r * Time.deltaTime;
            yield return 0;
        }
        transform.rotation = lookRot;
    }


}
