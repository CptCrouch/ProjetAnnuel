using UnityEngine;
using System.Collections;

public class PlayerOnWire : MonoBehaviour {
    public float speedMove = 5f;
    public CloudPlateformer cloudPlateformer;
    [HideInInspector]
    public bool imMoving = false;
    [HideInInspector]
    public GameObject currentCellImOn;
    
    void Start()
    {
        //transform.position = new Vector3()
    }

	
    public IEnumerator MoveOnWire(Vector3 direction)
    {
        Vector3 startPos = transform.position;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.down,out hit,1f))
        {
            currentCellImOn = hit.collider.gameObject;
        }

        imMoving = true;
        if (direction == Vector3.right)
        {
            while (transform.position.x <= startPos.x + 1)
            {
                Debug.Log("pouet");
                transform.Translate(direction * Time.deltaTime * speedMove);
                yield return null;
            }
            transform.position = new Vector3(startPos.x + 1, startPos.y, startPos.z);
        }
        if (direction == Vector3.left)
        {
            while (transform.position.x >= startPos.x - 1)
            {

                transform.Translate(direction * Time.deltaTime * speedMove);
                yield return null;
            }
            transform.position = new Vector3(startPos.x - 1, startPos.y, startPos.z);
        }
        if (direction == Vector3.forward)
        {
            while (transform.position.z <= startPos.z +1)
            {

                transform.Translate(direction * Time.deltaTime * speedMove);
                yield return null;
            }
            transform.position = new Vector3(startPos.x, startPos.y, startPos.z + 1);
        }
        if (direction == Vector3.back)
        {
            while (transform.position.z >= startPos.z - 1)
            {

                transform.Translate(direction * Time.deltaTime * speedMove);
                yield return null;
            }
            transform.position = new Vector3(startPos.x, startPos.y, startPos.z - 1);
        }
        imMoving = false;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f) && currentCellImOn != null)
            cloudPlateformer.MoveOnBorder(currentCellImOn);
        else
            currentCellImOn = null;
    }
}
