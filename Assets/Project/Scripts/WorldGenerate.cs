﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WorldGenerate : MonoBehaviour {

    public int height;
    public int length;
    public int width;

    public float tailleYBigHexagon;


   
    public int diametreWorldHexagon;
    

    public float increaseZ = 1.3f;
    public float increaseX = 0.75f;

    public int howManyCellBonusInPool;

    public Color startCellColor;
    public Color goDownCellColor;

    //public int diametreGrille = 10;

    public bool isHexagonWorld = false;
    

    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject prefabHexagon;
    [SerializeField]
    private GameObject prefabWall;
    [SerializeField]
    private GameObject prefabWallHexagon;
    [SerializeField]
    private GameObject pool;

    private int poolCount;
    private int rayon;
    //public List<GameObject> cells;
    
    


    // Use this for initialization
    void Start () {
        rayon = (diametreWorldHexagon-1) / 2;

        if (isHexagonWorld == false)
            GenerateWorld();
        else
        {
            if (Application.isEditor ==  false)
            {
                GenerateHexagonWorld(rayon);
                
            }
            else
            {
                Debug.Log("WorldGenerate in editor");
            }
            
        }


	}
    public void GenerateWorld()
    {
       
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        Vector3 pos = new Vector3(-width / 2 + k, - height/2, -length / 2 + j);
                        //GameObject newObject = pool.transform.GetChild(poolCount).gameObject;
                        GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                        newObject.transform.position = pos;
                        newObject.transform.localScale = new Vector3(1, height, 1);
                        newObject.transform.SetParent(transform);
                        newObject.SetActive(true);
                        newObject.name = newObject.transform.position.x + " / " + newObject.transform.position.y + " / " + newObject.transform.position.z;
                        newObject.AddComponent<Cell>();
                        newObject.GetComponent<Cell>().startPosY = -height / 2;
                        newObject.GetComponent<Cell>().startScaleY = height;
                        poolCount++;
                        //Debug.Log(poolCount);
                    }
                }

        for (int i = 0; i < howManyCellBonusInPool; i++)
        {
            GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            newObject.transform.SetParent(pool.transform);
            newObject.SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            
            GameObject newObject = Instantiate(prefabWall, Vector3.zero, Quaternion.identity) as GameObject;
            if (i == 0)
            {
                Vector3 pos = new Vector3(-0.5f,0,length/2+width/2-0.5f);
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(width*3, 1000, width);
            }
            if (i == 1)
            {
                Vector3 pos = new Vector3(length / 2 + width / 2 -0.5f, 0, -0.5f);
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(length, 1000, length*3);

            }
            if (i == 2)
            {
                Vector3 pos = new Vector3(-0.5f, 0, -(length / 2 + width / 2 + 0.5f));
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(width*3, 1000, width);
            }
            if (i == 3)
            {
                Vector3 pos = new Vector3(-(length / 2 + width / 2 + 0.5f), 0, -0.5f);
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(length, 1000, length*3);
            }
        }
            
        
    }

    public void GenerateHexagonWorld(int rayon)
    {
        for (int i = -rayon; i <= rayon; i++)
        {
            int posX = diametreWorldHexagon - Mathf.Abs(i);
            if(posX % 2 == 0)
            {
                for (int j = -posX+1; j <= posX -1; j=j+2)
                {
                    Vector3 pos = new Vector3(j  * increaseX, /*-height*/ (tailleYBigHexagon * 100) / 2, i * increaseZ);

                    
                    GameObject newObject = Instantiate(prefabHexagon, Vector3.zero, Quaternion.identity) as GameObject;
                    newObject.transform.position = pos;
                    //newObject.transform.localScale = new Vector3(newObject.transform.localScale.x, height, newObject.transform.localScale.z);
                    //newObject.GetComponent<MaterialPropertyBlockAdder>().PropertyVectors[0].PropertyValue.y = height * 0.66f;
                    
                    newObject.transform.SetParent(transform);
                    newObject.SetActive(true);
                    newObject.name = newObject.transform.position.x + " / " + newObject.transform.position.y + " / " + newObject.transform.position.z;
                    newObject.AddComponent<CellTwo>();
                    CellTwo cell = newObject.GetComponent<CellTwo>();
                    cell.startPosY = /*-height*/ (tailleYBigHexagon * 100) / 2;
                    cell.startScaleY = height;
                    newObject.GetComponent<MeshRenderer>().sharedMaterial.color = startCellColor;
                    cell.startColor = startCellColor;
                    if(cell.transform.childCount>0)
                    newObject.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.color = startCellColor;
                    cell.colorGoDown = goDownCellColor;

                    poolCount++;
                    
                }
            }
                //pair
                else
            {
                //impair
                int lel = (posX - 1) / 2;
                for (int j = -lel; j <= lel; j++)
                {
                    Vector3 pos = new Vector3(j * increaseX *2, /*-height*/ (tailleYBigHexagon * 100) / 2, i * increaseZ);

                    GameObject newObject = Instantiate(prefabHexagon, Vector3.zero, Quaternion.identity) as GameObject;
                    newObject.transform.position = pos;
                    //newObject.transform.localScale = new Vector3(newObject.transform.localScale.x, height, newObject.transform.localScale.z);
                    //newObject.GetComponent<MaterialPropertyBlockAdder>().PropertyVectors[0].PropertyValue.y = height * 0.66f;
                    newObject.transform.SetParent(transform);
                    newObject.SetActive(true);
                    newObject.name = newObject.transform.position.x + " / " + newObject.transform.position.y + " / " + newObject.transform.position.z;
                    newObject.AddComponent<CellTwo>();
                    CellTwo cell = newObject.GetComponent<CellTwo>();
                    cell.startPosY = /*-height*/ (tailleYBigHexagon * 100) / 2;
                    cell.startScaleY = height;
                    newObject.GetComponent<MeshRenderer>().sharedMaterial.color = startCellColor;
                    cell.startColor = startCellColor;
                    if (cell.transform.childCount > 0)
                        newObject.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.color = startCellColor;
                    cell.colorGoDown = goDownCellColor;

                    poolCount++;
                    
                }
            }
                
        }
    }

    public void CleanEditorWorld()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
            //Debug.Log(i);
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawCube(Vector3.zero, new Vector3(diametreGrille, diametreGrille, diametreGrille));
    }
    void SpawnWallHexagon(Vector3 pos)
    {
        GameObject newObject = Instantiate(prefabWallHexagon, Vector3.zero, Quaternion.identity) as GameObject;
        newObject.transform.position = pos;
        newObject.transform.localScale = new Vector3(newObject.transform.localScale.x, height+100, newObject.transform.localScale.z);
        newObject.SetActive(true);
    }
	
	
}