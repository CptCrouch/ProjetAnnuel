using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public InteractableObject currentObject;
    public void RotateRight()
    {
        //StartCoroutine(currentObject.RotatingObject(new Quaternion(currentObject.transform.rotation.x, currentObject.transform.rotation.y - 90, currentObject.transform.rotation.z, currentObject.transform.rotation.w), "Right"));
        //StartCoroutine(currentObject.RotatingObject("Right"));
        currentObject.RotateSimple("Right");

    }
    public void RotateLeft()
    {
        //StartCoroutine(currentObject.RotatingObject(new Quaternion(currentObject.transform.rotation.x, currentObject.transform.rotation.y + 90, currentObject.transform.rotation.z, currentObject.transform.rotation.w), "Left"));
        //StartCoroutine(currentObject.RotatingObject("Left"));
        currentObject.RotateSimple("Left");
    }
    public void RotateUp()
    {
        //StartCoroutine(currentObject.RotatingObject(new Quaternion(currentObject.transform.rotation.x+90, currentObject.transform.rotation.y , currentObject.transform.rotation.z, currentObject.transform.rotation.w), "Up"));
        //StartCoroutine(currentObject.RotatingObject( "Up"));

        currentObject.RotateSimple("Up");
    }
    public void RotateDown()
    {
        //StartCoroutine(currentObject.RotatingObject(new Quaternion(currentObject.transform.rotation.x-90, currentObject.transform.rotation.y , currentObject.transform.rotation.z, currentObject.transform.rotation.w), "Down"));
        //StartCoroutine(currentObject.RotatingObject( "Down"));
        currentObject.RotateSimple("Down");
    }
    void Update()
    {
        //Debug.Log(Mathf.Round(currentObject.transform.eulerAngles.x));
    }
}
