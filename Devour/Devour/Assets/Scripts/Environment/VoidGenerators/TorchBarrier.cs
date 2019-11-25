//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBarrier : MonoBehaviour{

    [SerializeField] private GeneratorTorch[] generatorTorches;
    [SerializeField] private int torchesRequiredToDeactivate;
    private int torchesDeactivated;

    private void Start() {
        for(int i = 0; i < generatorTorches.Length; i++) {
            if (generatorTorches[i].IsDeactivated) {
                torchesDeactivated++;
            }
        }
        if(torchesDeactivated >= torchesRequiredToDeactivate) {
            Destroy(gameObject);
        }
    }

}
