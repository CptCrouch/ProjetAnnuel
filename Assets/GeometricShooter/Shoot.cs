using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
    public float angle = 45;
    //public ProjectilBehavior[] projectile;
    public float speedMoving = 0.5f;
    public GameObject prefabProjectile;
    public float duréeShoot=2f;

    private ProjectilBehavior currentProjectile;
    private Vector3 positionShooting;
    private bool canShoot =true;
	// Use this for initialization
	void Start () {
        currentProjectile = FindObjectOfType<ProjectilBehavior>();
        positionShooting = currentProjectile.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // Vector3 position = projectile[0].transform.position - transform.position;
        // Debug.Log(position.x+" " + position.y + " "+ position.z + " ");

        float axis = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(axis*speedMoving, 0, 0));


	if(Input.GetMouseButtonDown(0))
        {
            if (canShoot == true)
            {
                currentProjectile.Shoot(Vector3.forward);
                StartCoroutine(waitToStopMoving());
            }
        }
	}
    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, new Vector3(angle/90, 0, 1));
        Debug.DrawRay(transform.position, new Vector3(-angle/90, 0, 1));
    }
    IEnumerator waitToStopMoving()
    {
        canShoot = false;
        yield return new WaitForSeconds(duréeShoot);
        currentProjectile.GetComponent<Rigidbody>().isKinematic = true;
        GameObject newProjectile = Instantiate(prefabProjectile) as GameObject;
        newProjectile.transform.SetParent(transform);
        newProjectile.transform.localPosition = new Vector3(0,0,2);
        currentProjectile = newProjectile.GetComponent<ProjectilBehavior>();
        canShoot = true;
    }

}
