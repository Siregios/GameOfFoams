using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBoss : BossBase {

    // Use this for initialization
    protected override void Start() {
        base.Start();

    }

    Coroutine focusRoutine = null;

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


    }
}
