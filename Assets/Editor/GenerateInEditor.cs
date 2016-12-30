using UnityEngine;
using UnityEditor;
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

    public void ApplyNewCellType(int indexCellTypes,GameObject[] selection)
    {
        for (int i = 0; i < selection.Length; i++)
        {
            if (selection[i].tag == "Cell")
            {
                CellTwo cellTwo = selection[i].GetComponent<CellTwo>();
                Undo.RecordObject(cellTwo, "Update Cell");

                cellTwo.cellType.name = cellTypes[indexCellTypes].name;
                cellTwo.cellType.color = cellTypes[indexCellTypes].color;
                cellTwo.cellType.speedUp = cellTypes[indexCellTypes].speedUp;
                cellTwo.cellType.diffWithBasePosY = cellTypes[indexCellTypes].diffWithBasePosY;
                cellTwo.cellType.imAppliedToCell = true;


                cellTwo.UpdateCellType();
                EditorUtility.SetDirty(cellTwo);
            }
        }
    }
    public void UpdateAllCellTypes()
    {
        for (int i = 0; i < worldGenerate.transform.childCount; i++)
        {
            if (worldGenerate.transform.GetChild(i).name != "Cell")
            {
                for (int j = 0; j < cellTypes.Count; j++)
                {
                    if (worldGenerate.transform.GetChild(i).GetComponent<CellTwo>().cellType.name == cellTypes[j].name)
                    {
                        CellTwo cellTwoTemp = worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
                        CellType cellTypeTemp = cellTypes[j];

                        Undo.RecordObject(cellTwoTemp, "Update Cell");
                        cellTwoTemp.cellType.color = cellTypeTemp.color;
                        cellTwoTemp.cellType.speedUp = cellTypeTemp.speedUp;
                        cellTwoTemp.cellType.diffWithBasePosY = cellTypeTemp.diffWithBasePosY;
                        cellTwoTemp.UpdateCellType();
                        EditorUtility.SetDirty(cellTwoTemp);

                    }
                }
            }
        }
    }
    
}
