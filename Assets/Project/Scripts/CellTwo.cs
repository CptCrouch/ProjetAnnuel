using UnityEngine;
using System.Collections;

public class CellTwo : MonoBehaviour
{
    public CellType cellType = new CellType();

    public MaterialFeedBackVariables variables = new MaterialFeedBackVariables();

    public DestroyState state = new DestroyState();

    private DestructionBehavior destructionBehavior;
    [HideInInspector]
    public GameObject childTimeFeedback;

    //[HideInInspector]
    //public CellTwo cellWhoImAdjacentWith;

    [HideInInspector]
    public bool _imMoving;
    [HideInInspector]
    public bool _imReturningStartPos;
    [HideInInspector]
    public bool imAtStartPos = true;


    private Vector3 lastScale;
    private float ecartAvecStartPosY = 0;



    [HideInInspector]
    public float startPosYbyWorldGenerate;

    
    [HideInInspector]
    public float timeToGoBackToStartColor = 2f;
    [HideInInspector]
    public float speedUpByCellType;

    [HideInInspector]
    public Color startColor;
    [HideInInspector]
    public Color startEmissionColor;
    [HideInInspector]
    public Material startMat;
    


    private Color startColorTemp;
    
    private Color startEmissionColoTemp;
    
    private Material startMatTemp;

   

    public int currentAltitude;

    private Material currentMat;

    void Start()
    {
        destructionBehavior = FindObjectOfType<DestructionBehavior>();
        state = DestroyState.Idle;

        startMat = GetComponent<MeshRenderer>().material;
        startColor = startMat.color;
        startEmissionColor = startMat.GetColor("_EmissionColor");

        currentMat = startMat;

    }


    #region ChangeCellState
    // Sert dans l'editor
    public void UpdateCellType()
    {
        transform.name = cellType.name;
        //mat.set
        //GetComponent<MeshRenderer>().material.color = cellType.color;

        float lastDifference = transform.position.y - startPosYbyWorldGenerate;
        transform.position = new Vector3(transform.position.x, transform.position.y + cellType.diffWithBasePosY-lastDifference, transform.position.z);
   
    }

    public IEnumerator ChangeColor()
    {

        if (cellType.feedBackOnEmission == false && cellType.feedBackOnMaterial == false)
            startMat.color = variables.feedBackCellColor;
        else if (cellType.feedBackOnMaterial == false)
            startMat.SetColor("_EmissionColor", variables.feedBackCellColor);
        else
        {
            if (currentAltitude == 1)
                GetComponent<MeshRenderer>().material = variables.feedbackCellMaterialAlt1;
            else if (currentAltitude == 2)
                GetComponent<MeshRenderer>().material = variables.feedbackCellMaterialAlt2;
            else if (currentAltitude >= 3)
                GetComponent<MeshRenderer>().material = variables.feedbackCellMaterialAlt3;
            else
                GetComponent<MeshRenderer>().material = variables.feedbackCellMaterial;
        }
        
        yield return new WaitForEndOfFrame();
        if (_imMoving == false)
        {
            
            if (cellType.feedBackOnEmission == false && cellType.feedBackOnMaterial == false)
                startMat.color = startColor;
            else if (cellType.feedBackOnMaterial == false)
                startMat.SetColor("_EmissionColor", startEmissionColor);
            else
                GetComponent<MeshRenderer>().material = currentMat;
           
        }
        
    }

    public void ChangeScaleTimeFeedback(bool isGrow)
    {
        if (isGrow == true)
        {
            childTimeFeedback.transform.localScale = new Vector3(childTimeFeedback.transform.localScale.x * 2, childTimeFeedback.transform.localScale.y * 2, childTimeFeedback.transform.localScale.z * 2);
            childTimeFeedback.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            childTimeFeedback.transform.localScale = new Vector3(childTimeFeedback.transform.localScale.x / 2, childTimeFeedback.transform.localScale.y / 2, childTimeFeedback.transform.localScale.z / 2);
            childTimeFeedback.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    public IEnumerator StartTimerDestruction()
    {
        state = DestroyState.OnDestroy;
        childTimeFeedback.SetActive(true);
        ChangeScaleTimeFeedback(true);
        
        float timer = 0;
        while(timer<=destructionBehavior.timeToDestroyCell && state==DestroyState.OnDestroy)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (state == DestroyState.OnDestroy)
        {
            destructionBehavior.LaunchCellDestruction(this);
            childTimeFeedback.SetActive(false);
            ChangeScaleTimeFeedback(false);
        }

        
    }


    public IEnumerator ReturnToStartPos(float speed,GameObject prefabDissolve)
    {
        Vector3 firstPos = transform.position;

        currentAltitude = 0;
        if (cellType.feedBackOnMaterial == true)
        {
            GetComponent<MeshRenderer>().material = startMat;
            currentMat = startMat;
        }
        state = DestroyState.Idle;
        if (cellType.imAppliedToCell == false)
        transform.position = new Vector3(transform.position.x, startPosYbyWorldGenerate, transform.position.z);
        else
        transform.position = new Vector3(transform.position.x, startPosYbyWorldGenerate + cellType.diffWithBasePosY, transform.position.z);

        imAtStartPos = true;
        GameObject feedBackDissolve = Instantiate(prefabDissolve, firstPos,Quaternion.identity) as GameObject;
        Renderer mat = feedBackDissolve.GetComponent<Renderer>();
        mat.material.SetFloat("_Didi", 1);
        mat.material.SetVector("_ObjectPosition", new Vector4(transform.position.x, 1, transform.position.z,1));
        //Debug.Log(mat.sharedMaterial.GetFloat("_Didi"));
        while (mat.material.GetFloat("_Didi") >=0)
        {
            //Debug.Log(mat.sharedMaterial.GetFloat("_Didi"));
            float time = mat.material.GetFloat("_Didi") - Time.deltaTime * speed;
            mat.material.SetFloat("_Didi",time);
            yield return null;
        }

        destructionBehavior.ChooseRandomClosestCell(destructionBehavior.GetClosestCells(this));

        Destroy(feedBackDissolve);

    }

    

    public IEnumerator GetPunch(float strength, float speed, Vector3 direction,bool isByPlayer)
    {
        Vector3 firstPos = transform.position;
        DestroyState startState = state;

        // Aller Feedbacks

        if (cellType.feedBackOnEmission == false && cellType.feedBackOnMaterial == false)
            startMat.color = variables.colorWhenGrow;
        else if (cellType.feedBackOnMaterial == false)
            startMat.SetColor("_EmissionColor", variables.colorWhenGrow);
        else
            GetComponent<MeshRenderer>().material = variables.materialWhenGrow;
        //

        //Changement d'etat

        state = DestroyState.OnMove;
        _imMoving = true;
        //

        // Montée

        while (transform.position.y <= firstPos.y + (strength * direction.y))
        {
            transform.Translate(direction * Time.deltaTime * speed);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, firstPos.y + (strength * direction.y), transform.position.z);
        //

        //Changement d'Etat Fin

        _imMoving = false;
        imAtStartPos = false;

        currentAltitude++;

        if (isByPlayer == true)
        {
            if(startState ==DestroyState.AdjacentCell)
            {
                destructionBehavior.GetAreaDisableFeedbacks(1, destructionBehavior.GetAllCellOnDestroyInArea(1, this), true);
            }
            StartCoroutine(StartTimerDestruction());
            destructionBehavior.GetAreaEnableFeedbacks(1, this);
            Debug.Log(state);
        }
        else
            state = DestroyState.Idle;
        //

        // Retour Feedbacks
        if (cellType.feedBackOnEmission == false && cellType.feedBackOnMaterial == false)
            startMat.color = startColor;
        else if (cellType.feedBackOnMaterial == false)
            startMat.SetColor("_EmissionColor", startEmissionColor);
        else
        {
            if (currentAltitude == 1)
                GetComponent<MeshRenderer>().material =cellType.matAlt1;
            else if(currentAltitude == 2)
                GetComponent<MeshRenderer>().material = cellType.matAlt2;
            else if(currentAltitude >=3)
                GetComponent<MeshRenderer>().material = cellType.matAlt3;

            currentMat = GetComponent<MeshRenderer>().material;

        }
        //


    }

    public void EmittGrowSound()
    {
        float playerPosY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/tileUp", new Vector3(transform.position.x,playerPosY,transform.position.z));
    }
    #endregion


    

}
