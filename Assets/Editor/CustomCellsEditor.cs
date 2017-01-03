using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateInEditor))]
public class CustomCellsEditor : Editor {

    GenerateInEditor m_targetedScript;

    public override void OnInspectorGUI()
    {
        m_targetedScript = (GenerateInEditor)target;
        DrawDefaultInspector();
        DrawCellsEditInspector();
    }

    void DrawCellsEditInspector()
    {
        DrawCreateCleanWorld();
        GUILayout.Space(5);
        GUILayout.Label("Cell Types", EditorStyles.boldLabel);
        GUILayout.Space(5);
        GUILayout.Label("Order : Name / IsWithFeedBackEmission/ IsWithFeedBackMaterial / Material / Altitude");
        GUILayout.Space(5);
        for (int i = 0; i < m_targetedScript.cellTypes.Count; i++)
        {
            DrawCellType(i);
                
        }
        DrawAddNewCellTypeButton();
    }

    void DrawCreateCleanWorld()
    {
        GUILayout.Space(5);
        if (GUILayout.Button("Generate World",GUILayout.Height(30)))
        {
            m_targetedScript.CreateWorld();
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Clean World", GUILayout.Height(30)))
        {
            m_targetedScript.CleanWorld();
        }
    }
    void DrawCellType(int index)
    {
        if(index < 0 || index >= m_targetedScript.cellTypes.Count)
        {
            return;
        }

        //SerializedProperty listIterator = serializedObject.FindProperty("CellType");

        GUILayout.BeginHorizontal();
        {
            // permet de mettre en gras si c'est modifié
            /*if(listIterator.isInstantiatedPrefab == true)
            {
                EditorGUIHelper.SetBoldDefaultFont(listIterator.GetArrayElementAtIndex(index).prefabOverride);
            }*/

            //GUILayout.Label("Name", EditorStyles.label, GUILayout.Width(50));

            // BeginChangeCheck et EndChangeCheck permet de voir si la variable a été modifié
            EditorGUI.BeginChangeCheck();
            string newName = GUILayout.TextField(m_targetedScript.cellTypes[index].name, GUILayout.Width(120));
            Texture tex = null;

            //Color newColor = EditorGUILayout.ColorField("",m_targetedScript.cellTypes[index].color, GUILayout.Width(120));

            bool newBoolFeedbackEmission = GUILayout.Toggle(m_targetedScript.cellTypes[index].feedBackOnEmission,tex, GUILayout.Width(20));
            bool newBoolFeedbackMaterial = GUILayout.Toggle(m_targetedScript.cellTypes[index].feedBackOnMaterial, tex, GUILayout.Width(20));

            Material newMaterial = (Material)EditorGUILayout.ObjectField( m_targetedScript.cellTypes[index].mat, typeof(Material), true, GUILayout.Width(120));
           
            //float newSpeedUp = EditorGUILayout.FloatField("", m_targetedScript.cellTypes[index].speedUp, GUILayout.Width(80));
            
            int newStartY = EditorGUILayout.IntField("", m_targetedScript.cellTypes[index].diffWithBasePosY, GUILayout.Width(80));

            if(newBoolFeedbackEmission == true)
            {
                newBoolFeedbackMaterial = false;
            }
            if(newBoolFeedbackMaterial ==true)
            {
                newBoolFeedbackEmission = false;
            }

            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_targetedScript, "Modified CellType");

                m_targetedScript.cellTypes[index].name = newName;
                //m_targetedScript.cellTypes[index].color = newColor;
                //m_targetedScript.cellTypes[index].speedUp = newSpeedUp;
                m_targetedScript.cellTypes[index].mat = newMaterial;
                m_targetedScript.cellTypes[index].diffWithBasePosY = newStartY;
                m_targetedScript.cellTypes[index].feedBackOnEmission = newBoolFeedbackEmission;
                m_targetedScript.cellTypes[index].feedBackOnMaterial = newBoolFeedbackMaterial;

                EditorUtility.SetDirty(m_targetedScript);
                UpdateAllCellTypes();
                
            }
            EditorGUIHelper.SetBoldDefaultFont(false);

            if(GUILayout.Button("Remove"))
            {
                EditorApplication.Beep();
                if(EditorUtility.DisplayDialog("Really?", "Do you really want to remove the cellType" + m_targetedScript.cellTypes[index].name + "?", "Yes", "No") == true)
                {
                    Undo.RecordObject(m_targetedScript, "Delete CellType");
                    m_targetedScript.cellTypes.RemoveAt(index);
                    EditorUtility.SetDirty(m_targetedScript);
                }
            }
        }
        GUILayout.EndHorizontal();
    }
    void DrawAddNewCellTypeButton()
    {
        if(GUILayout.Button("Add New CellType", GUILayout.Height(30)))
        {
            Undo.RecordObject(m_targetedScript, "Add new CellType");

            m_targetedScript.cellTypes.Add(new CellType { name = "New CellType" });

            EditorUtility.SetDirty(m_targetedScript);
        }
    }

   
    public void UpdateAllCellTypes()
    {
        for (int i = 0; i < m_targetedScript.worldGenerate.transform.childCount; i++)
        {
            if (m_targetedScript.worldGenerate.transform.GetChild(i).name != "Cell")
            {
                for (int j = 0; j < m_targetedScript.cellTypes.Count; j++)
                {
                    if (m_targetedScript.worldGenerate.transform.GetChild(i).GetComponent<CellTwo>().cellType.name == m_targetedScript.cellTypes[j].name)
                    {
                        CellTwo cellTwoTemp = m_targetedScript.worldGenerate.transform.GetChild(i).GetComponent<CellTwo>();
                        CellType cellTypeTemp = m_targetedScript.cellTypes[j];

                        Undo.RecordObject(cellTwoTemp, "Update Cell");
                        //cellTwoTemp.cellType.color = cellTypeTemp.color;
                        //cellTwoTemp.cellType.speedUp = cellTypeTemp.speedUp;
                        cellTwoTemp.cellType.diffWithBasePosY = cellTypeTemp.diffWithBasePosY;
                        cellTwoTemp.cellType.feedBackOnEmission = cellTypeTemp.feedBackOnEmission;
                        cellTwoTemp.UpdateCellType();
                        EditorUtility.SetDirty(cellTwoTemp);

                    }
                }
            }
        }
    }

}
