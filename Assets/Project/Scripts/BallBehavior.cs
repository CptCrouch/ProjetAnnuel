using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallBehavior : MonoBehaviour {
    WorldGenerate worldGenerate;
    PunchNotRandom punchNotRandom;
    PunchHexagon punchHexagon;
    [HideInInspector]
    public Rigidbody rb;

    [Header("[ ForceArea On Collision ]")]
    /*[SerializeField]
    public float puissanceMinimale = 2f;
    [SerializeField]
    public float puissanceMinimumArea1 = 4f;
    [SerializeField]
    public float puissanceMinimumArea2 = 5f;
    [SerializeField]
    public float puissanceMinimumArea3 = 7f;
    [SerializeField]
    public float puissanceMinimumArea4 = 10f;*/
    public float timeToWaitForDominoEffect;


    [Space(10)]
    [Header("[ Clamp Ball ]")]
    [SerializeField]
    public float multiplicateurVelocityCollide = 100f;
    [SerializeField]
    private float angleMaxVelocityY = 5f;
    
    [Space(10)]
    [Header("[ Move Auto ]")]
    //[SerializeField]
    //private float minimumSpeedToAutoMove = 5f;
    [SerializeField]
    private float multiplicateurSpeedAutoMove = 2f;
    [SerializeField]
    private float startSpeedAfterCollide = 1f;

    [Space(10)]
    [Header("[ FeedbackDissolve ]")]
    [SerializeField]
    public float speedFeedbackDissolve = 0.5f;
    [SerializeField]
    public GameObject prefabDissolve;
    public int maxAltitude;

    private List<CellTwo> cellToDissolve = new List<CellTwo>();

    [HideInInspector]
    public Transform currentCellTarget;
    [HideInInspector]
    public bool imGrounded = false;

    private float currentVelocityY;
    private bool currentlyMovingTowardCell = false;
    private float currentTime =1f;

    public Rigidbody rbPlayer;
    


    // Use this for initialization
    void Start() {
        punchNotRandom = FindObjectOfType<PunchNotRandom>();
        if (punchNotRandom == null)
        {
            punchHexagon = FindObjectOfType<PunchHexagon>();
        }
        worldGenerate = FindObjectOfType<WorldGenerate>();
        rb = GetComponent<Rigidbody>();
        rbPlayer = GameObject.Find("Player").GetComponent<Rigidbody>();
        currentTime = startSpeedAfterCollide;
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        currentVelocityY = rb.velocity.y;
        currentVelocityY = Mathf.Clamp(rb.velocity.y, -1000, angleMaxVelocityY);
        rb.velocity = new Vector3(rb.velocity.x, currentVelocityY, rb.velocity.z);
        //Debug.Log(rb.velocity.magnitude);


        if (currentCellTarget != null && imGrounded == true)
        {
            Vector3 direction = new Vector3(currentCellTarget.position.x, 0, currentCellTarget.position.z) - transform.position;
            //Debug.Log(direction.normalized);
            currentTime += Time.deltaTime;
            rb.velocity = direction.normalized* multiplicateurSpeedAutoMove *currentTime;
            //rbPlayer.velocity = direction.normalized * multiplicateurSpeedAutoMove * currentTime;
            //rb.velocity = Vector3.Lerp(rb.velocity, direction.normalized * speedAutoMove, Time.deltaTime);
        }
        //rbPlayer.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);


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

            if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
            {

                int alt = cellCollided.currentAltitude;
                if (alt > maxAltitude)
                    alt = maxAltitude;
                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                GetAreaOfCell(alt, cellCollided.transform);
                //cellToDissolve.Add(cellCollided);
                //LaunchDissolve();
                //rbPlayer.velocity = Vector3.zero;
                //rb.velocity *= 10f;
                currentTime = startSpeedAfterCollide;
               currentCellTarget = null;
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
                //cellToDissolve.Add(cellCollided);
                //LaunchDissolve();
                int alt = cellCollided.currentAltitude;
                if (alt > maxAltitude)
                    alt = maxAltitude;
                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                GetAreaOfCell(alt, cellCollided.transform);

                rbPlayer.velocity = Vector3.zero;
                currentTime = startSpeedAfterCollide;
                currentCellTarget = null;
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

    IEnumerator WaitForDominoEffect(int area, Transform center)
    {
        yield return new WaitForSeconds(timeToWaitForDominoEffect);
        GetAreaOfCell(area,center);
    }


    void GetAreaOfCell(int areaForce, Transform center)
    {
        for (int h = 1; h <= areaForce; h++)
        {
            Vector3 hitVector = new Vector3(center.transform.position.x, 0, center.position.z);

            for (int i = 0; i < punchHexagon.worldGenerateObject.transform.childCount; i++)
            {
                //if (CheckCellOnList(i) == true)
                //{
                CellTwo cellTwo = punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>();
                Vector3 targetVector = new Vector3(cellTwo.transform.position.x, 0, cellTwo.transform.position.z);
                float distanceFromCenterHexagon = Vector3.Distance(hitVector, targetVector);
                if (distanceFromCenterHexagon < 1.6f * h)
                {
                    if (cellTwo._imMoving == false && cellTwo.imAtStartPos == false)
                    {
                        //cellToDissolve.Add(punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>());
                        int alt = cellTwo.currentAltitude;
                        if (alt > maxAltitude)
                            alt = maxAltitude;
                        StartCoroutine(cellTwo.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));

                        StartCoroutine(WaitForDominoEffect(alt, cellTwo.transform));
                    }
                }
                //}
            }
          
        }
    }
    bool CheckCellOnList(int index)
    {
        for (int i = 0; i < cellToDissolve.Count; i++)
        {
            if(cellToDissolve[i] == punchHexagon.worldGenerateObject.transform.GetChild(index))
            {
                return false;
            }
        }
        return true;
    }

    void LaunchDissolve()
    {
        Debug.Log(cellToDissolve.Count);
        for (int i = 0; i < cellToDissolve.Count; i++)
        {
            StartCoroutine(cellToDissolve[i].ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
            if(cellToDissolve[i].transform == currentCellTarget)
            {
                currentCellTarget = null;
            }
        }
        cellToDissolve.Clear();
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
           
