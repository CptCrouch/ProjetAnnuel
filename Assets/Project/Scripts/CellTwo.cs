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
    public float timeToGoBackToStartColor = 2f;
    [HideInInspector]
    public float speedUpByCellType;



    public void UpdateCellType()
    {
        transform.name = cellType.name;
        GetComponent<MeshRenderer>().material.color = cellType.color;

        float lastDifference = transform.position.y - startPosYbyWorldGenerate;
        transform.position = new Vector3(transform.position.x, transform.position.y + cellType.diffWithBasePosY-lastDifference, transform.position.z);
   
    }

    public IEnumerator ChangeColor()
    {
        GetComponent<MeshRenderer>().material.color = colorFeedback;
        
        yield return new WaitForEndOfFrame();
        if (_imMoving == false)
        {
            if(cellType.imAppliedToCell == false)
                GetComponent<MeshRenderer>().material.color = startColorbyWorldGenerate;
            else
                GetComponent<MeshRenderer>().material.color = cellType.color;
            
        }
        //yield return new WaitForSeconds(0.5f);
        /*while (GetComponent<MeshRenderer>().material.color != startColor)
        {
            Color lerpedColor = Color.Lerp(Color.white, startColor, Time.deltaTime * 10);
            GetComponent<MeshRenderer>().material.color = lerpedColor;
            if (transform.childCount > 0)
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = lerpedColor;
        }*/

        /*GetComponent<MeshRenderer>().material.color = startColor;
        if (transform.childCount > 0)
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = startColor;*/

    }

    


    public IEnumerator ReturnToStartPos(float speed,GameObject prefabDissolve,int numberToDissolve)
    {
        Vector3 firstPos = transform.position;
        
        if(cellType.imAppliedToCell == false)
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
        GetComponent<MeshRenderer>().material.color = colorWhenGrow;
        _imMoving = true;
       
        while (transform.position.y <= firstPos.y + (strength * direction.y))
        {
            transform.Translate(direction * Time.deltaTime * speed);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, firstPos.y + (strength * direction.y), transform.position.z);
        
        _imMoving = false;
        imAtStartPos = false;
        //yield return new WaitForSeconds(0.5f);
        if(cellType.imAppliedToCell == false)
            GetComponent<MeshRenderer>().material.color = startColorbyWorldGenerate;
        else
            GetComponent<MeshRenderer>().material.color = cellType.color;

    }
   
}
