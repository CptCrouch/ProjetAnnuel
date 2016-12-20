using UnityEngine;
using System.Collections;

public class Shaderscale : MonoBehaviour {

    Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        rend.material.SetVector("_Scale", new Vector4(transform.localScale.x,
                                        transform.localScale.y,
                                        transform.localScale.z, 0f));
    }
}
