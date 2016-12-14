using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {
    WorldGenerate worldGenerate;
    PunchNotRandom punchNotRandom;
    PunchHexagon punchHexagon;
    Rigidbody rb;

    [SerializeField]
    public float puissanceMinimale = 2f;
    [SerializeField]
    public float puissanceMinimumArea1 = 4f;
    [SerializeField]
    public float puissanceMinimumArea2 = 5f;
    [SerializeField]
    public float puissanceMinimumArea3 = 7f;
    [SerializeField]
    public float puissanceMinimumArea4 = 10f;

    [SerializeField]
    public float multiplicateurVelocityCollide = 100f;
    [SerializeField]
    public float velocityMax= 15f;
    // Use this for initialization
    void Start() {
        punchNotRandom = FindObjectOfType<PunchNotRandom>();
        if (punchNotRandom == null)
        {
            punchHexagon = FindObjectOfType<PunchHexagon>();
        }
        worldGenerate = FindObjectOfType<WorldGenerate>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

    }

    public IEnumerator ChangeMaterialHighlight()
    {

        GetComponent<MeshRenderer>().material.SetFloat("_Emission", 0.2f);
        yield return new WaitForEndOfFrame();
        GetComponent<MeshRenderer>().material.SetFloat("_Emission", 0f);
    }
    public void ShootOnBall(Vector3 direction, float strength, Vector3 playerPos)
    {
        
        rb.AddForce(direction * strength, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("pouet");
        if (collision.collider.transform.tag == "Cell")
        {
            Cell cellCollided;
            
            if (collision.collider.transform.childCount > 0)
            {
                 cellCollided = collision.collider.GetComponent<Cell>();
            }
            else
            {
                 cellCollided = collision.collider.transform.parent.GetComponent<Cell>();
            }
                float puissanceCollision = collision.relativeVelocity.magnitude;
            
                //Debug.Log(puissanceCollision);
                if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
                {
                    if (punchHexagon == null)
                        StartCoroutine(cellCollided.ReturnToStartScale(punchNotRandom.speedScaleCellUp));
                    else
                    {
                    if (puissanceCollision > puissanceMinimale)
                    {
                        StartCoroutine(cellCollided.ReturnToStartScale(punchHexagon.speedScaleCellUp));
                        if (rb.velocity.x < velocityMax && rb.velocity.y < velocityMax && rb.velocity.z < velocityMax)
                        {
                            Debug.Log("Avant  " + rb.velocity);
                            rb.velocity *= multiplicateurVelocityCollide*1.2f;
                            Debug.Log("Après  " + rb.velocity);
                        }
                    }
                    }

                if (puissanceCollision >= puissanceMinimumArea1 && puissanceCollision < puissanceMinimumArea2)
                {
                    GetAreaOfCell(1, cellCollided.transform);
                    if (rb.velocity.x < velocityMax && rb.velocity.y < velocityMax && rb.velocity.z < velocityMax)
                    {
                        Debug.Log("Avant  " + rb.velocity);
                        rb.velocity *= multiplicateurVelocityCollide*0.2f;
                        Debug.Log("Après  " + rb.velocity);
                    }

                }
                if (puissanceCollision >= puissanceMinimumArea2 && puissanceCollision < puissanceMinimumArea3)
                {
                    GetAreaOfCell(2, cellCollided.transform);
                    if (rb.velocity.x < velocityMax && rb.velocity.y < velocityMax && rb.velocity.z < velocityMax)
                    {
                        Debug.Log("Avant  " + rb.velocity);
                        rb.velocity *= multiplicateurVelocityCollide*0.1f;
                        Debug.Log("Après  " + rb.velocity);
                    }

                }
                if (puissanceCollision >= puissanceMinimumArea3 && puissanceCollision < puissanceMinimumArea4)
                {
                    GetAreaOfCell(3, cellCollided.transform);
                    if (rb.velocity.x < velocityMax && rb.velocity.y < velocityMax && rb.velocity.z < velocityMax)
                    {
                        Debug.Log("Avant  " + rb.velocity);
                        rb.velocity *= multiplicateurVelocityCollide * 0.1f;
                        Debug.Log("Après  " + rb.velocity);
                    }
                }
                if (puissanceCollision >= puissanceMinimumArea4)
                {
                    GetAreaOfCell(4, cellCollided.transform);
                    
                }

                //rb.velocity *= multiplicateurVelocityCollide;
            }
           }
          
    }
    

    void GetAreaOfCell(int areaForce, Transform center)
    {
        for (int h = 1; h <= areaForce; h++)
        {
            if (punchHexagon == null)
            {
                for (int i = 0; i < punchNotRandom.worldGenerateObject.transform.childCount; i++)
                {
                
                    float distanceFromCenter = Mathf.Abs(punchNotRandom.worldGenerateObject.transform.GetChild(i).position.x - center.position.x) + Mathf.Abs(punchNotRandom.worldGenerateObject.transform.GetChild(i).position.z - center.position.z);

                    if (distanceFromCenter == h)
                    {
                        if (punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>()._imMoving == false && punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().imAtStartPos == false)
                            StartCoroutine(punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().ReturnToStartScale(punchNotRandom.speedScaleCellUp));
                    }
                }
                
               
            }
            else {
                {
                    for (int i = 0; i < punchHexagon.worldGenerateObject.transform.childCount; i++)
                    {
                        Vector3 hitVector = new Vector3(center.transform.position.x, 0, center.position.z);
                        Vector3 targetVector = new Vector3(punchHexagon.worldGenerateObject.transform.GetChild(i).position.x, 0, punchHexagon.worldGenerateObject.transform.GetChild(i).position.z);
                        float distanceFromCenterHexagon = Vector3.Distance(hitVector, targetVector);
                        if (distanceFromCenterHexagon < 1.6f * h)
                        {
                            if (punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>()._imMoving == false && punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().imAtStartPos == false)
                                StartCoroutine(punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().ReturnToStartScale(punchHexagon.speedScaleCellUp));
                        }
                    }
                }
            }
       
        }
    }
}
           
