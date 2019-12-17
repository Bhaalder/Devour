using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurSettings : MonoBehaviour
{
    private PostProcessVolume blur;

    private 
    // Start is called before the first frame update
    void Start()
    {
        blur = GetComponent<PostProcessVolume>();
        blur.enabled = DataStorage.Instance.SettingsData.DepthBlur;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
