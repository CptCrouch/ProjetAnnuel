using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Punch : MonoBehaviour {
    [SerializeField]
    private float rangePunch= 10f;
    [SerializeField,Range(0, 3)]
    private float strengh = 1f;
    [SerializeField, Range(0, 3)]
    private int punchArea = 1;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private GameObject worldGenerateObject;
    [SerializeField]
    private Slider sliderAireForce;
    [SerializeField]
    private float speedSliderBar=10f;

    private PauseInGame pauseScript;

    private List<GameObject> cellTargeted = new List<GameObject>();
    // Use this for initialization
    void Start () {
        pauseScript = FindObjectOfType<PauseInGame>();
	}
    

    // Update is called once per frame
    void Update () {
        Vector3 cameraCenter =Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        RaycastHit hit;
        /*int layer = 8;
        LayerMask layerMask = 1 << layer;*/
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            sliderAireForce.value --;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            sliderAireForce.value ++;
        }

        punchArea = Mathf.RoundToInt(sliderAireForce.value);

        if (pauseScript.isActive == false)
        {
            if (Physics.Raycast(cameraCenter, transform.forward, out hit, rangePunch/*, layerMask*/))
            {

                StartCoroutine(hit.collider.gameObject.GetComponent<Cell>().ChangeColor());
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.gameObject.GetComponent<Cell>()._imMoving == false)
                    {
                        float hitPosX = hit.collider.gameObject.GetComponent<Cell>()._myPosX;
                        float hitPosY = hit.collider.gameObject.GetComponent<Cell>()._myPosY;
                        float hitPosZ = hit.collider.gameObject.GetComponent<Cell>()._myPosZ;


                        for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                        {
                            if (hitPosX == worldGenerateObject.transform.GetChild(i).position.x)
                            {
                                if (hitPosZ == worldGenerateObject.transform.GetChild(i).position.z)
                                {
                                    StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunch(strengh, speed, Vector3.down));
                                    cellTargeted.Add(hit.collider.gameObject);
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
                                for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                                {
                                    // Ici on va calculer la distance du cube dans le tableau avec le cube centrale visé par le curseur, pour cela on compare en soustrayant leurs positions x entre elles et leurs positions z entre elles
                                    // On les additionne en valeur absolue pour toujours avoir un nombre toujours positif, pour obtenir la distance en cube avec le cube centrale et détecté qu'il va subir une force aussi.
                                    float distanceFromCenter = Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.x - hitPosX) + Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.z - hitPosZ);
                                    if (distanceFromCenter == h)
                                    {
                                        // on calcule comment on va diviser la force pour avoir un effet sismique
                                        float roundFloat = RoundValue(strengh / (punchArea + 1), 100);
                                        //Debug.Log(roundFloat);
                                        float modifiedStrength = Mathf.Abs(roundFloat * ((punchArea + 1) - distanceFromCenter));
                                        Debug.Log(modifiedStrength);


                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunch(modifiedStrength, speed, Vector3.down));
                                        cellTargeted.Add(worldGenerateObject.transform.GetChild(i).gameObject);
                                    }
                                }
                            }

                        }
                    }



                    /// REMONTEE
                    /// 
                    ///
                    int randomCell = Random.Range(0, worldGenerateObject.transform.childCount);

                    //Si false alors on recalcule un autre numero random
                    while (CheckCellPosValidity(randomCell) == false)
                    {
                        randomCell = Random.Range(0, worldGenerateObject.transform.childCount);
                    }


                    Cell cell = worldGenerateObject.transform.GetChild(randomCell).GetComponent<Cell>();
                    for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                    {
                        if (cell._myPosX == worldGenerateObject.transform.GetChild(i).position.x)
                        {
                            if (cell._myPosZ == worldGenerateObject.transform.GetChild(i).position.z)
                            {
                                StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunch(strengh, speed, Vector3.up));
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
                            for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                            {
                                // Ici on va calculer la distance du cube dans le tableau avec le cube centrale visé par le curseur, pour cela on compare en soustrayant leurs positions x entre elles et leurs positions z entre elles
                                // On les additionne en valeur absolue pour toujours avoir un nombre toujours positif, pour obtenir la distance en cube avec le cube centrale et détecté qu'il va subir une force aussi.
                                if (CheckCellPosValidity(i) == true)
                                {
                                    float distanceFromCenter = Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.x - cell._myPosX) + Mathf.Abs(worldGenerateObject.transform.GetChild(i).position.z - cell._myPosZ);
                                    if (distanceFromCenter == h)
                                    {
                                        // on calcule comment on va diviser la force pour avoir un effet sismique
                                        float roundFloat = RoundValue(strengh / (punchArea + 1), 100);

                                        float modifiedStrength = Mathf.Abs(roundFloat * ((punchArea + 1) - distanceFromCenter));

                                        StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunch(modifiedStrength, speed, Vector3.up));
                                    }
                                }
                            }
                        }

                    }
                    cellTargeted.Clear();
                }

            }
        }
    }
    public bool CheckCellPosValidity(int randomNumber)
    {
        for (int i = 0; i < cellTargeted.Count; i++)
        {
            if(worldGenerateObject.transform.GetChild(randomNumber).gameObject == cellTargeted[i])
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
