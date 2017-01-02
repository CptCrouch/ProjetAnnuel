using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallBehavior : MonoBehaviour {
    WorldGenerate worldGenerate;
    PunchNotRandom punchNotRandom;
    PunchHexagon punchHexagon;
    [HideInInspector]
    public Rigidbody rb;

    [Header ("[ ForceArea On Collision ]")]
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


    [Space(10)]
    [Header("[ Clamp Ball ]")]
    [SerializeField]
    public float multiplicateurVelocityCollide = 100f;
    [SerializeField]
    public float velocityMaxToAddBounce= 15f;
    [SerializeField]
    private float angleMaxVelocityY = 5f;
    [SerializeField]
    private float minimumSpeedToStop = 1f;
    [Space(10)]
    [Header("[ Move Auto ]")]
    [SerializeField]
    private float minimumSpeedToAutoMove = 5f;
    [SerializeField]
    private float speedAutoMove = 2f;

    [Space(10)]
    [Header("[ FeedbackDissolve ]")]
    [SerializeField]
    public float speedFeedbackDissolve = 0.5f;
    [SerializeField]
    public GameObject prefabDissolve;

    private List<GameObject> objectToDissolve = new List<GameObject>();

    [HideInInspector]
    public Transform currentCellTarget;
    [HideInInspector]
    public bool imGrounded = false;

    private float currentVelocityY;
    private bool currentlyMovingTowardCell = false;
    


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
    void FixedUpdate() {
        currentVelocityY = rb.velocity.y;
        currentVelocityY = Mathf.Clamp(rb.velocity.y, -1000, angleMaxVelocityY);
        rb.velocity = new Vector3(rb.velocity.x, currentVelocityY, rb.velocity.z);
        //Debug.Log(rb.velocity.magnitude);


        if (rb.velocity.magnitude < minimumSpeedToAutoMove && currentCellTarget != null && imGrounded == true)
        {
            Vector3 direction = new Vector3(currentCellTarget.position.x, 0, currentCellTarget.position.z) - transform.position;
            Debug.Log(direction.normalized);
            rb.velocity = direction.normalized*speedAutoMove;
            //rb.velocity = Vector3.Lerp(rb.velocity, direction.normalized * speedAutoMove, Time.deltaTime);
        }
        
        if (imGrounded && rb.velocity.magnitude < minimumSpeedToStop)
            rb.velocity = Vector3.zero;

    }

    public IEnumerator ChangeMaterialHighlight()
    {

        GetComponent<MeshRenderer>().material.SetFloat("_Emission", 0.4f);
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

            CellTwo cellCollided = collision.collider.GetComponent<CellTwo>();
            
            
                
            
                //Debug.Log(puissanceCollision);
                if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
                {
                
                float puissanceCollision = collision.relativeVelocity.magnitude;

                if (punchHexagon == null)
                        StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve,prefabDissolve,1));
                    else
                    {
                    if (puissanceCollision > puissanceMinimale)
                    {
                        objectToDissolve.Add(cellCollided.gameObject);
                        if (rb.velocity.magnitude< velocityMaxToAddBounce)
                        {
                            Debug.Log("Avant  " + rb.velocity);
                            rb.velocity *= multiplicateurVelocityCollide*1.5f;
                            Debug.Log("Après  " + rb.velocity);
                        }
                    }
                    }

                if (puissanceCollision >= puissanceMinimumArea1 && puissanceCollision < puissanceMinimumArea2)
                {
                    GetAreaOfCell(1, cellCollided.transform);
                    if (rb.velocity.magnitude < velocityMaxToAddBounce)
                    {
                        Debug.Log("Avant  " + rb.velocity);
                        rb.velocity *= multiplicateurVelocityCollide*0.2f;
                        Debug.Log("Après  " + rb.velocity);
                    }

                }
                if (puissanceCollision >= puissanceMinimumArea2 && puissanceCollision < puissanceMinimumArea3)
                {
                    GetAreaOfCell(2, cellCollided.transform);
                    if (rb.velocity.magnitude < velocityMaxToAddBounce)
                    {
                        Debug.Log("Avant  " + rb.velocity);
                        rb.velocity *= multiplicateurVelocityCollide*0.1f;
                        Debug.Log("Après  " + rb.velocity);
                    }

                }
                if (puissanceCollision >= puissanceMinimumArea3 && puissanceCollision < puissanceMinimumArea4)
                {
                    GetAreaOfCell(3, cellCollided.transform);
                    if (rb.velocity.magnitude < velocityMaxToAddBounce)
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
                LaunchDissolve();

                if (cellCollided.transform == currentCellTarget)
                {
                    currentCellTarget = null;
                }
            }
           }
          
    }
    
    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.transform.tag == "Cell")
        {
            CellTwo cellCollided = collision.collider.GetComponent<CellTwo>();
            if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
            {
                objectToDissolve.Add(cellCollided.gameObject);
                LaunchDissolve();
                rb.velocity *= multiplicateurVelocityCollide;
                if (cellCollided.transform == currentCellTarget)
                {
                    currentCellTarget = null;
                }
            }

        }
        else if(collision.collider.transform.tag == "Ground")
        {
            imGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.transform.tag == "Ground")
        {
            imGrounded = false;

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
                        if (punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>()._imMoving == false && punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().imAtStartPos == false)
                            StartCoroutine(punchNotRandom.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().ReturnToStartPos(speedFeedbackDissolve,prefabDissolve,1));
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
                            if (punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>()._imMoving == false && punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().imAtStartPos == false)
                                objectToDissolve.Add(punchHexagon.worldGenerateObject.transform.GetChild(i).gameObject);
                        }
                    }
                }
            }
       
        }
    }


    void LaunchDissolve()
    {
        Debug.Log(objectToDissolve.Count);
        for (int i = 0; i < objectToDissolve.Count; i++)
        {
            StartCoroutine(objectToDissolve[i].GetComponent<CellTwo>().ReturnToStartPos(speedFeedbackDissolve, prefabDissolve, objectToDissolve.Count));
            if(objectToDissolve[i].transform == currentCellTarget)
            {
                currentCellTarget = null;
            }
        }
        objectToDissolve.Clear();
    }


    Transform GetClosestCell(Transform worldGen)
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentBallPos = transform.position;
        for (int i = 0; i < worldGen.childCount; i++)
        {
            if (worldGen.GetChild(i).GetComponent<Cell>().imAtStartPos == false)
            {
                Vector3 directionToTarget = new Vector3(worldGen.GetChild(i).position.x, currentBallPos.y, worldGen.GetChild(i).position.z) - currentBallPos;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistance)
                {
                    closestDistance = dSqrToTarget;
                    closestTarget = worldGen.GetChild(i);
                }
            }
        }

        return closestTarget;
    }
}
           
