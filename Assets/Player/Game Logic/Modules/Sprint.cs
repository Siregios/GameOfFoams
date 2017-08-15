using UnityEngine;
using System.Collections;

public class Sprint : MonoBehaviour {

    [SerializeField]
    protected float speedMultiplier = 2;
    protected float Duration = 100;
    protected float MaxDuration= 100;
    protected float DurationRate = 0.1f;
    protected float RecoveryRate = 1;
    
    //May need to playaround with the duration time in final build. 
    InputToMovement movement;
    IInput input;

    void Start()
    {
        input = GetComponentInParent<IInput>();
        movement = GetComponentInParent<InputToMovement>();
    }

    void Update()
    {
        if (input.sprinting && Duration > 0)
        {
            movement.speedMultiplier = speedMultiplier;
            Duration -= DurationRate;
        }
        if(Duration <= 0)
        {
            movement.speedMultiplier = 1f;
            Duration += RecoveryRate;
            //Again, may need a bit of playing around
        }    
        else
        {
            movement.speedMultiplier = 1f;
        }
    }
}

//Edited on 8/14/2017 by Ian ring to add samina bar logic
