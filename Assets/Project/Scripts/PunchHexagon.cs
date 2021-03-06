﻿using UnityEngine;
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
    private int punchArea = 0;
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
    public DestructionBehavior destroyManager;
    [SerializeField]
    public bool isWithPinceau;

    [Space(10)]
    [Header("[ SnowBall Altitude ]")]
    public int altitudeToForce1;
    public int altitudeToForce2;
    public int altitudeToForce3;

    public Image[] allFeedbackImage;


    private PauseInGame pauseScript;
    
    private bool holdMouseButtonLeft;
    private bool holdMouseButtonRight;
    private GameObject lastTargetedCell;
    private GameObject lastTargetedFeedbackCell;
    private List<GameObject> lastFeedBackCells = new List<GameObject>();
    private Vector3 choosedTool;
    private Vector3 choosedReaction;

    private List<GameObject> cellTargeted = new List<GameObject>();
    private int currentGeneralAltitude;
    // Use this for initialization
    void Start()
    {
        pauseScript = FindObjectOfType<PauseInGame>();

        if (punchAireForceActivate == false)
        {
            ChooseWhichFeedback(punchArea, punchAireForceActivate);
        }
        //ballBehavior = FindObjectOfType<BallBehavior>();
    }


    // Update is called once per frame
    void Update()
    {
        currentGeneralAltitude = GetGeneralAltitude();
        //Debug.Log(currentGeneralAltitude);
        /*if(currentGeneralAltitude > altitudeToForce1 && currentGeneralAltitude < altitudeToForce2)
        {
            punchArea = 1;
        }
        else if(currentGeneralAltitude > altitudeToForce2 && currentGeneralAltitude < altitudeToForce3)
        {
            punchArea = 2;
        }
        else if (currentGeneralAltitude > altitudeToForce3)
        {
            punchArea = 3;
        }
        else
        {
            punchArea = 0;
        }*/
        sliderAireForce.value = punchArea;
        if (punchAireForceActivate == true)
        {
            ChooseWhichFeedback(punchArea,punchAireForceActivate);
        }

        //Vector3 cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerCell = 8;
        LayerMask layerMaskCell = 1 << layerCell;
        int layerBall = 9;
        LayerMask layerMaskBall = 1 << layerBall;
        if (punchAireForceActivate == true)
        {
            /*if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                sliderAireForce.value--;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                sliderAireForce.value++;
            }*/

            //punchArea = Mathf.RoundToInt(sliderAireForce.value);
        }
        


        if (pauseScript.isActive == false)
        {
            if (isWithPinceau == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    holdMouseButtonLeft = true;
                    choosedTool = Vector3.up;
                    choosedReaction = Vector3.down;
                }
               
            }
            if (Physics.Raycast(ray, out hit, rangeBallTool, layerMaskBall))
            {
                /*StartCoroutine(hit.collider.GetComponent<BallBehavior>().ChangeMaterialHighlight());
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 direction = (hit.collider.transform.position - transform.position).normalized;
                    hit.collider.GetComponent<BallBehavior>().ShootOnBall(direction, forceShootBall,transform.position);
                    holdMouseButton = false;
                }*/
            }
            else
            {
                #region FeedBack
                 
                if (Physics.Raycast(ray, out hit, rangeEarthTool, layerMaskCell))
                {
                   
                    if(hit.collider.gameObject.GetComponent<CellTwo>()._imMoving == false)
                        StartCoroutine(hit.collider.gameObject.GetComponent<CellTwo>().ChangeColor());

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
                                           
                                         StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().ChangeColor());
                                      
                                    }
                                }
                            }
                        }
                    }
                   
                    #endregion

                    
                    #region Elevation Terrain
                    if (isWithPinceau == true && holdMouseButtonLeft == true)
                    {

                        CellTwo cellHit = hit.collider.GetComponent<CellTwo>();


                        if (cellHit._imMoving == false && hit.collider.gameObject != lastTargetedCell)
                        {
                            float hitPosX = hit.collider.transform.position.x;
                            float hitPosY = hit.collider.transform.position.y;
                            float hitPosZ = hit.collider.transform.position.z;

                            lastTargetedCell = hit.collider.transform.gameObject;

                            
                            destroyManager.listOfCellOnStart.Remove(cellHit);
                            
                            

                            if (punchArea >= 1)
                                StartCoroutine(cellHit.GetPunch(profondeur + punchArea - 1, speedScaleCellUp, choosedTool,true));
                            else
                                StartCoroutine(cellHit.GetPunch(profondeur + punchArea, speedScaleCellUp, choosedTool,true));

                            cellTargeted.Add(hit.collider.gameObject);
                            //cellHit.EmittGrowSound();

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
                                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().GetPunch(modifiedStrength, speedScaleCellUp, choosedTool,true));
                                                    else
                                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().GetPunch(profondeur + punchArea - h, speedScaleCellUp, choosedTool,true));

                                                    //cellTargeted.Add(worldGenerateObject.transform.GetChild(i).gameObject);
                                                    destroyManager.listOfCellOnStart.Remove(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>());


                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        
                    }
                    
                    else if (Input.GetMouseButtonDown(0))
                    {
                        choosedTool = Vector3.up;

                        CellTwo cellHit = hit.collider.GetComponent<CellTwo>();


                        if (cellHit._imMoving == false && hit.collider.gameObject != lastTargetedCell)
                        {
                            float hitPosX = hit.collider.transform.position.x;
                            float hitPosY = hit.collider.transform.position.y;
                            float hitPosZ = hit.collider.transform.position.z;

                            lastTargetedCell = hit.collider.transform.gameObject;

                            
                            destroyManager.listOfCellOnStart.Remove(cellHit);

                            if (punchArea >= 1)
                                StartCoroutine(cellHit.GetPunch(profondeur + punchArea - 1, speedScaleCellUp, choosedTool,true));
                            else
                                StartCoroutine(cellHit.GetPunch(profondeur + punchArea, speedScaleCellUp, choosedTool,true));

                            cellTargeted.Add(hit.collider.gameObject);
                            //cellHit.EmittGrowSound();

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
                                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().GetPunch(modifiedStrength, speedScaleCellUp, choosedTool,true));
                                                    else
                                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().GetPunch(profondeur + punchArea - h, speedScaleCellUp, choosedTool,true));

                                                    destroyManager.listOfCellOnStart.Remove(worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>());

                                                    //cellTargeted.Add(worldGenerateObject.transform.GetChild(i).gameObject);


                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    #endregion

                    #region TargetBall
                    /*if (isWithPinceau == true && holdMouseButtonRight == true)
                    {
                        ballBehavior.currentCellTarget = hit.collider.transform;
                        ballBehavior.currentCellTarget.GetComponent<CellTwo>().imTargeted = true;

                        if (ballBehavior.currentSpeed < ballBehavior.minimumSpeedToAutoMove)
                            ballBehavior.speedToUse = ballBehavior.minimumSpeedToAutoMove;
                        else
                            ballBehavior.speedToUse = ballBehavior.currentSpeed;
                    }*/
                    #endregion

                }
                if (Input.GetMouseButtonUp(0) )
                {
                    holdMouseButtonLeft = false;
                    lastTargetedCell = null;
                }
                if(Input.GetMouseButtonUp(1))
                {
                    holdMouseButtonRight = false;
                }
                if(Input.GetMouseButton(0) == false)
                {
                    holdMouseButtonLeft = false;
                    lastTargetedCell = null;
                }

            }
        }
    }

    

    void ChooseWhichFeedback(int index, bool isActivate)
    {
        if (isActivate == true)
        {
            for (int i = 0; i < allFeedbackImage.Length; i++)
            {
                if (i == index)
                {
                    allFeedbackImage[i].enabled = true;
                }
                else
                {
                    allFeedbackImage[i].enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < allFeedbackImage.Length; i++)
            {
                allFeedbackImage[i].enabled = false;
            }
        }
    }
    public int GetGeneralAltitude()
    {
        int altitude =0;
        for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
        {
            int altCell = worldGenerateObject.transform.GetChild(i).GetComponent<CellTwo>().currentAltitude;
            altitude = altitude + altCell;
        }
        return altitude;
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
