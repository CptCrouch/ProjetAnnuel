using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WorldGenerate : MonoBehaviour {

    public int height;
    public int length;
    public int width;

    public int howManyCellBonusInPool;

    public int diametreGrille = 100;
    

    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject prefabWall;
    [SerializeField]
    private GameObject pool;

    private int poolCount;
    //public List<GameObject> cells;
    
    


    // Use this for initialization
    void Start () {
        GenerateWorld();

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
                Vector3 pos = new Vector3(0,0,length/2+width/2);
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(width, 1000, width);
            }
            if (i == 1)
            {
                Vector3 pos = new Vector3(length / 2 + width / 2, 0, 0);
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(length, 1000, length);

            }
            if (i == 2)
            {
                Vector3 pos = new Vector3(0, 0, -(length / 2 + width / 2));
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(width, 1000, width);
            }
            if (i == 3)
            {
                Vector3 pos = new Vector3(-(length / 2 + width / 2), 0, 0);
                newObject.transform.position = pos;
                newObject.transform.localScale = new Vector3(length, 1000, length);
            }
        }
            
        
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawCube(Vector3.zero, new Vector3(diametreGrille, diametreGrille, diametreGrille));
    }
	
	
}
