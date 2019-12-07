//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossIntroUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private TextMeshProUGUI bossTitleText;

    [SerializeField] private Animator animator;

    private void Awake() {
        BossIntroEvent.RegisterListener(BossIntro);
    }

    private void BossIntro(BossIntroEvent introEvent) {
        bossNameText.text = introEvent.bossName;
        bossTitleText.text = introEvent.bossTitle;
        //animator.SetTrigger("");
    }


    private void OnDestroy() {
        BossIntroEvent.UnRegisterListener(BossIntro);
    }
}
