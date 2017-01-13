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
    [Header("[ Clamp Ball and Bouncing ]")]
    [SerializeField]
    public float multiplicateurVelocityCollide = 100f;
    [SerializeField]
    private float angleMaxVelocityY = 5f;
    [SerializeField]
    private float maxSpeedToAddBounce= 5f;
    [SerializeField]
    private bool addBounceOnBall = false;

    [Space(10)]
    [Header("[ Move Auto ]")]
    [SerializeField]
    public float minimumSpeedToAutoMove = 5f;
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
    [HideInInspector]
    public float currentSpeed = 0f;
    [HideInInspector]
    public float speedToUse = 0f;

    private float currentVelocityY;
    private bool currentlyMovingTowardCell = false;
    private float currentTime =1f;

    [HideInInspector]
    public List<CellTwo> listOfCellOnStart = new List<CellTwo>();
    

    
    


    // Use this for initialization
    void Start() {
        punchNotRandom = FindObjectOfType<PunchNotRandom>();
        if (punchNotRandom == null)
        {
            punchHexagon = FindObjectOfType<PunchHexagon>();
        }
        worldGenerate = FindObjectOfType<WorldGenerate>();
        rb = GetComponent<Rigidbody>();
        
        currentTime = startSpeedAfterCollide;
        for (int i = 0; i < worldGenerate.transform.childCount; i++)
        {
            listOfCellOnStart.Add(worldGenerate.transform.GetChild(i).GetComponent<CellTwo>());
        }
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        currentVelocityY = rb.velocity.y;
        currentVelocityY = Mathf.Clamp(rb.velocity.y, -1000, angleMaxVelocityY);
        rb.velocity = new Vector3(rb.velocity.x, currentVelocityY, rb.velocity.z);

        currentSpeed = rb.velocity.magnitude;
        //Debug.Log(rb.velocity.magnitude);


        if (currentCellTarget != null && imGrounded == true)
        {
            Vector3 direction = new Vector3(currentCellTarget.position.x, 0, currentCellTarget.position.z) - transform.position;
 
            currentTime += Time.deltaTime;

            rb.velocity = direction.normalized * speedToUse;
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

    /*void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("pouet");
        if (collision.transform.tag == "Cell")
        {

            CellTwo cellCollided = collision.GetComponent<CellTwo>();

            if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
            {

                int alt = cellCollided.currentAltitude;
                
                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                GetAreaOfCell(1, cellCollided.transform,alt);
                //cellToDissolve.Add(cellCollided);
                //LaunchDissolve();
                //rbPlayer.velocity = Vector3.zero;
                if (rb.velocity.magnitude < maxSpeedToAddBounce && addBounceOnBall == true)
                {
                    //rb.velocity *= multiplicateurVelocityCollide;
                }
                currentTime = startSpeedAfterCollide;
                
               currentCellTarget = null;
            }
        }
          
    }*/
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("pouet");
        if (collision.collider.transform.tag == "Cell")
        {

            CellTwo cellCollided = collision.collider.GetComponent<CellTwo>();

            if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
            {

                int alt = cellCollided.currentAltitude;

                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                listOfCellOnStart.Add(cellCollided);
                ChooseAndLaunchProperty(alt,cellCollided);

                //GetAreaOfCellAndLaunchReturn(1, cellCollided.transform, alt);
                if (rb.velocity.magnitude < maxSpeedToAddBounce && addBounceOnBall == true)
                {
                    rb.velocity *= multiplicateurVelocityCollide;
                }
                currentTime = startSpeedAfterCollide;


                currentCellTarget = null;
            }
            
        }

    }
    /*void OnTriggerStay(Collider collision)
    {
        if (collision.transform.tag == "Cell")
        {
            CellTwo cellCollided = collision.GetComponent<CellTwo>();
            if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
            {
               
                int alt = cellCollided.currentAltitude;
                
                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                GetAreaOfCell(1, cellCollided.transform,alt);
                currentTime = startSpeedAfterCollide;
                currentCellTarget = null;
            }

        }
    }*/
    
    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.transform.tag == "Cell")
        {
            CellTwo cellCollided = collision.collider.GetComponent<CellTwo>();
            if (cellCollided._imMoving == false && cellCollided.imAtStartPos == false)
            {
               
                int alt = cellCollided.currentAltitude;
                
                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                listOfCellOnStart.Add(cellCollided);
                ChooseAndLaunchProperty(alt, cellCollided);
                //GetAreaOfCellAndLaunchReturn(1, cellCollided.transform,alt);


                currentTime = startSpeedAfterCollide;
                currentCellTarget = null;
            }

        }
        else
        if (collision.collider.transform.tag == "Ground")
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

    public void ChooseAndLaunchProperty(int altitude, CellTwo target)
    {
        if(altitude ==1)
        {
            UpOneCellRandom(target);
           
        }
        else if(altitude == 2)
        {
            StartCoroutine(WaitForAreaEffect(1, target.transform, altitude));
        }
        else if(altitude >= 3)
        {
            StartCoroutine(WaitForDominosEffect(target.transform, altitude));
        }
    }

    public void UpOneCellRandom(CellTwo cellToAvoid)
    {
        int random = Random.Range(0, listOfCellOnStart.Count);
        //Debug.Log(random);
        while(listOfCellOnStart[random] == cellToAvoid)
        {
            random = Random.Range(0, worldGenerate.transform.childCount);
        }
        CellTwo cellTwoTemp = listOfCellOnStart[random].GetComponent<CellTwo>();
        StartCoroutine(cellTwoTemp.GetPunch(1, punchHexagon.speedScaleCellUp, Vector3.up));
        listOfCellOnStart.Remove(cellTwoTemp);
    }

    public void LaunchChainDestruction(Transform center, int currentIndex)
    {
        if (currentIndex > 0)
        {
            Vector3 hitVector = new Vector3(center.transform.position.x, 0, center.position.z);
            List<CellTwo> listCloseCell = new List<CellTwo>();
            for (int i = 0; i < punchHexagon.worldGenerateObject.transform.childCount; i++)
            {

                CellTwo cellTwo = punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>();
                Vector3 targetVector = new Vector3(cellTwo.transform.position.x, 0, cellTwo.transform.position.z);
                float distanceFromCenterHexagon = Vector3.Distance(hitVector, targetVector);

                if (distanceFromCenterHexagon < 1.6f)
                {

                    if (cellTwo._imMoving == false && cellTwo.imAtStartPos == false /*&& cellTwo.currentAltitude < startAltitude*/)
                    {
                        listCloseCell.Add(cellTwo);
                    }
                }
                //}
            }
            if(listCloseCell.Count > 0)
            {
                int random = Random.Range(0, listCloseCell.Count - 1);
                ChooseAndLaunchProperty(listCloseCell[random].currentAltitude, listCloseCell[random]);

                StartCoroutine(listCloseCell[random].ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                listOfCellOnStart.Add(listCloseCell[random]);
                StartCoroutine(WaitForDominosEffect(listCloseCell[random].transform, currentIndex - 1));
            }
 
        }

    }


    IEnumerator WaitForAreaEffect(int area, Transform center,int startAltitude)
    {
        yield return new WaitForSeconds(timeToWaitForDominoEffect);
        GetAreaOfCellAndLaunchReturn(area,center,startAltitude);
    }
    IEnumerator WaitForDominosEffect( Transform center, int currentIndex)
    {
        yield return new WaitForSeconds(timeToWaitForDominoEffect);
        LaunchChainDestruction(center, currentIndex);
    }


    void GetAreaOfCellAndLaunchReturn(int areaForce, Transform center,int startAltitude )
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
                    
                    if (cellTwo._imMoving == false && cellTwo.imAtStartPos == false /*&& cellTwo.currentAltitude < startAltitude*/)
                    {
                        //cellToDissolve.Add(punchHexagon.worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>());
                        int alt = cellTwo.currentAltitude;
                        
                        StartCoroutine(cellTwo.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                        listOfCellOnStart.Add(cellTwo);
                        ChooseAndLaunchProperty(alt, cellTwo);

                        //StartCoroutine(WaitForDominoEffect(1, cellTwo.transform,alt));
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
           
