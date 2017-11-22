using UnityEngine;
using System.Collections;

public class Sprint : MonoBehaviour {

    [SerializeField]
public float speedMultiplier = 1;
   public float Duration = 100;
    protected float MaxDuration= 100;
    protected float DurationRate = 0.1f;
    protected float RecoveryRate = 0.01f;
    
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
        if (Input.GetButton("Sprint") && Duration > 0)
        {
           
            movement.speedMultiplier = speedMultiplier;
            Duration -= DurationRate;
            if (Duration <= 0)
            {
                movement.speedMultiplier = 1f;
                //Again, may need a bit of playing around
            }
        }

       
        else
        {
            movement.speedMultiplier = 1f;
            Duration += RecoveryRate;

           
        }
        if (Duration >= MaxDuration)
        {
            Duration = MaxDuration;
        }
    }
}

//Edited on 8/14/2017 by Ian ring to add samina bar logic
