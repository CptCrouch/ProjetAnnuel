﻿using UnityEngine;
using System.Collections;

[UnityEngine.ExecuteInEditMode]
public class MaterialPropertyBlockAdder : MonoBehaviour
{
    [UnityEngine.Serialization.FormerlySerializedAs("propertyColors")]
    [UnityEngine.SerializeField]
    public PropertyColor[] PropertyColors = { };

    [UnityEngine.Serialization.FormerlySerializedAs("propertyVectors")]
    [UnityEngine.SerializeField]
    public PropertyVector[] PropertyVectors = { };

    [UnityEngine.SerializeField]
    public PropertyFloat[] PropertyFloats = { };

    [System.NonSerialized]
    UnityEngine.MaterialPropertyBlock materialPropertyBlock;
    
    public void Start ()
    {
        this.Apply();
    }
	
	public void Apply()
    {
        UnityEngine.Renderer renderer = this.transform.GetComponent<UnityEngine.Renderer>();
        if (renderer != null)
        {
            if (this.materialPropertyBlock == null)
            {
                this.materialPropertyBlock = new UnityEngine.MaterialPropertyBlock();
            }

            renderer.GetPropertyBlock(this.materialPropertyBlock);

            {
                int propertyCount = this.PropertyColors.Length;
                for (int i = 0; i < propertyCount; ++i)
                {
                    this.materialPropertyBlock.SetColor(this.PropertyColors[i].PropertyName, this.PropertyColors[i].PropertyValue);
                }
            }

            {
                int propertyCount = this.PropertyVectors.Length;
                for (int i = 0; i < propertyCount; ++i)
                {
                    this.materialPropertyBlock.SetVector(this.PropertyVectors[i].PropertyName, this.PropertyVectors[i].PropertyValue);
                }
            }

            {
                int propertyCount = this.PropertyFloats.Length;
                for (int i = 0; i < propertyCount; ++i)
                {
                    this.materialPropertyBlock.SetFloat(this.PropertyFloats[i].PropertyName, this.PropertyFloats[i].PropertyValue);
                }
            }

            renderer.SetPropertyBlock(this.materialPropertyBlock);
        }
    }

    [System.Serializable]
    public struct PropertyColor
    {
        public string PropertyName;
        public UnityEngine.Color PropertyValue;
    }

    [System.Serializable]
    public struct PropertyVector
    {
        public string PropertyName;
        public UnityEngine.Vector4 PropertyValue;
    }

    [System.Serializable]
    public struct PropertyFloat
    {
        public string PropertyName;
        public float PropertyValue;
    }
}
