using UnityEngine;
using System.Collections;

public class RotateWithMouse : MonoBehaviour {

    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;
    public float deadZoneMouse = 0.1f;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.Rotate(v, -h, 0);
            Debug.Log(h + " // " + v);
        }
        /*if (Input.GetMouseButton(0))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse Y");
            float v = verticalSpeed * Input.GetAxis("Mouse X");

            if (h > deadZoneMouse)
                transform.Rotate(horizontalSpeed, 0, 0);
            if (h < -deadZoneMouse)
                transform.Rotate(-horizontalSpeed, 0, 0);
            if (v > deadZoneMouse)
                transform.Rotate(0, verticalSpeed, 0);
            if (v < -deadZoneMouse)
                transform.Rotate(0, -verticalSpeed, 0);

            Debug.Log(h + " // " + v);
        }*/
        /*if (Input.GetMouseButton(0))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse Y");
            float v = verticalSpeed * Input.GetAxis("Mouse X");


            if (h > deadZoneMouse)
            {
                float angle = Mathf.Atan2(v, -h) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            }
            if (h < -deadZoneMouse)
            {
                float angle = Mathf.Atan2(v, -h) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            }
            if (v > deadZoneMouse)
            {
                float angle = Mathf.Atan2(v, -h) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
            }
            if (v < -deadZoneMouse)
            {
                float angle = Mathf.Atan2(v, -h) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
            }

            Debug.Log(h + " // " + v);


        }*/
        
    }
}
