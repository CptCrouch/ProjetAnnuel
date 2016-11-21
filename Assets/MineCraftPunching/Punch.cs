using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour {
    [SerializeField]
    private float rangePunch= 10f;
    [SerializeField,Range(0, 1)]
    private float strengh = 1f;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private GameObject worldGenerateObject;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cameraCenter =Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        RaycastHit hit;
        /*int layer = 8;
        LayerMask layerMask = 1 << layer;*/
        if (Physics.Raycast(cameraCenter, transform.forward, out hit, rangePunch/*, layerMask*/))
        {

            StartCoroutine(hit.collider.gameObject.GetComponent<Cell>().ChangeColor());
            if(Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.GetComponent<Cell>()._imMoving == false)
                {
                    for (int i = 0; i < worldGenerateObject.transform.childCount; i++)
                    {
                        if (hit.collider.gameObject.GetComponent<Cell>()._myPosX == worldGenerateObject.transform.GetChild(i).position.x)
                        {
                            if (hit.collider.gameObject.GetComponent<Cell>()._myPosZ == worldGenerateObject.transform.GetChild(i).position.z)
                            {
                                StartCoroutine(worldGenerateObject.transform.GetChild(i).GetComponent<Cell>().GetPunch(strengh, speed, Vector3.down));
                            }
                        }
                    }
                }
                int randomCell = Random.Range(0, worldGenerateObject.transform.childCount);
                while(worldGenerateObject.transform.GetChild(randomCell).GetComponent<Cell>()._myPosX == hit.collider.gameObject.GetComponent<Cell>()._myPosX 
                    && worldGenerateObject.transform.GetChild(randomCell).GetComponent<Cell>()._myPosZ == hit.collider.gameObject.GetComponent<Cell>()._myPosZ)
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
            }
              
        }
    }
    
}
