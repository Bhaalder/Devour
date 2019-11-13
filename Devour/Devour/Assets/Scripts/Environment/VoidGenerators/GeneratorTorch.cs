using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTorch : MonoBehaviour{

    public bool IsDeactivated { get; set; }

    [SerializeField] private GameObject particles;
    [Tooltip("The sceneName where the required generator has to be destroyed")]
    [SerializeField] private string generatorSceneName;

    private void OnEnable() {
        if (GameController.Instance.DestroyedVoidGenerators.ContainsKey(generatorSceneName)) {
            IsDeactivated = true;
            Destroy(particles);
        }
    }

}
