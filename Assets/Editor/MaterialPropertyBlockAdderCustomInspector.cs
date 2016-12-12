using UnityEngine;
using System.Collections;
[UnityEditor.CustomEditor(typeof(MaterialPropertyBlockAdder))]
public class MaterialPropertyBlockAdderCustomInspector : UnityEditor.Editor
{
public override void OnInspectorGUI()
    {
        UnityEditor.EditorGUI.BeginChangeCheck();
        this.DrawDefaultInspector();
        if (UnityEditor.EditorGUI.EndChangeCheck())
        {
            MaterialPropertyBlockAdder typedTarget = this.target as MaterialPropertyBlockAdder;
            if (typedTarget)
            {
                typedTarget.Apply();
            }
        }
        
    }
}