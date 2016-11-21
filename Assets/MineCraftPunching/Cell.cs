using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
    public float _myPosX;
    public float _myPosY;
    public float _myPosZ;

    public bool _imMoving;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _myPosX = transform.position.x;
        _myPosY = transform.position.y;
        _myPosZ = transform.position.z;
	
	}
    public IEnumerator ChangeColor()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForEndOfFrame();
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
    public IEnumerator GetPunch(float strength,float speed,Vector3 direction)
    {
        Vector3 firstPos = transform.position;
        _imMoving = true;
        if (direction.y < 0)
        {
            while (_myPosY > firstPos.y + (strength * direction.y))
            {
                transform.Translate(direction * Time.deltaTime * speed);
                yield return null;
            }
        }
        else if (direction.y>0)
        {
            while (_myPosY < firstPos.y + (strength * direction.y))
            {
                transform.Translate(direction * Time.deltaTime * speed);
                yield return null;
            }
        }
        _imMoving = false;

    }
    

        
}
