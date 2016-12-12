using UnityEngine;
using System.Collections;

public class WallDisapear : MonoBehaviour {

	void OnCollisionStay (Collision col)
    {
        col.collider.GetComponent<MeshRenderer>().enabled = false;
        Debug.Log("disabled");

    }
}
