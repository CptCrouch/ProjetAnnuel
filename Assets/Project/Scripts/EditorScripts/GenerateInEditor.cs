using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class GenerateInEditor : MonoBehaviour {

    public WorldGenerate worldGenerate;

    [Space(10)]
    [Header("[ Diametre de l'HexagonWorld ]")]

    public int diametreWorldHexagon;

    [Space(10)]
    [Header("[ Feedback Colors ]")]
    public Color startCellColor;
    public Color feedBackCellColor = Color.red;
    public Color colorWhenGrow = Color.white;
    public Color colorWhenTargeted = Color.white;
    
    [Space(10)]
    public Material materialWhenGrow;
    public Material feedbackCellMaterial;
    public Material materialWhenTargeted;

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
            worldGenerate.GenerateHexagonWorld(diametreWorldHexagon, 
                startCellColor, 
                feedBackCellColor, 
                colorWhenGrow, 
                materialWhenGrow, 
                feedbackCellMaterial,
                colorWhenTargeted,
                materialWhenTargeted);
            alreadyAWorld = true;
        }
        else
        {
            Debug.Log("Il y déjà un monde. Utilise le bouton Clean");
        }
    }

   
    
}
