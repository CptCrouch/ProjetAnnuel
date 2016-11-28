using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
    

    public bool _imMoving;

    private Vector3 lastScale;
    
	
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
            while (transform.position.y >= firstPos.y + (strength * direction.y))
            {
                transform.Translate(direction * Time.deltaTime * speed);
                yield return null;
            }
        }
        else if (direction.y>0)
        {
            while (transform.position.y <= firstPos.y + (strength * direction.y))
            {
                transform.Translate(direction * Time.deltaTime * speed);
                yield return null;
            }
        }
        _imMoving = false;

    }
    public IEnumerator GetPunchScale(float strength, float speed, Vector3 direction, bool isBlue)
    {
        Vector3 firstScale = transform.localScale;
        
        _imMoving = true;
        if(isBlue)
        GetComponent<Renderer>().material.color = Color.blue;
        else
        GetComponent<Renderer>().material.color = Color.red;

        if (direction.y < 0)
        {
            while (transform.localScale.y >= firstScale.y + (strength * direction.y) && transform.localScale.y >=1)
            {
                
                transform.localScale += new Vector3(0, -speed * Time.deltaTime, 0);
               
                //transform.localPosition += direction * speed / 2 * Time.deltaTime;
                transform.Translate(direction * Time.deltaTime * (speed / 2));
                //Debug.Log("pouet");
                yield return null;
            }
            //transform.localScale = new Vector3 (transform.localScale.x, firstScale.y + (strength * direction.y), transform.localScale.z);
            
        }
        else if (direction.y > 0)
        {
            while (transform.localScale.y <= firstScale.y + (strength * direction.y))
            {
                transform.localScale += new Vector3(0, speed * Time.deltaTime, 0);
                
                //transform.localPosition += direction * speed / 2 * Time.deltaTime;
                transform.Translate(direction * Time.deltaTime *( speed /2));
                
                yield return null;
            }
            //transform.localScale = new Vector3(transform.localScale.x, firstScale.y + (strength * direction.y), transform.localScale.z);
            
        }
        _imMoving = false;
        GetComponent<Renderer>().material.color = Color.white;



    }
    



}
