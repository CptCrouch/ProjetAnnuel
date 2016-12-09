using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WorldGenerate : MonoBehaviour {

    public int height;
    public int length;
    public int width;


    // doit etre un nombre impair
    public int diametreWorldHexagon;
    

    public float increaseZ = 1.3f;
    public float increaseX = 0.75f;

    public int howManyCellBonusInPool;

    //public int diametreGrille = 10;

    public bool isHexagonWorld = false;
    

    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject prefabHexagon;
    [SerializeField]
    private GameObject prefabWall;
    [SerializeField]
    private GameObject pool;

    private int poolCount;
    private int rayon;
    //public List<GameObject> cells;
    
    


    // Use this for initialization
    void Start () {
        rayon = (diametreWorldHexagon-1) / 2;
        Debug.Log(rayon);
        if (isHexagonWorld == false)
            GenerateWorld();
        else
            GenerateHexagonWorld();


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

    public void GenerateHexagonWorld()
    {
        for (int i = -rayon; i <= rayon; i++)
        {
            int pouet = diametreWorldHexagon - Mathf.Abs(i);
            if(pouet % 2 == 0)
            {
                for (int j = -pouet+1; j <= pouet -1; j=j+2)
                {
                    Vector3 pos = new Vector3(j  * increaseX, -height, i * increaseZ);
                    
                    GameObject newObject = Instantiate(prefabHexagon, Vector3.zero, Quaternion.identity) as GameObject;

                    newObject.transform.position = pos;
                    newObject.transform.localScale = new Vector3(newObject.transform.localScale.x, height, newObject.transform.localScale.z);
                    newObject.transform.SetParent(transform);
                    newObject.SetActive(true);
                    newObject.name = newObject.transform.position.x + " / " + newObject.transform.position.y + " / " + newObject.transform.position.z;
                    newObject.AddComponent<Cell>();
                    newObject.GetComponent<Cell>().startPosY = -height / 2;
                    newObject.GetComponent<Cell>().startScaleY = height;
                    poolCount++;
                }
            }
                //pair
                else
            {
                //impair
                int lel = (pouet - 1) / 2;
                for (int j = -lel; j <= lel; j++)
                {
                    Vector3 pos = new Vector3(j * increaseX *2, -height, i * increaseZ);
                    
                    GameObject newObject = Instantiate(prefabHexagon, Vector3.zero, Quaternion.identity) as GameObject;
                   
                    newObject.transform.position = pos;
                    newObject.transform.localScale = new Vector3(newObject.transform.localScale.x, height, newObject.transform.localScale.z);
                    newObject.transform.SetParent(transform);
                    newObject.SetActive(true);
                    newObject.name = newObject.transform.position.x + " / " + newObject.transform.position.y + " / " + newObject.transform.position.z;
                    newObject.AddComponent<Cell>();
                    newObject.GetComponent<Cell>().startPosY = -height / 2;
                    newObject.GetComponent<Cell>().startScaleY = height;
                    poolCount++;
                }
            }
                
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawCube(Vector3.zero, new Vector3(diametreGrille, diametreGrille, diametreGrille));
    }
	
	
}
