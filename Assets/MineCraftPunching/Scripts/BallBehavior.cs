using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {
    WorldGenerate worldGenerate;
    PunchNotRandom punchNotRandom;
    Rigidbody rb;

    [SerializeField]
    public float puissanceMinimumArea1 = 4f;
    [SerializeField]
    public float puissanceMinimumArea2 = 5f;
    [SerializeField]
    public float puissanceMinimumArea3 = 7f;
    [SerializeField]
    public float puissanceMinimumArea4 = 10f;

    // Use this for initialization
    void Start () {
        punchNotRandom = FindObjectOfType<PunchNotRandom>();
        worldGenerate = FindObjectOfType<WorldGenerate>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShootOnBall(Vector3 direction, float strength)
    {
        rb.AddForce(direction * strength, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("pouet");
        if (collision.collider.transform.tag == "Cell")
        {
            Cell cellCollided = collision.collider.GetComponent<Cell>();
            float puissanceCollision = collision.relativeVelocity.magnitude;
            Debug.Log(puissanceCollision);
            if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
            {
                StartCoroutine(cellCollided.ReturnToStartScale(punchNotRandom.speed));
                if (puissanceCollision >= puissanceMinimumArea1 && puissanceCollision < puissanceMinimumArea2)
                    GetAreaOfCell(1, cellCollided.transform);
                if (puissanceCollision >= puissanceMinimumArea2 && puissanceCollision < puissanceMinimumArea3)
                    GetAreaOfCell(2, cellCollided.transform);
                if (puissanceCollision >= puissanceMinimumArea3 && puissanceCollision < puissanceMinimumArea4)
                    GetAreaOfCell(3, cellCollided.transform);
                if (puissanceCollision >= puissanceMinimumArea4)
                    GetAreaOfCell(4, cellCollided.transform);

            }
            

        }
    }

    void GetAreaOfCell(int areaForce, Transform center)
    {
        for (int h = 1; h <= areaForce; h++)
        {
            // pour chaque cube présent dans la scène
            for (int i = 0; i < punchNotRandom.worldGenerateObject.transform.childCount; i++)
            {
                
                float distanceFromCenter = Mathf.Abs(punchNotRandom.worldGenerateObject.transform.GetChild(i).position.x - center.position.x) + Mathf.Abs(punchNotRandom.worldGenerateObject.transform.GetChild(i).position.z - center.position.z);
                
                if (distanceFromCenter == h)
                {
                    if (punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>()._imMoving == false && punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().imAtStartPos == false)
                        StartCoroutine(punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().ReturnToStartScale(punchNotRandom.speed));
                }
            }
        }
    }
}
