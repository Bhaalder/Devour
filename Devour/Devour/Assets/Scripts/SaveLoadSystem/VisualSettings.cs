//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSettings : MonoBehaviour
{
    [SerializeField] private Toggle depthBlurToggle;
    public bool DepthBlur { get; set; }

    private Settings settings;

    private void Start()
    {
        settings = DataStorage.Instance.Settings;
        DepthBlur = settings.DepthBlur;
        depthBlurToggle.isOn = DepthBlur;
    }

    void Update()
    {
        
    }

    public void OnToggleChange()
    {
        DepthBlur = depthBlurToggle.isOn;
        SaveVisualSettings();
        DataStorage.Instance.SaveSettings();
        VisualSettingsEvent blurToggle = new VisualSettingsEvent { };
        blurToggle.FireEvent();
    }

    public void SaveVisualSettings()
    {
        if (settings != null)
        {
            settings.DepthBlur = DepthBlur;
        }
    }

}
