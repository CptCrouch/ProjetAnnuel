using UnityEngine;
using System.Collections;

public class MoveOnPlane : MonoBehaviour {

    public float speedNormal = 5f;
    private float speed;
    public float speedRun = 10;
    public KeyCode activeGravity = KeyCode.LeftShift;
    private CharacterController characController;
	// Use this for initialization
	void Start () {
        characController = GetComponent<CharacterController>();
        speed = speedNormal;
	}
	
	// Update is called once per frame
	void Update () {
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        float axisY = Input.GetAxis("UpDown");
        if (transform.position.y > 0.5)
            transform.Translate(new Vector3(axisX, axisY, axisZ) * speed * Time.deltaTime);
        else
            transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);

        if(Input.GetKeyDown(activeGravity))
        {
            if(characController.enabled == false)
                characController.enabled = true;
            else
                characController.enabled = false;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            
            speed = speedRun;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {

            speed = speedNormal;
        }



    }
}
