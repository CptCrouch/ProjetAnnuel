using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public KeyCode up ;
	public KeyCode down ;
	public KeyCode left ;
	public KeyCode right ;

	public float speed;

	// Use this for initialization
	void Start () 
	{
		
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (up))
			transform.Translate (Vector3.forward * speed * Time.deltaTime);

		if (Input.GetKey (down))
			transform.Translate (Vector3.back * speed * Time.deltaTime);

		if (Input.GetKey (left))
			transform.Translate (Vector3.left * speed * Time.deltaTime);

		if (Input.GetKey (right))
			transform.Translate (Vector3.right * speed * Time.deltaTime);
	
	}
}
