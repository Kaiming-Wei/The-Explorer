using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialRender : MonoBehaviour
{
    public Material material;
    public int renderQueue;

    // Start is called before the first frame update
    void Start()
    {
        material.renderQueue = renderQueue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
