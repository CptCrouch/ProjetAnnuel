using UnityEngine;
using System.Collections;

public class Scale : MonoBehaviour {
    public float speed =10f;
	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(1, 100f, 1);

	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale -= new Vector3(0,speed * Time.deltaTime, 0);
        transform.localPosition += Vector3.down * speed / 2 * Time.deltaTime;
    }
}
