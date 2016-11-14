using UnityEngine;
using System.Collections;

public class CutScript : MonoBehaviour {

    public Mesh mesh;
    public Vector3[] basev;
    public Vector2[] baseh;
    public bool[] side; 
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        basev = mesh.vertices;
        baseh = mesh.uv;
        side = new bool[basev.Length];

    }
}
