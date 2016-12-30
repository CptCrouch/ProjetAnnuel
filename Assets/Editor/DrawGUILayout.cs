using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class DrawGUILayout : Editor {

    public static GenerateInEditor generateInEditor;
 
    public static int SelectedType
    {
        get { return EditorPrefs.GetInt("SelectedType", 0); }
        set { EditorPrefs.SetInt("SelectedType", value); }
    }

    static DrawGUILayout()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
       
    }

    static void OnSceneGUI(SceneView _sceneView)
    {
        if(generateInEditor == null)
        {
            generateInEditor = FindObjectOfType<GenerateInEditor>();
        }
        if (Selection.gameObjects.Length >0 && CheckObjectSelection(Selection.gameObjects) == true )
        {
            DrawToolsMenu(_sceneView.position);
            SceneView.RepaintAll();
            DrawCustomBlockButtons(_sceneView);

        }
        

        //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    }
    static bool CheckObjectSelection(GameObject[] objSelected)
    {
        for (int i = 0; i < objSelected.Length; i++)
        {
            if (objSelected[i].tag != "Cell" && objSelected[i].tag != "WorldGenerate")
            {
                return false;
            }
        }
        return true;
    }
    static void DrawToolsMenu(Rect _position)
    {
        //By using Handles.BeginGUI() we can start drawing regular GUI elements into the SceneView
        Handles.BeginGUI();

        //Here we draw a toolbar at the bottom edge of the SceneView
        GUILayout.BeginArea(new Rect(0, _position.height - 35, _position.width, 30 ), EditorStyles.whiteLabel);
        GUILayout.BeginHorizontal();
        
        if(GUILayout.Button("Apply CellType To Selection",EditorStyles.toolbarButton))
        {
            ApplyNewCellType(SelectedType, Selection.gameObjects);
        }
        GUILayout.FlexibleSpace();
            
        
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }
    
    static void DrawCustomBlockButtons(SceneView _sceneView)
    {
        Handles.BeginGUI();

        GUI.Box(new Rect(0, 0, 90, _sceneView.position.height - 35), GUIContent.none, EditorStyles.textArea);

        for (int i = 0; i < generateInEditor.cellTypes.Count; ++i)
        {
            DrawCustomBlockButton(i);
        }

        Handles.EndGUI();
    }

    static void DrawCustomBlockButton(int index)
    {
        bool _isActive = index == SelectedType;

        //By passing a Prefab or GameObject into AssetPreview.GetAssetPreview you get a texture that shows this object
        //Texture2D _previewImage = AssetPreview.GetAssetPreview(_allPrefabs.allPrefabs[_index].prefab);
        //GUIContent _buttonContent = new GUIContent(_previewImage);
        GUIStyle toggleStyle = GUI.skin.button;

        GUIStyle _text = GUIStyle.none;
        _text.normal.textColor = generateInEditor.cellTypes[index].color;
        GUI.Label(new Rect(12, index * 98 + 8, 100, 20), generateInEditor.cellTypes[index].name, _text);
        bool _isToggleDown = GUI.Toggle(new Rect(12, index * 98 + 25, 60, 60), _isActive, "",toggleStyle);
        //Color _whatColor = EditorGUILayout.ColorField(Color.white,new Rect())

        //If this button is clicked but it wasn't clicked before (ie. if the user has just pressed the button)
        if (_isToggleDown && !_isActive)
            SelectedType = index;
    }

    public static void ApplyNewCellType(int indexCellTypes, GameObject[] selection)
    {
        for (int i = 0; i < selection.Length; i++)
        {
            if (selection[i].tag == "Cell")
            {
                CellTwo cellTwo = selection[i].GetComponent<CellTwo>();
                Undo.RecordObject(cellTwo, "Update Cell");

                cellTwo.cellType.name = generateInEditor.cellTypes[indexCellTypes].name;
                cellTwo.cellType.color = generateInEditor.cellTypes[indexCellTypes].color;
                cellTwo.cellType.speedUp = generateInEditor.cellTypes[indexCellTypes].speedUp;
                cellTwo.cellType.diffWithBasePosY = generateInEditor.cellTypes[indexCellTypes].diffWithBasePosY;
                cellTwo.cellType.imAppliedToCell = true;


                cellTwo.UpdateCellType();
                EditorUtility.SetDirty(cellTwo);
            }
        }
    }


}
