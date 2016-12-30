using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class GenerateInEditor : MonoBehaviour {

    public WorldGenerate worldGenerate;
    
    
    [HideInInspector]
    public List<CellType> cellTypes = new List<CellType>();

    private bool alreadyAWorld = false;

    public void CleanWorld()
    {
        while (worldGenerate.transform.childCount > 0)
        {
            worldGenerate.CleanEditorWorld();
        }
        alreadyAWorld = false;
    }
    public void CreateWorld()
    {
        if (alreadyAWorld == false)
        {
            worldGenerate.GenerateHexagonWorld((worldGenerate.diametreWorldHexagon - 1) / 2);
            alreadyAWorld = true;
        }
        else
        {
            Debug.Log("Il y déjà un monde. Utilise le bouton Clean");
        }
    }

   
    
}
