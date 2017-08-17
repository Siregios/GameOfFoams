using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBoss : BossBase {

    public Animator swordController;

    // Use this for initialization
    protected override void Start() {
        base.Start();

    }

    Coroutine focusRoutine = null;
    float swingCooldown = 0.0f;

    // Update is called once per frame
    void Update() {
        agent.destination = player.position;

        if (agent.remainingDistance < agent.stoppingDistance) { // focus on player if close enough
            if (focusRoutine == null) {
                focusRoutine = StartCoroutine(FocusRoutine(player, 100.0f, agent.angularSpeed));
            }
        } else if (focusRoutine != null) {
            StopCoroutine(focusRoutine);
            focusRoutine = null;
        }

        swingCooldown -= Time.deltaTime;
        if (swingCooldown <= 0.0f) {
            float sqrDist = Vector3.SqrMagnitude(transform.position - player.position);
            float minDist = 5.0f;
            if (sqrDist < minDist * minDist) {
                // try to attack
                swordController.SetTrigger("Attack");
                swingCooldown = 2.0f;
            }
        }


    }
}
