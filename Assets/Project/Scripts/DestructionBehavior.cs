using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructionBehavior : MonoBehaviour {
    WorldGenerate worldGenerate; 
    PunchHexagon punchHexagon;
    

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
    [Header("[ FeedbackDissolve ]")]
    [SerializeField]
    public float speedFeedbackDissolve = 0.5f;
    [SerializeField]
    public GameObject prefabDissolve;
    

    private List<CellTwo> cellToDissolve = new List<CellTwo>();

    
    
 
    [HideInInspector]
    public List<CellTwo> listOfCellOnStart = new List<CellTwo>();
    

    
    


    // Use this for initialization
    void Start() {
        
        worldGenerate = FindObjectOfType<WorldGenerate>();
  
        for (int i = 0; i < worldGenerate.transform.childCount; i++)
        {
            listOfCellOnStart.Add(worldGenerate.transform.GetChild(i).GetComponent<CellTwo>());
        }
        
    }


    public IEnumerator ChangeMaterialHighlight()
    {

        GetComponent<MeshRenderer>().material.SetFloat("_Emission", 0.4f);
        yield return new WaitForEndOfFrame();
        GetComponent<MeshRenderer>().material.SetFloat("_Emission", 0f);
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

                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                listOfCellOnStart.Add(cellCollided);
                ChooseAndLaunchProperty(alt,cellCollided);

                //GetAreaOfCellAndLaunchReturn(1, cellCollided.transform, alt);
               


                
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
               
                int alt = cellCollided.currentAltitude;
                
                StartCoroutine(cellCollided.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
                listOfCellOnStart.Add(cellCollided);
                ChooseAndLaunchProperty(alt, cellCollided);
                //GetAreaOfCellAndLaunchReturn(1, cellCollided.transform,alt);


                
                
            }

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

   


    /*Transform GetClosestCell(Transform worldGen)
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
    }*/
}
           
