using UnityEngine;
using System.Collections;

public class cameraDamping : MonoBehaviour {

	public float boundMax_X;
	public float boundMin_X;

	public float boundMax_Z;
	public float boundMin_Z;

	public Camera cam ;

	public float camSpeed ;

	void Start () 
	{
		camSpeed = cam.pixelWidth * 2 ;
	}

	void Update () 
	{  
		

		Vector3 dir = Input.mousePosition - new Vector3 (cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
		transform.localPosition += new Vector3 (dir.x ,0,dir.y) / camSpeed ;

		if (transform.localPosition.x > boundMax_X )
			transform.localPosition = new Vector3 (boundMax_X, transform.localPosition.y, transform.localPosition.z);
		if (transform.localPosition.x < boundMin_X )
			transform.localPosition = new Vector3 (boundMin_X, transform.localPosition.y, transform.localPosition.z);
		if (transform.localPosition.z > boundMax_Z )
			transform.localPosition = new Vector3 (transform.localPosition.x , transform.localPosition.y, boundMax_Z);
		if (transform.localPosition.z < boundMin_Z )
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, boundMin_Z);
	}
}
