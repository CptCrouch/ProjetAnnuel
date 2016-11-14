using UnityEngine;
using System.Collections;

public class ToolUser : MonoBehaviour {

	public Material capMaterial;
    public bool leftStaying;
    public bool imMiddle;
    public float precisionDetectionObject = 0.1f;

    public float longueurUI =100f;
    public float largeurUI=5f;

	// Use this for initialization
	void Start () {
       

    }
	
	void Update(){

		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
            LayerMask layerMask = 1 << 9;

			if(Physics.Raycast(transform.position, transform.forward, out hit,layerMask)){

				GameObject victim = hit.collider.gameObject;

				GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);

                //GameObject [] pieces2 = BLINDED_AM_ME.MeshCut.Cut(pieces[1], transform.position, transform.right, capMaterial);
                /*if (leftStaying == true)
                {
                    if (!pieces[1].GetComponent<Rigidbody>())
                        pieces[1].AddComponent<Rigidbody>();

                    Destroy(pieces[1], 1);
                }
                else
                {
                    if (!pieces[0].GetComponent<Rigidbody>())
                        pieces[0].AddComponent<Rigidbody>();

                    Destroy(pieces[0], 1);
                }*/
                if(imMiddle == true)
                {
                    if (!pieces[1].GetComponent<Rigidbody>())
                    {
                        pieces[1].AddComponent<Rigidbody>();
                        pieces[1].AddComponent<BoxCollider>();
                    }

                    pieces[1].GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);

                    pieces[1].GetComponent<MeshRenderer>().materials[1].color = Color.red;
                    //Destroy(pieces[1]);

                   
                }
                /*CombineInstance[] combineMesh = new CombineInstance[pieces.Length - 1];
                MeshFilter[] meshFilters = new MeshFilter[pieces.Length - 1];
                for (int i = 0; i < meshFilters.Length; i++)
                {
                    if (pieces[i] != pieces[1])
                    {
                        meshFilters[i] = pieces[i].GetComponent<MeshFilter>();
                        combineMesh[i].mesh = meshFilters[i].sharedMesh;
                        combineMesh[i].transform = meshFilters[i].transform.localToWorldMatrix;
                        meshFilters[i].gameObject.SetActive(false);
                    }
                }
                victim.transform.GetComponent<MeshFilter>().mesh = new Mesh();
                victim.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combineMesh);
                victim.transform.gameObject.SetActive(true);*/


            }

		}
	}

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward *longueurUI);
		Gizmos.DrawLine(transform.position + transform.up * largeurUI, transform.position + transform.up * largeurUI + transform.forward *longueurUI);
		Gizmos.DrawLine(transform.position + -transform.up * largeurUI, transform.position + -transform.up * largeurUI + transform.forward * longueurUI);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * largeurUI);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * largeurUI);

	}

}
