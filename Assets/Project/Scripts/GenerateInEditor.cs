using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GenerateInEditor : MonoBehaviour {

    public WorldGenerate worldGenerate;
    public bool generateInput = false;
    public bool cleanWorld = false;
	void Update () {
        if (cleanWorld == true)
        {
            while (worldGenerate.transform.childCount > 0)
            {
                worldGenerate.CleanEditorWorld();
            }
            cleanWorld = false;
        }
        if (generateInput == true)
        {
           
            worldGenerate.GenerateHexagonWorld((worldGenerate.diametreWorldHexagon - 1) / 2);
            generateInput = false;
            
        }

	}
}
