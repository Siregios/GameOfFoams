using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBoss : BossBase {

    public float swordSwingDistance = 4.0f;
    public int numChargesPerPhase = 3;
    public LayerMask chargeStopLayer;   // layers if raycast hit will stop charge early
    public Animator swordController;
    public MeleeWeaponTrail chargeTrail;
    public ParticleSystem eyeParticlesLeft;
    public ParticleSystem eyeParticlesRight;
    GameObject eyeLeft;
    GameObject eyeRight;

    Coroutine focusRoutine = null;
    float swingCooldown = 0.0f;
    float idleTime = 0.0f;
    float nextChargeTime = 0.0f;

    State state = State.IDLE;
    enum State {
        ATTACKING,
        CHARGING,
        IDLE,
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();

        nextChargeTime = Time.time + Random.Range(1.0f, 2.0f);
        //nextChargeTime = Time.time + Random.Range(10.0f, 20.0f);
        eyeLeft = eyeParticlesLeft.transform.GetChild(0).gameObject;
        eyeRight = eyeParticlesRight.transform.GetChild(0).gameObject;
    }

    void SetEyes(bool enabled) {
        if (enabled) {
            eyeParticlesLeft.Play();
            eyeParticlesRight.Play();
        } else {
            eyeParticlesLeft.Stop();
            eyeParticlesRight.Stop();
        }
        eyeLeft.SetActive(enabled);
        eyeRight.SetActive(enabled);
    }

    // Update is called once per frame
    void Update() {

        if (state != State.CHARGING) {
            if (Time.time > nextChargeTime) {
                StartCoroutine(ChargeRoutine());
                state = State.CHARGING;
            }
        }

        if (state == State.IDLE) {
            idleTime += Time.deltaTime;
            if (idleTime > 1.0f) {
                state = State.ATTACKING;
                idleTime = 0.0f;
            }
        }

        if (state == State.ATTACKING) {
            agent.destination = player.position;

            if (agent.remainingDistance < agent.stoppingDistance) { // focus on player if close enough
                if (focusRoutine == null) {
                    focusRoutine = StartCoroutine(FocusRoutine(player, 100.0f, agent.angularSpeed));
                }
            } else if (focusRoutine != null) {  // else cancel the focusing routine if player is futher
                StopCoroutine(focusRoutine);
                focusRoutine = null;
            }

            swingCooldown -= Time.deltaTime;
            if (swingCooldown <= 0.0f) {
                float sqrDist = Vector3.SqrMagnitude(transform.position - player.position);
                if (sqrDist < swordSwingDistance * swordSwingDistance) {
                    // try to attack
                    swordController.SetTrigger("Attack");
                    swingCooldown = 2.0f;
                }
            }

        }

    }

    RaycastHit hit;
    IEnumerator ChargeRoutine() {
        agent.enabled = false;
        yield return Yielders.Get(1.0f);

        SetEyes(true);
        float origSpeed = agent.speed;
        float origAccel = agent.acceleration;
        agent.speed *= 8.0f;
        agent.acceleration = 1000.0f;

        for (int i = 0; i < numChargesPerPhase; ++i) {
            agent.enabled = false;

            // wait and stare at player for random amount of time
            float stareTime = Random.Range(1.0f, 5.0f);
            yield return FocusRoutine(player, stareTime, agent.angularSpeed);

            // charge towards player and a bit past, with random left / right deviation
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, dir);
            Vector3 newDest = player.position + dir * 4.0f + right * Random.Range(-4.0f, 4.0f);

            // raycast check to see if hit something along path so dont try to path around it
            float distance = Vector3.Distance(transform.position, newDest);
            Vector3 start = eyeParticlesLeft.transform.position;
            //Debug.DrawRay(start, dir * distance, Color.green, 5.0f);
            if (Physics.Raycast(start, dir, out hit, distance, chargeStopLayer)) {
                newDest = hit.point;
                newDest.y = transform.position.y;
            }

            // charge towards player 
            agent.enabled = true;
            agent.destination = newDest;
            chargeTrail.Emit = true;

            // wait till boss is close enough to charge point
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance) {
                yield return null;
            }
            chargeTrail.Emit = false;
        }

        SetEyes(false);
        agent.velocity = Vector3.zero;
        agent.speed = origSpeed;
        agent.acceleration = origAccel;
        nextChargeTime = Time.time + Random.Range(10.0f, 20.0f);

        state = State.IDLE;
    }

    void OnCollisionEnter(Collision col) {
        foreach (ContactPoint cp in col.contacts) {
            // if either of the contact colliders is with the bosses sword
            // or if charging and either are with the boss directly then dmg player
            if (cp.thisCollider.CompareTag(Tags.bossSword) || cp.otherCollider.CompareTag(Tags.bossSword) ||
                (state == State.CHARGING && (cp.thisCollider.CompareTag(Tags.boss) || cp.otherCollider.CompareTag(Tags.boss)))) {
                if (col.collider.transform.root.CompareTag(Tags.player)) {
                    Health h = col.collider.transform.root.GetComponent<Health>();
                    h.Damage(10.0f);
                    return;
                }
            }
        }
    }
}
