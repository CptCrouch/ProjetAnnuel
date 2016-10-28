using UnityEngine;
using System.Collections;

public class ProjectilBehavior : MonoBehaviour {
    public float speed = 10f;
    public float speedRotato = 2f;
    public float forceShoot = 10f;


    GameObject player;
    bool canRotato = true;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Sphere");
	}
	
	// Update is called once per frame
	void Update () {
        //transform.RotateAround(player.transform.position,Vector3.up, speed * Time.deltaTime);
        if(canRotato ==true)
        transform.Rotate(Vector3.up, speedRotato);
	}
    public void Shoot(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * forceShoot, ForceMode.Impulse);
        canRotato = false;
        transform.parent = null;
       
    }
    
}
