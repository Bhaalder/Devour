//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBarrier : MonoBehaviour{

    [SerializeField] private GeneratorTorch[] generatorTorches;
    [SerializeField] private int torchesRequiredToDeactivate;
    private int torchesDeactivated;

    private void Start() {
        foreach(GeneratorTorch torch in generatorTorches) {
            if (torch.IsDeactivated) {
                torchesDeactivated++;
            }
        }
        if(torchesDeactivated >= torchesRequiredToDeactivate) {
            Destroy(gameObject);
        }
    }

}
