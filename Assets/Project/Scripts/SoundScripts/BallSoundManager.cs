using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSoundManager : MonoBehaviour {

    public string rolling = "event:/char_rolling";
    public FMOD.Studio.EventInstance ballRolling;

    FMOD.Studio.ParameterInstance volumeBallRolling;
    FMOD.Studio.ParameterInstance speedBallRolling;

   

    //public FMODUnity.StudioEventEmitter myBallEvent;


    BallBehavior ballBehavior;
    // Use this for initialization
    void Start () {
        ballBehavior = FindObjectOfType<BallBehavior>();

        ballRolling = FMODUnity.RuntimeManager.CreateInstance(rolling);
        ballRolling.getParameter("Volume", out volumeBallRolling);
        ballRolling.getParameter("Speed", out speedBallRolling);
        speedBallRolling.setValue(0.5f);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ballRolling, transform, ballBehavior.rb);
        ballRolling.start();

        
        
    }
	
	// Update is called once per frame
	void Update () {
		
        if(ballBehavior.imGrounded == true)
        {
            if (ballBehavior.rb.velocity.magnitude > 1)
                volumeBallRolling.setValue(1);
            else
                volumeBallRolling.setValue(0);
        }
        else
        {
            volumeBallRolling.setValue(0);
        }
        
        
       

	}
}
