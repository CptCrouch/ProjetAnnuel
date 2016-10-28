using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour {

    public float speedRotate;
    public float intervalleDetection;
    public int upState;
    public int rightState;
	public IEnumerator RotatingObject(string direction)
    {
        
        Debug.Log("Object " + transform.rotation.x + "/" + transform.rotation.y + "/" + transform.rotation.z + "/" + transform.rotation.w);
        DisableButtons();
        //Debug.Log("Direction " + dirRot.x + "/" + dirRot.y + "/" + dirRot.z + "/" + dirRot.w);


        ////
        ///////
        //UP ROTATION
        ///////
        ////


        if (direction == "Up")
        {
            if (upState == 0)
            {
                while (Mathf.Round(transform.eulerAngles.x) != Mathf.Round(90))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.x));
                    
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }
                
            }
            if (upState == 1)
            {
                while (Mathf.Round(180-transform.eulerAngles.x) != Mathf.Round(180))
                {
                    //Debug.Log("1////" + Mathf.Round(180-transform.eulerAngles.x));
                   
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }
                
            }
            if (upState == 2)
            {
                while (Mathf.Round(360 - transform.eulerAngles.x) != Mathf.Round(90))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.x));
                    
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }
                
            }
            if (upState == 3)
            {
                while (Mathf.Round(transform.eulerAngles.x) != Mathf.Round(360))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.x));
                    
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }
                
            }
            
            upState++;
            if(upState > 3)
            {
                upState = 0;
            }
        }


        ////
        ///////
        //DOWN ROTATION
        ///////
        ////


    
        if (direction == "Down")
        {
            if (upState == 0 )
            {
                while (Mathf.Round(360-transform.eulerAngles.x) != Mathf.Round(90))
                {
                    Debug.Log("1////" + Mathf.Round(transform.eulerAngles.x));
                    
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(-0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }

            }
            if (upState == 3)
            {
                while (Mathf.Round(180 - transform.eulerAngles.x) != Mathf.Round(180))
                {
                    //Debug.Log("1////" + Mathf.Round(180-transform.eulerAngles.x));
                    
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(-0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }

            }
            if (upState == 2)
            {
                while (Mathf.Round(transform.eulerAngles.x) != Mathf.Round(90))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.x));
                   
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(-0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }

            }
            if (upState == 1)
            {
                while (Mathf.Round(transform.eulerAngles.x) != Mathf.Round(0))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.x));
                    
                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(-0.1f * Time.deltaTime * speedRotate, 0, 0);
                    yield return null;
                }

            }
            
            upState--;

            if (upState < 0)
                upState = 3;
        }


        ////
        ///////
        //LEFT ROTATION
        ///////
        ////


        if (direction == "Left")
        {
            if (rightState == 0)
            {
                while (Mathf.Round(transform.eulerAngles.y) != Mathf.Round(90))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, 0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }
            if (rightState == 1)
            {
                while (Mathf.Round(180 - transform.eulerAngles.y) != Mathf.Round(0))
                {
                    Debug.Log("1////" + Mathf.Round(180-transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, 0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }
            if (rightState == 2)
            {
                while (Mathf.Round(360 - transform.eulerAngles.y) != Mathf.Round(90))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, 0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }
            if (rightState == 3)
            {
                while (Mathf.Round(transform.eulerAngles.y) != Mathf.Round(0))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, 0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }

            rightState++;

            if (rightState > 3)
                rightState = 0;
        }


        ////
        ///////
        //RIGHT ROTATION
        ///////
        ////


        if (direction == "Right")
        {
            if (rightState == 0)
            {
                while (Mathf.Round(360-transform.eulerAngles.y) != Mathf.Round(90))
                {
                    Debug.Log("1////" + Mathf.Round(transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, -0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }
            if (rightState == 3)
            {
                while (Mathf.Round(180 - transform.eulerAngles.y) != Mathf.Round(0))
                {
                    //Debug.Log("1////" + Mathf.Round(180-transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, -0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }
            if (rightState == 2)
            {
                while (Mathf.Round(transform.eulerAngles.y) != Mathf.Round(90))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, -0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }
            if (rightState == 1)
            {
                while (Mathf.Round(transform.eulerAngles.y) != Mathf.Round(0))
                {
                    //Debug.Log("1////" + Mathf.Round(transform.eulerAngles.y));

                    //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x +1, transform.rotation.y, transform.rotation.z, transform.rotation.w), speedRotate * Time.deltaTime);
                    transform.Rotate(0, -0.1f * Time.deltaTime * speedRotate, 0);
                    yield return null;
                }

            }

            rightState--;

            if (rightState < 0)
                rightState = 3;
        }
        EnableButtons();
    }

    public void RotateSimple(string direction)
    {
        if(direction == "Up")
        {
            transform.Rotate(Vector3.right, 90,Space.World);
        }
        if (direction == "Down")
        {
            transform.Rotate(Vector3.right, -90, Space.World);
        }
        if (direction == "Right")
        {
            transform.Rotate(Vector3.up,-90, Space.World);
        }
        if (direction == "Left")
        {
            transform.Rotate(Vector3.up, 90, Space.World);
        }

    }
    public void DisableButtons()
    {
        Button[] getAllButton = FindObjectsOfType<Button>();
        for (int i = 0; i < getAllButton.Length; i++)
        {
            getAllButton[i].interactable = false;
        }
    }
    public void EnableButtons()
    {
        Button[] getAllButton = FindObjectsOfType<Button>();
        for (int i = 0; i < getAllButton.Length; i++)
        {
            getAllButton[i].interactable = true;
        }
    }
}
