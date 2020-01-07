//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurSettings : MonoBehaviour
{
    private PostProcessLayer blur;

    void Start()
    {
        blur = GetComponent<PostProcessLayer>();
        blur.enabled = DataStorage.Instance.Settings.DepthBlur;
        VisualSettingsEvent.RegisterListener(OnVisualSettingsEvent);
    }

    private void OnVisualSettingsEvent(VisualSettingsEvent visualSettingsEvent)
    {
        blur.enabled = DataStorage.Instance.Settings.DepthBlur;
    }

}
