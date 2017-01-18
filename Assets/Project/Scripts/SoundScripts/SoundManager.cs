using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public FMOD.Studio.EventInstance iDestroy;
    FMOD.Studio.ParameterInstance volumeBallRolling;

    [FMODUnity.EventRef]
    public string destruction = "event:/SFX/destroyChain";


    //public FMODUnity.StudioEventEmitter ekez;


    public GameObject player;


    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        //iDestroy = 
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Sound
    public void EmittGrowSound()
    {
        Vector3 playerPos = player.transform.position;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/tileUp", playerPos);
    }

    public void EmittDestroySound(float parameter, CellTwo cellTwo)
    {
        Vector3 playerPos = player.transform.position;
        FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(destruction);
        FMOD.Studio.ParameterInstance parameterEvent;
        eventInstance.getParameter("chainValue", out parameterEvent);

        parameterEvent.setValue(parameter);
        eventInstance.start();
        


        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/destroySingle", playerPos,float);
    }
    #endregion
}
