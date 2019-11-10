//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentScreen : MonoBehaviour{

    [SerializeField] private Button damageButton;
    [SerializeField] private Button survivalButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button voidButton;

    [SerializeField] private Button resetButton;
    [SerializeField] private Button doneButton;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI voidEssenceCostText;
    [SerializeField] private TextMeshProUGUI lifeForceCostText;

    [TextArea(0, 5)]
    [SerializeField] private string resetButtonInfo;
    [TextArea(0, 5)]
    [SerializeField] private string doneButtonInfo;

    private TalentPointButton[] talentPointButtons;

    private List<TalentPoint> currentAddedTalentPoints;

    private void Awake() {
        currentAddedTalentPoints = new List<TalentPoint>();
        talentPointButtons = new TalentPointButton[4];
        talentPointButtons[0] = damageButton.GetComponent<TalentPointButton>();
        talentPointButtons[1] = survivalButton.GetComponent<TalentPointButton>();
        talentPointButtons[2] = speedButton.GetComponent<TalentPointButton>();
        talentPointButtons[3] = voidButton.GetComponent<TalentPointButton>();

        damageButton.onClick.AddListener(() => { CheckSelectedButton(talentPointButtons[0]); });
        survivalButton.onClick.AddListener(() => { CheckSelectedButton(talentPointButtons[1]); });
        speedButton.onClick.AddListener(() => { CheckSelectedButton(talentPointButtons[2]); });
        voidButton.onClick.AddListener(() => { CheckSelectedButton(talentPointButtons[3]); });

        resetButton.onClick.AddListener(() => { ResetButton(); });
        doneButton.onClick.AddListener(() => { DoneButton(); });

    }

    private void OnEnable() {
        voidEssenceCostText.text = "0";
        lifeForceCostText.text = "0";
        descriptionText.text = "";
        errorText.text = "";
        bool hasVoidMend;
        if (GameController.Instance.Player.HasAbility(PlayerAbility.VOIDMEND)) {
            hasVoidMend = true;
        } else {
            hasVoidMend = false;
        }
        voidButton.gameObject.SetActive(hasVoidMend);
    }

    private void OnGUI() {     
        if (EventSystem.current.currentSelectedGameObject != null) {
            try {
                descriptionText.text = EventSystem.current.currentSelectedGameObject.GetComponent<TalentPointButton>().TalentPoint.description;
            } catch (System.NullReferenceException) {
                if(EventSystem.current.currentSelectedGameObject == resetButton.gameObject) {
                    descriptionText.text = resetButtonInfo;
                } else if(EventSystem.current.currentSelectedGameObject == doneButton.gameObject) {
                    descriptionText.text = doneButtonInfo;
                } else {
                    descriptionText.text = "";
                }
            }
        }
    }

    private void CheckSelectedButton(TalentPointButton talentButton) {
        int currentPointsAdded = 0;
        foreach(TalentPoint talentPoint in currentAddedTalentPoints) {
            if(talentPoint.talentPointType == talentButton.TalentPoint.talentPointType) {
                ++currentPointsAdded;
            }
        }
        if (talentButton.PointsInvested + currentPointsAdded == talentButton.MaxPointsToInvest) {
            ErrorMessage("You cannot spend any more points in this category!");
            return;
        }
        talentButton.SetTalentButtonText(talentButton.PointsInvested + (currentPointsAdded + 1));
        currentAddedTalentPoints.Add(talentButton.TalentPoint);

    }

    private void ResetButton() {
        int iterations;
        if (GameController.Instance.Player.HasAbility(PlayerAbility.VOIDMEND)) {
            iterations = 0;
        } else {
            iterations = 1;
        }
        for(int i = 0 ; i < talentPointButtons.Length - iterations; i++) {
            talentPointButtons[i].SetTalentButtonText(talentPointButtons[i].PointsInvested);
        }
        currentAddedTalentPoints.Clear();
    }

    private void DoneButton() {
        //KOLLA OM MAN HAR RÅD
        foreach(TalentPoint point in currentAddedTalentPoints) {
            TalentPointGainEvent talentGain = new TalentPointGainEvent {
                talentPoint = point
            };
            talentGain.FireEvent();
        }
        VoidTalentScreenEvent closeScreen = new VoidTalentScreenEvent { };
        closeScreen.FireEvent();
    }

    private void ErrorMessage(string errorMessage) {
        errorText.text = errorMessage;
        StopAllCoroutines();
        StartCoroutine(ResetErrorText());
    }

    private IEnumerator ResetErrorText() {
        yield return new WaitForSecondsRealtime(3);
        errorText.text = "";
    }

    private void OnDisable() {
        currentAddedTalentPoints.Clear();
    }

    private void OnDestroy() {
        
    }
}
