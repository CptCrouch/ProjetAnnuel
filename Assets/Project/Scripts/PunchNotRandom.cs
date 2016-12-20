﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PunchNotRandom : MonoBehaviour {

    [SerializeField]
    private float rangeEarthTool = 10f;
    [SerializeField]
    private float rangeBallTool = 10f;
    [SerializeField, Range(0, 3)]
    private float profondeur = 1f;
    [SerializeField, Range(0, 4)]
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
    private bool feedBackAireForce = true;


    private PauseInGame pauseScript;
    private bool holdMouseButton;
    private GameObject lastTargetedCell;
    private Vector3 choosedTool;
    private Vector3 choosedReaction;

    private List<GameObject> cellTargeted = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        pauseScript = FindObjectOfType<PauseInGame>();
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
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            sliderAireForce.value--;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            sliderAireForce.value++;
        }

        punchArea = Mathf.RoundToInt(sliderAireForce.value);
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
                if (Physics.Raycast(cameraCenter, transform.forward, out hit, rangeEarthTool, layerMaskCell))
                {
                    //if (hit.collider.gameObject.GetComponent<Renderer>().material.color == Color.white)
                        StartCoroutine(hit.collider.gameObject.GetComponent<Cell>().ChangeColor());

                    if (feedBackAireForce)
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
                                    float distanceFromCenter = Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.x - hit.collider.transform.position.x) + Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.z - hit.collider.transform.position.z);
                                    //Debug.Log(distanceFromCenter);
                                    if (distanceFromCenter == h)
                                    {
                                        //if (worldGenerateObject.transform.GetChild(i).GetComponent<Renderer>().material.color == Color.white)
                                            StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().ChangeColor());
                                    }
                                }
                            }
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


                            StartCoroutine(hit.collider.transform.GetComponent<Cell>().GetPunchScale(profondeur + punchArea, speedScaleCellUp, choosedTool, false));
                            cellTargeted.Add(hit.collider.gameObject);

                            // dans le cas ou l'on a une aire de force supérieur ou égale à 1
                            if (punchArea >= 1)
                            {
                                // pour chaque strate du cube central visé, la première est représenté avec 4 cube, la deuxième 9 cube, la troisième 12 etc...
                                for (int h = 1; h <= punchArea; h++)
                                {
                                    // pour chaque cube présent dans la scène
                                    for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                                    {
                                        // Ici on va calculer la distance du cube dans le tableau avec le cube centrale visé par le curseur, pour cela on compare en soustrayant leurs positions x entre elles et leurs positions z entre elles
                                        // On les additionne en valeur absolue pour toujours avoir un nombre toujours positif, pour obtenir la distance en cube avec le cube centrale et détecté qu'il va subir une force aussi.
                                        float distanceFromCenter = Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.x - hitPosX) + Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.z - hitPosZ);
                                        //Debug.Log(distanceFromCenter);
                                        if (distanceFromCenter == h)
                                        {

                                            // on calcule comment on va diviser la force pour avoir un effet sismique
                                            float roundFloat = RoundValue(profondeur / (punchArea + 1), 100);
                                            //Debug.Log(roundFloat);
                                            float modifiedStrength = Mathf.Abs(roundFloat * ((punchArea + 1) - distanceFromCenter));
                                            Debug.Log(modifiedStrength);

                                            if (forceUniform == false)
                                                StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunchScale(modifiedStrength, speedScaleCellUp, choosedTool, false));
                                            else
                                                StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunchScale(profondeur + punchArea - h, speedScaleCellUp, choosedTool, false));

                                            cellTargeted.Add(worldGenerateObject.transform.GetChild(i).gameObject);


                                        }
                                    }
                                }

                            }




                            /// REAction
                            /// 
                            ///
                            /*List<GameObject> cellToReactWith = new List<GameObject>();
                            for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                            {
                                    if(worldGenerateObject.transform.GetChild(i).localScale.y <= -(hit.collider.transform.localScale.y-worldGenerateObject.GetComponent<WorldGenerate>().height)+0.20f 
                                        && worldGenerateObject.transform.GetChild(i).localScale.y >= -(hit.collider.transform.localScale.y - worldGenerateObject.GetComponent<WorldGenerate>().height) - 0.20f)
                                    {
                                        cellToReactWith.Add(worldGenerateObject.transform.GetChild(i).gameObject);
                                    }

                            }
                            Debug.Log(cellToReactWith.Count);

                            int randomCell = Random.Range(0, worldGenerateObject.transform.childCount);

                            //Si false alors on recalcule un autre numero random
                            while (CheckCellPosValidity(randomCell) == false)
                            {
                                randomCell = Random.Range(0, cellToReactWith.Count);
                            }


                            Cell cell = worldGenerateObject.transform.GetChild(randomCell).GetComponent<Cell>();
                            for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                            {
                                if (cell.transform.position.x == worldGenerateObject.transform.GetChild(i).position.x)
                                {
                                    if (cell.transform.position.z == worldGenerateObject.transform.GetChild(i).position.z)
                                    {

                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunchScale(profondeur+punchArea, speed, choosedReaction,true));

                                    }
                                }
                            }
                            // dans le cas ou l'on a une aire de force supérieur ou égale à 1
                            if (punchArea >= 1)
                            {

                                // pour chaque strate du cube central visé, la première est représenté avec 4 cube, la deuxième 9 cube, la troisième 12 etc...
                                for (int h = 1; h <= punchArea; h++)
                                {
                                    // pour chaque cube présent dans la scène
                                    for (int j = 0; j < worldGenerateObject.transform.childCount; j++)
                                    {
                                        // Ici on va calculer la distance du cube dans le tableau avec le cube centrale visé par le curseur, pour cela on compare en soustrayant leurs positions x entre elles et leurs positions z entre elles
                                        // On les additionne en valeur absolue pour toujours avoir un nombre toujours positif, pour obtenir la distance en cube avec le cube centrale et détecté qu'il va subir une force aussi.
                                        if (CheckCellPosValidity(j) == true)
                                        {
                                            float distanceFromCenter = Mathf.Abs(worldGenerateObject.transform.GetChild(j).position.x - cell.transform.position.x) + Mathf.Abs(worldGenerateObject.transform.GetChild(j).position.z - cell.transform.position.z);
                                            if (distanceFromCenter == h)
                                            {
                                                // on calcule comment on va diviser la force pour avoir un effet sismique
                                                float roundFloat = RoundValue(profondeur / (punchArea + 1), 100);

                                                float modifiedStrength = Mathf.Abs(roundFloat * ((punchArea + 1) - distanceFromCenter));
                                                //Debug.Log("pouet");
                                                if (forceUniform == false)
                                                    StartCoroutine(worldGenerateObject.transform.GetChild(j).GetComponent<Cell>().GetPunchScale(modifiedStrength, speed, choosedReaction, true));
                                                else
                                                    StartCoroutine(worldGenerateObject.transform.GetChild(j).GetComponent<Cell>().GetPunchScale(profondeur+punchArea-h, speed, choosedReaction, true));


                                            }
                                        }
                                    }
                                }

                            }
                            cellTargeted.Clear();

                            //sliderProfondeur.value = 0;
                            //profondeur = 0;*/
                        }
                    }


                }
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
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