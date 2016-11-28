using UnityEngine;
using System.Collections;

public class MoveOnPlane : MonoBehaviour {

    public float speed = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        float axisY = Input.GetAxis("UpDown");

        transform.Translate(new Vector3(axisX, axisY, axisZ)* speed *Time.deltaTime);
        
        
	}
}
