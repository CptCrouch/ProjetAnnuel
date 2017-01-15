using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DestroyState { Idle, OnDestroy, OnMove, AdjacentCell}

public class DestructionBehavior : MonoBehaviour {
    WorldGenerate worldGenerate; 
    PunchHexagon punchHexagon;
     
    public float timeToWaitForDominoEffect;

    public float timeToDestroyCell;

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
        punchHexagon = FindObjectOfType<PunchHexagon>();
  
        for (int i = 0; i < worldGenerate.transform.childCount; i++)
        {
            listOfCellOnStart.Add(worldGenerate.transform.GetChild(i).GetComponent<CellTwo>());
        }
        
        
    }

   
    public void LaunchCellDestruction(CellTwo cell)
    {
        int alt = cell.currentAltitude;

        StartCoroutine(cell.ReturnToStartPos(speedFeedbackDissolve, prefabDissolve));
        List<CellTwo> cellListTemp = new List<CellTwo>();
        cellListTemp.Add(cell);
        GetAreaDisableFeedbacks(1, cellListTemp, false);
        listOfCellOnStart.Add(cell);
        ChooseAndLaunchProperty(alt, cell);
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
            random = Random.Range(0,listOfCellOnStart.Count);
        }
        CellTwo cellTwoTemp = listOfCellOnStart[random];
        
        StartCoroutine(cellTwoTemp.GetPunch(1, punchHexagon.speedScaleCellUp, Vector3.up,false));
        listOfCellOnStart.Remove(cellTwoTemp);
    }

    public void LaunchChainDestruction(Transform center, int currentIndex)
    {
        if (currentIndex > 0)
        {
            Vector3 hitVector = new Vector3(center.transform.position.x, 0, center.position.z);
            List<CellTwo> listCloseCell = new List<CellTwo>();
            for (int i = 0; i < worldGenerate.transform.childCount; i++)
            {

                CellTwo cellTwo = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
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
        Vector3 hitVector = new Vector3(center.position.x, 0, center.position.z);
        for (int h = 1; h <= areaForce; h++)
        {

            for (int i = 0; i < worldGenerate.transform.childCount; i++)
            {
                //if (CheckCellOnList(i) == true)
                //{
                CellTwo cellTwo = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
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

    public void GetAreaEnableFeedbacks(int areaForce, CellTwo cellCenter)
    {
        Vector3 hitVector = new Vector3(cellCenter.transform.position.x, 0, cellCenter.transform.position.z);
        for (int h = 1; h <= areaForce; h++)
        {
           

            for (int i = 0; i < worldGenerate.transform.childCount; i++)
            {
                
                CellTwo cellTwo = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
                if (cellTwo != cellCenter)
                {
                    Vector3 targetVector = new Vector3(cellTwo.transform.position.x, 0, cellTwo.transform.position.z);
                    float distanceFromCenterHexagon = Vector3.Distance(hitVector, targetVector);
                    if (distanceFromCenterHexagon < 1.6f * h)
                    {
                        cellTwo.state = DestroyState.AdjacentCell;
                        
                        cellTwo.childTimeFeedback.SetActive(true);
                    }
                }
                
            }
        }
    }
    public List<CellTwo> GetAllCellOnDestroyInArea(int areaForce,CellTwo center)
    {
        List<CellTwo> getList = new List<CellTwo>();
        Vector3 hitVector = new Vector3(center.transform.position.x, 0, center.transform.position.z);
        for (int h = 1; h <= areaForce; h++)
        {
           

            for (int i = 0; i < worldGenerate.transform.childCount; i++)
            {

                CellTwo cellTwo = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
                Vector3 targetVector = new Vector3(cellTwo.transform.position.x, 0, cellTwo.transform.position.z);
                float distanceFromCenterHexagon = Vector3.Distance(hitVector, targetVector);
                if (distanceFromCenterHexagon < 1.6f * h)
                {

                    if(cellTwo.state == DestroyState.OnDestroy)
                    {
                        getList.Add(cellTwo);
                    }

                }

            }
        }
        return getList;
    }

    public void GetAreaDisableFeedbacks(int areaForce, List<CellTwo> listCellTwoToDisable, bool isDisablingByPlayer)
    {
        for (int j = 0; j < listCellTwoToDisable.Count; j++)
        {


            if (isDisablingByPlayer == true)
            {
                listCellTwoToDisable[j].state = DestroyState.AdjacentCell;

                listCellTwoToDisable[j].childTimeFeedback.SetActive(true);
                listCellTwoToDisable[j].ChangeScaleTimeFeedback(false);
                // reset time feedback si besoin
            }

            for (int h = 1; h <= areaForce; h++)
            {
                Vector3 hitVector = new Vector3(listCellTwoToDisable[j].transform.position.x, 0, listCellTwoToDisable[j].transform.position.z);

                for (int i = 0; i < worldGenerate.transform.childCount; i++)
                {

                    CellTwo cellTwo = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
                    Vector3 targetVector = new Vector3(cellTwo.transform.position.x, 0, cellTwo.transform.position.z);
                    float distanceFromCenterHexagon = Vector3.Distance(hitVector, targetVector);
                    if (distanceFromCenterHexagon < 1.6f * h)
                    {

                        cellTwo.state = DestroyState.Idle;
                        cellTwo.childTimeFeedback.SetActive(false);

                    }

                }
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

   


   

    public List<CellTwo> GetClosestCells(CellTwo currentCell)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestPosition = null;
        Vector3 hitVector = new Vector3(currentCell.transform.position.x, 0, currentCell.transform.position.z);
        for (int i = 0; i < worldGenerate.transform.childCount; i++)
        {
            CellTwo cell = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
           if(cell.imAtStartPos == false )
            {
                Vector3 targetVector = new Vector3(cell.transform.position.x, 0, cell.transform.position.z);
                float distanceFromCenterHexagon = Vector3.Distance(hitVector, targetVector);
                if(distanceFromCenterHexagon < closestDistance)
                {
                    closestDistance = distanceFromCenterHexagon;
                    closestPosition = cell.transform;
                }
            }
        }

        List<CellTwo> newList = new List<CellTwo>();
        for (int i = 0; i < worldGenerate.transform.childCount; i++)
        {
            CellTwo cell = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
            if(cell.transform.position == closestPosition.position && cell.state != DestroyState.OnDestroy)
            {
                newList.Add(cell);
            }
        }

        
        return newList;
    }

    public void ChooseRandomClosestCell(List<CellTwo> list)
    {
        if (list.Count >= 1)
        {
            int random = Random.Range(0, list.Count);

            StartCoroutine(list[random].StartTimerDestruction());
            GetAreaEnableFeedbacks(1, list[random]);
        }
    }

}
           
