using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSettings : MonoBehaviour
{
    [SerializeField] private Toggle depthBlurToggle;
    public bool DepthBlur { get; set; }

    private Settings settings;

    private void OnEnable()
    {
        settings = DataStorage.Instance.Settings;
        DepthBlur = settings.DepthBlur;
        depthBlurToggle.isOn = DepthBlur;
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
