using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PunchHexagon : MonoBehaviour {

    [SerializeField]
    private float rangeEarthTool = 10f;
    [SerializeField]
    private float rangeBallTool = 10f;
    [SerializeField, Range(0, 3)]
    private float profondeur = 1f;
    [SerializeField, Range(0, 3)]
    private int punchArea = 1;
    [SerializeField]
    public float speedScaleCellUp = 1f;
    [SerializeField]
    public float speedScaleCellDown = 1f;
    [SerializeField]
    private float forceShootBall = 10f;

    [SerializeField]
    public GameObject worldGenerateObject;
    [SerializeField]
    private Slider sliderAireForce;
    [SerializeField]
    private Slider sliderProfondeur;
    [SerializeField]
    private float speedSliderBar = 10f;
    [SerializeField]
    private bool forceUniform;
    [SerializeField]
    public bool punchAireForceActivate = true;
    [SerializeField]
    public BallBehavior ballBehavior;


    private PauseInGame pauseScript;
    
    private bool holdMouseButton;
    private GameObject lastTargetedCell;
    private GameObject lastTargetedFeedbackCell;
    private List<GameObject> lastFeedBackCells = new List<GameObject>();
    private Vector3 choosedTool;
    private Vector3 choosedReaction;

    private List<GameObject> cellTargeted = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        pauseScript = FindObjectOfType<PauseInGame>();
        if(punchAireForceActivate == true)
        sliderAireForce.gameObject.SetActive(true);
        //ballBehavior = FindObjectOfType<BallBehavior>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        RaycastHit hit;
        int layerCell = 8;
        LayerMask layerMaskCell = 1 << layerCell;
        int layerBall = 9;
        LayerMask layerMaskBall = 1 << layerBall;
        if (punchAireForceActivate == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                sliderAireForce.value--;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                sliderAireForce.value++;
            }

            punchArea = Mathf.RoundToInt(sliderAireForce.value);
        }
        //profondeur = sliderProfondeur.value;


        if (pauseScript.isActive == false)
        {
            /*if (Input.GetMouseButtonDown(0))
            {
                holdMouseButton = true;
                choosedTool = Vector3.down;
                choosedReaction = Vector3.up;
            }*/






            if (Input.GetMouseButtonDown(0))
            {
                holdMouseButton = true;
                choosedTool = Vector3.up;
                choosedReaction = Vector3.down;
            }
            if (Physics.Raycast(cameraCenter, transform.forward, out hit, rangeBallTool, layerMaskBall))
            {
                StartCoroutine(hit.collider.GetComponent<BallBehavior>().ChangeMaterialHighlight());
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 direction = (hit.collider.transform.position - transform.position).normalized;
                    hit.collider.GetComponent<BallBehavior>().ShootOnBall(direction, forceShootBall,transform.position);
                    holdMouseButton = false;
                }
            }
            else
            {
                /// PARTIE FEEDBACK
                /// 
                if (Physics.Raycast(cameraCenter, transform.forward, out hit, rangeEarthTool, layerMaskCell))
                {
                    //if (hit.collider.gameObject.GetComponent<Renderer>().material.color == Color.white)

                    //if (CheckCellFeedBackValidity(hit.collider.gameObject) == true)
                    //{
                    if(hit.collider.gameObject.GetComponent<CellTwo>()._imMoving == false)
                        StartCoroutine(hit.collider.gameObject.GetComponent<CellTwo>().ChangeColor());
                        //lastFeedBackCells.Add(hit.collider.gameObject);
                        //lastTargetedFeedbackCell = hit.collider.gameObject;
                    //}
                    

                    if (punchAireForceActivate)
                    {
                        if (punchArea >= 1)
                        {
                            for (int h = 1; h <= punchArea; h++)
                            {
                                // pour chaque cube présent dans la scène
                                for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                                {
                                    // Ici on va calculer la distance du cube dans le tableau avec le cube centrale visé par le curseur, pour cela on compare en soustrayant leurs positions x entre elles et leurs positions z entre elles
                                    // On les additionne en valeur absolue pour toujours avoir un nombre toujours positif, pour obtenir la distance en cube avec le cube centrale et détecté qu'il va subir une force aussi.

                                    Vector3 hitVector = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
                                    Vector3 targetVector = new Vector3(worldGenerateObject.transform.GetChild(i).position.x, 0, worldGenerateObject.transform.GetChild(i).position.z);
                                    float distanceFromCenter = Vector3.Distance(hitVector, targetVector);
                                    
                                    
                                    if (distanceFromCenter < 1.6f * h && worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>()._imMoving == false)
                                    {
                                        
                                        //if (worldGenerateObject.transform.GetChild(i).GetComponent<Renderer>().material.color == Color.white)
                                            StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().ChangeColor());
                                            //lastFeedBackCells.Add(worldGenerateObject.transform.GetChild(i).gameObject);
                                    }
                                }
                            }
                        }
                    }
                    /// 
                    /// End PARTIE FEEDBACK




                    if (holdMouseButton == true)
                    {
                        
                        if (hit.collider.GetComponent<CellTwo>()._imMoving == false && hit.collider.gameObject != lastTargetedCell)
                        {
                            float hitPosX = hit.collider.transform.position.x;
                            float hitPosY = hit.collider.transform.position.y;
                            float hitPosZ = hit.collider.transform.position.z;

                            lastTargetedCell = hit.collider.transform.gameObject;
                            ballBehavior.currentCellTarget = hit.collider.transform;

                            if (punchArea >= 1)
                                StartCoroutine(hit.collider.GetComponent<CellTwo>().GetPunch(profondeur + punchArea - 1, speedScaleCellUp, choosedTool));
                            else
                                StartCoroutine(hit.collider.GetComponent<CellTwo>().GetPunch(profondeur + punchArea, speedScaleCellUp, choosedTool));

                            cellTargeted.Add(hit.collider.gameObject);
                            hit.collider.GetComponent<CellTwo>().EmittGrowSound();

                            // dans le cas ou l'on a une aire de force supérieur ou égale à 1
                            if (punchAireForceActivate == true)
                            {
                                if (punchArea >= 1)
                                {
                                    // pour chaque strate du cube central visé, la première est représenté avec 4 cube, la deuxième 9 cube, la troisième 12 etc...
                                    for (int h = 1; h <= punchArea; h++)
                                    {
                                        // pour chaque cube présent dans la scène
                                        for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                                        {

                                            if (worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>()._imMoving == false)
                                            {
                                                // Ici on va calculer la distance du cube dans le tableau avec le cube centrale visé par le curseur, pour cela on compare en soustrayant leurs positions x entre elles et leurs positions z entre elles
                                                // On les additionne en valeur absolue pour toujours avoir un nombre toujours positif, pour obtenir la distance en cube avec le cube centrale et détecté qu'il va subir une force aussi.
                                                Vector3 hitVector = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
                                                Vector3 targetVector = new Vector3(worldGenerateObject.transform.GetChild(i).position.x, 0, worldGenerateObject.transform.GetChild(i).position.z);
                                                float distanceFromCenter = Vector3.Distance(hitVector, targetVector);


                                                if (distanceFromCenter < 1.6f * h)
                                                {

                                                    // on calcule comment on va diviser la force pour avoir un effet sismique
                                                    float roundFloat = RoundValue(profondeur / (punchArea + 1), 100);
                                                    //Debug.Log(roundFloat);
                                                    float modifiedStrength = Mathf.Abs(roundFloat * ((punchArea + 1) - distanceFromCenter));
                                                    //Debug.Log(modifiedStrength);

                                                    if (forceUniform == false)
                                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().GetPunch(modifiedStrength, speedScaleCellUp, choosedTool));
                                                    else
                                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().GetPunch(profondeur + punchArea - h, speedScaleCellUp, choosedTool));

                                                    //cellTargeted.Add(worldGenerateObject.transform.GetChild(i).gameObject);


                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        
                    }


                }
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    holdMouseButton = false;
                    lastTargetedCell = null;
                }
                if(Input.GetMouseButton(0)== false)
                {
                    holdMouseButton = false;
                    lastTargetedCell = null;
                }

            }
        }
    }

    public bool CheckCellPosValidity(int randomNumber)
    {
        for (int i = 0; i < cellTargeted.Count; i++)
        {
            if (worldGenerateObject.transform.GetChild(randomNumber).gameObject == cellTargeted[i])
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckCellFeedBackValidity(GameObject cell)
    {
        for (int i = 0; i < lastFeedBackCells.Count; i++)
        {
            if (worldGenerateObject.transform.GetChild(i).gameObject == cell)
            {
                return false;
            }
        }
        return true;
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
}
