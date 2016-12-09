﻿using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
    

    public bool _imMoving;
    public bool _imReturningStartPos;
    public bool imAtStartPos = true;

    private Vector3 lastScale;
    private float ecartAvecStartPosY = 0;

    public GameObject voisinNord;
    public GameObject voisinSud;
    public GameObject voisinOuest;
    public GameObject voisinEst;

    [HideInInspector]
    public float startPosY;
    [HideInInspector]
    public float startScaleY;
   
    





    public IEnumerator ChangeColor()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForEndOfFrame();
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void GoOnBorder(Vector3 pos)
    {
        transform.position = pos;
        /*voisinEst = null;
        voisinNord = null;
        voisinSud = null;
        voisinOuest = null;*/
    }

    public void GetNeighbourHood()
    {
        RaycastHit hitNord;
        RaycastHit hitSud;
        RaycastHit hitOuest;
        RaycastHit hitEst;

        //if (voisinNord == null)
        //{
            if (Physics.Raycast(transform.position, Vector3.forward, out hitNord, 0.75f))
            {
                voisinNord = hitNord.collider.gameObject;
                /*if (voisinNord.GetComponent<Cell>().voisinSud == null)
                    voisinNord.GetComponent<Cell>().voisinSud = transform.gameObject;*/
            }
            else
            {
                voisinNord = null;
            }
        //}
        //if (voisinSud == null)
        //{
            if (Physics.Raycast(transform.position, Vector3.back, out hitSud, 0.75f))
            {
                voisinSud = hitSud.collider.gameObject;
                /*if (voisinSud.GetComponent<Cell>().voisinNord == null)
                    voisinSud.GetComponent<Cell>().voisinNord = transform.gameObject;*/

            }
            else
            {
                voisinSud = null;
            }
        //}
        //if (voisinOuest == null)
        //{
            if (Physics.Raycast(transform.position, Vector3.left, out hitOuest, 0.75f))
            {
                voisinOuest = hitOuest.collider.gameObject;
                /*if (voisinOuest.GetComponent<Cell>().voisinEst == null)
                    voisinOuest.GetComponent<Cell>().voisinEst = transform.gameObject;*/
            }
            else
            {
                voisinOuest = null;
            }
        //}
        //if (voisinEst == null)
        //{
            if (Physics.Raycast(transform.position, Vector3.right, out hitEst, 0.75f))
            {
                voisinEst = hitEst.collider.gameObject;
                /*if (voisinEst.GetComponent<Cell>().voisinOuest == null)
                    voisinEst.GetComponent<Cell>().voisinOuest = transform.gameObject;*/
            }
            else
            {
                voisinEst = null;
            }
        //}
        
    }

    /*void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.forward, Color.blue, 0.75f);
        Debug.DrawRay(transform.position, Vector3.back, Color.yellow, 0.75f);
        Debug.DrawRay(transform.position, Vector3.left, Color.red, 0.75f);
        Debug.DrawRay(transform.position, Vector3.right, Color.green, 0.75f);
    }*/



    public IEnumerator ReturnToStartScale (float speed)
    {
        Vector3 firstScale = transform.localScale;
        //Vector3 lastScale = transform.localScale;
        _imMoving = true;
        _imReturningStartPos = true;
        GetComponent<Renderer>().material.color = Color.green;
        if (firstScale.y > startScaleY)
        {
            while(transform.localScale.y >startScaleY)
            {
                transform.localScale += new Vector3(0, -speed * Time.deltaTime, 0);
                transform.Translate(Vector3.down * Time.deltaTime * (speed / 2));

                //lastScale = transform.localScale;
                yield return null;
            }
        }
        else if(firstScale.y < startScaleY)
        {
            while (transform.localScale.y < startScaleY)
            {
                transform.localScale += new Vector3(0, speed * Time.deltaTime, 0);
                transform.Translate(Vector3.up * Time.deltaTime * (speed / 2));
                yield return null;
            }
        }
        transform.localScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, startPosY, transform.position.z);
        _imMoving = false;
        imAtStartPos = true;
        _imReturningStartPos = false;
        GetComponent<Renderer>().material.color = Color.white;
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
        Vector3 firstPos = transform.position;
        
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
               
                
                transform.Translate(direction * Time.deltaTime * (speed / 2));
                //Debug.Log("pouet");
                yield return null;
            }
            if (transform.localScale.y >= 1)
            {
                transform.localScale = new Vector3(transform.localScale.x, firstScale.y + (strength * direction.y), transform.localScale.z);
                if(strength == 1)
                transform.position = new Vector3(transform.position.x, firstPos.y + (0.5f * direction.y), transform.position.z);
            }
            //transform.localScale = new Vector3 (transform.localScale.x, firstScale.y + (strength * direction.y), transform.localScale.z);

        }
        else if (direction.y > 0)
        {
            while (transform.localScale.y <= firstScale.y + (strength * direction.y))
            {
                transform.localScale += new Vector3(0, speed * Time.deltaTime, 0);
                transform.Translate(direction * Time.deltaTime *( speed /2));
                
                yield return null;
            }
            transform.localScale = new Vector3(transform.localScale.x, firstScale.y + (strength * direction.y), transform.localScale.z);
            if(strength == 1)
            transform.position = new Vector3(transform.position.x, firstPos.y + (0.5f * direction.y), transform.position.z);

        }
        _imMoving = false;
        imAtStartPos = false;
        GetComponent<Renderer>().material.color = Color.white;



    }

    



}
