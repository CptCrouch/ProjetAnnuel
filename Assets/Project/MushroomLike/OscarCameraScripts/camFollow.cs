using UnityEngine;
using System.Collections;

public class camFollow : MonoBehaviour {

	public Transform player ;

	public float dist ;
	public float rangeMax ;

	public float speed;

	void Start () 
	{
	
	}

	void Update () 
	{
		dist = Vector3.Distance (transform.position, player.position) ;
		float step = speed * Time.deltaTime;

		if (dist > rangeMax)
			transform.position = Vector3.MoveTowards(transform.position, player.position, step);
			
	
	}
}
