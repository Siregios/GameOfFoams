using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MovementEffects;

// base class for bosses with some helper functions
public class BossBase : MonoBehaviour {

    // prob reference to camera later
    protected AudioSource source;
    protected NavMeshAgent agent;
    protected int playerLayer;

    // prob reference to player script
    //int playerLayer;

    protected virtual void Start() {
        //playerLayer = 1 << LayerMask.NameToLayer(Tags.Layers.Player);

        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        
    }


    // focus on a transforms position for an amount of time
    // turns at a rate degreesPerSec (not sure how well this works)
    protected IEnumerator<float> FocusRoutine(Transform target, float time, float degreesPerSec = 360.0f) {
        Quaternion startRot = transform.rotation;
        float t = 0.0f;
        Vector3 dir;
        while (time > 0.0f) {
            dir = (target.position - transform.position).normalized;
            float angle = Vector3.Angle(dir, transform.forward);
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.z));
            transform.rotation = Quaternion.Slerp(startRot, lookRot, t);
            float r = (angle < 1.0f) ? degreesPerSec : degreesPerSec / angle;
            t += r * Time.deltaTime;
            time -= Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        dir = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.z));
    }

    // look at a position at a rate of degreesPerSec
    // ends once you are looking at it
    protected IEnumerator<float> LookAtRoutine(Vector3 target, float degreesPerSec = 360.0f) {
        Quaternion startRot = transform.rotation;
        Vector3 dir = (target - transform.position).normalized;
        float angle = Vector3.Angle(dir, transform.forward);
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.z));
        float t = 0.0f;
        while (t < 1.0f) {
            transform.rotation = Quaternion.Slerp(startRot, lookRot, t);
            float r = (angle < 1.0f) ? degreesPerSec : degreesPerSec / angle;
            t += r * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        transform.rotation = lookRot;
    }


}
