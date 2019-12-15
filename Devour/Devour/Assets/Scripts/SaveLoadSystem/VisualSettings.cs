using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSettings : MonoBehaviour
{
    public bool DepthBlur { get; set; }

    private Settings settings;

    private void OnEnable()
    {
        settings = FindObjectOfType<Settings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveVisualSettings()
    {
        settings.DepthBlur = DepthBlur;
    }

}
