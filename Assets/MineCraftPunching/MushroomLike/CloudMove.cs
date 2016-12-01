using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CloudMove : MonoBehaviour
{

    [SerializeField]
    private float rangePunch = 10f;
    
    [SerializeField, Range(0, 3)]
    private int punchArea = 1;
    [SerializeField]
    private float speed = 1f;
    

    [SerializeField]
    private GameObject worldGenerateObject;
    [SerializeField]
    private Slider sliderAireForce;


    private int minX;
    private int maxX;
    private int minZ;
    private int maxZ;


    private PauseInGame pauseScript;
    private bool holdMouseButton;
    private GameObject lastTargetedCell;
   

    public List<Vector3> posBorder = new List<Vector3>();
    // Use this for initialization
    void Start()
    {
        pauseScript = FindObjectOfType<PauseInGame>();
    }


    // Update is called once per frame
    void Update()
    {
        Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

       
        
        

        //punchArea = Mathf.RoundToInt(sliderAireForce.value);
        //profondeur = sliderProfondeur.value;


        if (pauseScript.isActive == false)
        {
            
            if (Input.GetMouseButtonDown(1))
            {
                holdMouseButton = true;

            }
            if (Physics.Raycast(rayMouse, out hit, rangePunch/*, layerMask*/))
            {

                StartCoroutine(hit.collider.gameObject.GetComponent<Cell>().ChangeColor());
                /*if(Input.GetMouseButton(0))
                {
                    sliderProfondeur.value += Time.deltaTime * speedSliderBar;
                    profondeur = sliderProfondeur.value;
                }*/
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.gameObject.GetComponent<Cell>()._imMoving == false && hit.collider.gameObject != lastTargetedCell)
                    {
                        float hitPosX = hit.collider.transform.position.x;
                        float hitPosY = hit.collider.transform.position.y;
                        float hitPosZ = hit.collider.transform.position.z;

                        lastTargetedCell = hit.collider.gameObject;
                        for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                        {
                            worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetNeighbourHood();
                        }
                        GetPosBorderRaycast();
                        //GetPosOnBorder(GetPosMinMaxHV());

                        int random = Random.Range(0, posBorder.Count);
                        while (posBorder[random] == new Vector3(hitPosX + 1, hitPosY, hitPosZ)
                            || posBorder[random] == new Vector3(hitPosX - 1, hitPosY, hitPosZ)
                            || posBorder[random] == new Vector3(hitPosX, hitPosY, hitPosZ + 1)
                            || posBorder[random] == new Vector3(hitPosX, hitPosY, hitPosZ - 1))
                            random = Random.Range(0, posBorder.Count);
                        Debug.Log(posBorder.Count);
                        hit.collider.GetComponent<Cell>().GoOnBorder(posBorder[random]);



                        posBorder.Clear();
                    }

                }
                if (holdMouseButton == true)
                {

                    if (hit.collider.gameObject.GetComponent<Cell>()._imMoving == false && hit.collider.gameObject != lastTargetedCell)
                    {
                        float hitPosX = hit.collider.transform.position.x;
                        float hitPosY = hit.collider.transform.position.y;
                        float hitPosZ = hit.collider.transform.position.z;

                        lastTargetedCell = hit.collider.gameObject;
                        for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                        {
                            worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetNeighbourHood();
                        }
                        GetPosBorderRaycast();
                        //GetPosOnBorder(GetPosMinMaxHV());

                        int random = Random.Range(0, posBorder.Count );
                        while(posBorder[random] == new Vector3(hitPosX+1,hitPosY,hitPosZ) 
                            || posBorder[random] == new Vector3(hitPosX - 1, hitPosY, hitPosZ) 
                            || posBorder[random] == new Vector3(hitPosX , hitPosY, hitPosZ +1) 
                            || posBorder[random] == new Vector3(hitPosX, hitPosY, hitPosZ-1))
                            random = Random.Range(0, posBorder.Count);
                        Debug.Log(posBorder.Count);
                        hit.collider.GetComponent<Cell>().GoOnBorder(posBorder[random]);
                        


                        posBorder.Clear();
                        }
                }


            }
            if (Input.GetMouseButtonUp(1))
            {
                holdMouseButton = false;
                lastTargetedCell = null;
            }

        }
    }
    

    bool CheckPosValidity(Vector3 posTested)
    {
        for (int i = 0; i < posBorder.Count; i++)
        {
            if(posBorder[i] == posTested)
            {
                return false;
            }
        }
        return true;
    }
    public void GetPosBorderRaycast()
    {
        for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
        {
            Cell cell = worldGenerateObject.transform.GetChild(i).GetComponent<Cell>();
            if(cell.voisinNord == null)
            {
                Vector3 posNord = new Vector3(cell.transform.position.x, cell.transform.position.y, cell.transform.position.z + 1);
                if (CheckPosValidity(posNord) == true)
                    posBorder.Add(posNord);

            }
            if (cell.voisinSud == null)
            {
                Vector3 posSud = new Vector3(cell.transform.position.x, cell.transform.position.y, cell.transform.position.z -1);
                if (CheckPosValidity(posSud) == true)
                    posBorder.Add(posSud);

            }
            if (cell.voisinOuest == null)
            {
                Vector3 posOuest = new Vector3(cell.transform.position.x -1, cell.transform.position.y, cell.transform.position.z);
                if (CheckPosValidity(posOuest) == true)
                    posBorder.Add(posOuest);

            }
            if (cell.voisinEst == null)
            {
                Vector3 posEst = new Vector3(cell.transform.position.x + 1, cell.transform.position.y, cell.transform.position.z);
                if (CheckPosValidity(posEst) == true)
                    posBorder.Add(posEst);

            }
        }

    }

























    /// <summary>
    /// Rounding float value with defined digit precision.
    /// </summary>
    /// <param name ="num">Number to be rounded</param>
    /// <param name ="precision">Number of digit after comma (100 will be 0.00, 1000 will be 0.000 etc..)</param>
    /// <returns> Rounded float value </returns>
    public static float RoundValue(float num, float precision)
    {
        return Mathf.Floor(num * precision + 0.5f) / precision;
    }

    public void GetPosOnBorder(Vector4 posMinMaxHV)
    {
        Debug.Log(posMinMaxHV);
        minX = Mathf.RoundToInt(posMinMaxHV.x);
        maxX = Mathf.RoundToInt(posMinMaxHV.y);
        
        minZ = Mathf.RoundToInt(posMinMaxHV.z);
        maxZ = Mathf.RoundToInt(posMinMaxHV.w);

        int longueurHorizontal = maxX - minX;
        int longueurVertical = maxZ - minZ;

        for (int i = 0; i < longueurHorizontal; i++)
        {
            Vector4 bothPosMinMax = GetPosHorizontalIndex(minX + i);
            posBorder.Add(new Vector3(bothPosMinMax.x, 0, bothPosMinMax.y));
            posBorder.Add(new Vector3(bothPosMinMax.z, 0, bothPosMinMax.w));
        }
        for (int i = 0; i < longueurVertical; i++)
        {
            Vector4 bothPosMinMax = GetPosVerticalIndex(minZ + i);
            posBorder.Add(new Vector3(bothPosMinMax.x, 0, bothPosMinMax.y));
            posBorder.Add(new Vector3(bothPosMinMax.z, 0, bothPosMinMax.w));
        }

    }

    // recuperer les positions minimum et maximum de l'ile en général
    public Vector4 GetPosMinMaxHV()
    {
        float minX = 0;
        float maxX = 0;
        float minZ = 0;
        float maxZ = 0;
        for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
        {
            if(worldGenerateObject.transform.GetChild(i).transform.position.x < minX)
            {
                minX = worldGenerateObject.transform.GetChild(i).transform.position.x;
            }
            if (worldGenerateObject.transform.GetChild(i).transform.position.x > maxX)
            {
                maxX = worldGenerateObject.transform.GetChild(i).transform.position.x;
            }
            if (worldGenerateObject.transform.GetChild(i).transform.position.z < minZ)
            {
                minZ = worldGenerateObject.transform.GetChild(i).transform.position.z;
            }
            if (worldGenerateObject.transform.GetChild(i).transform.position.z > maxZ)
            {
                maxZ = worldGenerateObject.transform.GetChild(i).transform.position.z;
            }
        }
        return new Vector4(minX,maxX,minZ, maxZ);
    }

    // Recuperer les deux positions min et max pour une ligne horizontale
    public Vector4 GetPosHorizontalIndex(int posZ)
    {
        float min=0f;
        float max=0f;
        for (int j = 0; j < worldGenerateObject.transform.childCount; j++)
        {
            if (worldGenerateObject.transform.GetChild(j).position.z == posZ)
            {
                if (worldGenerateObject.transform.GetChild(j).position.x < min)
                {
                    min = worldGenerateObject.transform.GetChild(j).position.x;
                }
                if (worldGenerateObject.transform.GetChild(j).position.x > max)
                {
                    max = worldGenerateObject.transform.GetChild(j).position.x;
                }
            }
        }
        return new Vector4(min - 1,posZ, max + 1,posZ);
    }
    // Recuperer les deux positions min et max pour une colonne verticale
    public Vector4 GetPosVerticalIndex(int posX)
    {
        float min = 0f;
        float max = 0f;
        for (int j = 0; j < worldGenerateObject.transform.childCount; j++)
        {
            if (worldGenerateObject.transform.GetChild(j).position.x == posX)
            {
                if (worldGenerateObject.transform.GetChild(j).position.z < min)
                {
                    min = worldGenerateObject.transform.GetChild(j).position.z;
                }
                if (worldGenerateObject.transform.GetChild(j).position.z > max)
                {
                    max = worldGenerateObject.transform.GetChild(j).position.z;
                }
            }
        }
        return new Vector4(posX, min - 1, posX, max + 1);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(0,0.5f,0), 0.5f);
    }














}

