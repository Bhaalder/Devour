using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurSettings : MonoBehaviour
{
    private PostProcessLayer blur;

    private 
    // Start is called before the first frame update
    void Start()
    {
        blur = GetComponent<PostProcessLayer>();
        blur.enabled = DataStorage.Instance.Settings.DepthBlur;
    }

}
