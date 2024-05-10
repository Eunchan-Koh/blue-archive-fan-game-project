using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using UnityEngine;

public class test : MonoBehaviour
{
    MeshRenderer meshr;
    [Range(0.0f,1f)]
    public float r;
    [Range(0.0f,1f)]
    public float g;
    [Range(0.0f,1f)]
    public float b;
    [Range(0.0f,1f)]
    public float a;

    void Start(){
        meshr = GetComponent<MeshRenderer>();
    }
    void Update(){
        // Debug.Log(meshr.material.);
        meshr.material.color = new Color(r,g,b,a);
    }
}
