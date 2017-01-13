using UnityEngine;
using System.Collections;

public class CellTwo : MonoBehaviour
{
    public CellType cellType = new CellType();

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
    public Color startColorbyWorldGenerate;
    [HideInInspector]
    public Color colorFeedback;
    [HideInInspector]
    public Color colorWhenGrow;
    [HideInInspector]
    public Color colorWhenTargeted;

    [HideInInspector]
    public Material matFeedback;
    [HideInInspector]
    public Material matFeedbackAlt1;
    [HideInInspector]
    public Material matFeedbackAlt2;
    [HideInInspector]
    public Material matFeedbackAlt3;



    [HideInInspector]
    public Material matWhenGrow;
    [HideInInspector]
    public Material materialWhenTargeted;





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
    [HideInInspector]
    public bool imTargeted;


    private Color startColorTemp;
    
    private Color startEmissionColoTemp;
    
    private Material startMatTemp;

   

    public int currentAltitude;

    private Material currentMat;

    void Start()
    {
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
            startMat.color = colorFeedback;
        else if (cellType.feedBackOnMaterial == false)
            startMat.SetColor("_EmissionColor", colorFeedback);
        else
        {
            if (currentAltitude == 1)
                GetComponent<MeshRenderer>().material = matFeedbackAlt1;
            else if (currentAltitude == 2)
                GetComponent<MeshRenderer>().material = matFeedbackAlt2;
            else if (currentAltitude >= 3)
                GetComponent<MeshRenderer>().material = matFeedbackAlt3;
            else
                GetComponent<MeshRenderer>().material = matFeedback;
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

    


    public IEnumerator ReturnToStartPos(float speed,GameObject prefabDissolve)
    {
        Vector3 firstPos = transform.position;

        currentAltitude = 0;
        if (cellType.feedBackOnMaterial == true)
        {
            GetComponent<MeshRenderer>().material = startMat;
            currentMat = startMat;
        }

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
        Destroy(feedBackDissolve);

    }



    public IEnumerator GetPunch(float strength, float speed, Vector3 direction)
    {
        Vector3 firstPos = transform.position;

        if (cellType.feedBackOnEmission == false && cellType.feedBackOnMaterial == false)
            startMat.color = colorWhenGrow;
        else if (cellType.feedBackOnMaterial == false)
            startMat.SetColor("_EmissionColor", colorWhenGrow);
        else
            GetComponent<MeshRenderer>().material = matWhenGrow;
        


        _imMoving = true;
       
        while (transform.position.y <= firstPos.y + (strength * direction.y))
        {
            transform.Translate(direction * Time.deltaTime * speed);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, firstPos.y + (strength * direction.y), transform.position.z);
        
        _imMoving = false;
        imAtStartPos = false;

        currentAltitude++;

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
        
        



    }

    public void EmittGrowSound()
    {
        float playerPosY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/tileUp", new Vector3(transform.position.x,playerPosY,transform.position.z));
    }
    #endregion


    

}
