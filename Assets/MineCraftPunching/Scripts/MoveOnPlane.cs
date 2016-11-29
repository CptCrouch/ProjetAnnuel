using UnityEngine;
using System.Collections;

public class MoveOnPlane : MonoBehaviour {

    public float speed = 10;
    public KeyCode activeGravity = KeyCode.A;
    private CharacterController characController;
	// Use this for initialization
	void Start () {
        characController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        float axisY = Input.GetAxis("UpDown");

        transform.Translate(new Vector3(axisX, axisY, axisZ)* speed *Time.deltaTime);

        if(Input.GetKeyDown(activeGravity))
        {
            if(characController.enabled == false)
                characController.enabled = true;
            else
                characController.enabled = false;
        }


        
        
	}
}
