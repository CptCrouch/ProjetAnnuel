using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CloudPlateformer : MonoBehaviour { 



    [SerializeField]
    private GameObject worldGenerateObject;
    
    private PlayerOnWire player;

    private PauseInGame pauseScript;

    public List<Vector3> posBorder = new List<Vector3>();

    


    void Start()
    {
        pauseScript = FindObjectOfType<PauseInGame>();
        player = FindObjectOfType<PlayerOnWire>();
    }


    // Update is called once per frame
    void Update()
    {
        if (pauseScript.isActive == false)
        {
            //float axeX = Input.GetAxis("Horizontal");
            //float axez = Input.GetAxis("Vertical");

            if (player.imMoving == false)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    StartCoroutine(player.MoveOnWire(Vector3.right));
                    
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StartCoroutine(player.MoveOnWire(Vector3.left));
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine(player.MoveOnWire(Vector3.forward));
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    StartCoroutine(player.MoveOnWire(Vector3.back));
                }
            }




        }
    }

    public void MoveOnBorder(GameObject cellMoved)
    {
        float hitPosX = cellMoved.transform.position.x;
        float hitPosY = cellMoved.transform.position.y;
        float hitPosZ = cellMoved.transform.position.z;


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
        cellMoved.GetComponent<Cell>().GoOnBorder(posBorder[random]);

        posBorder.Clear();
    }

    bool CheckPosValidity(Vector3 posTested)
    {
        for (int i = 0; i < posBorder.Count; i++)
        {
            if (posBorder[i] == posTested)
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
            if (cell.voisinNord == null)
            {
                Vector3 posNord = new Vector3(cell.transform.position.x, cell.transform.position.y, cell.transform.position.z + 1);
                if (CheckPosValidity(posNord) == true)
                    posBorder.Add(posNord);

            }
            if (cell.voisinSud == null)
            {
                Vector3 posSud = new Vector3(cell.transform.position.x, cell.transform.position.y, cell.transform.position.z - 1);
                if (CheckPosValidity(posSud) == true)
                    posBorder.Add(posSud);

            }
            if (cell.voisinOuest == null)
            {
                Vector3 posOuest = new Vector3(cell.transform.position.x - 1, cell.transform.position.y, cell.transform.position.z);
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
}
