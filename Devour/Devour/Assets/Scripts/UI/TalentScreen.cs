//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentScreen : MonoBehaviour {

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
    private List<Collectible> currentCollectibleCosts;
    private List<Collectible> playerCollectibles;

    private void Awake() {
        currentAddedTalentPoints = new List<TalentPoint>();
        currentCollectibleCosts = new List<Collectible>();
        playerCollectibles = new List<Collectible>();
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
        SetCollectibleCostText();
        descriptionText.text = "";
        errorText.text = "";
        playerCollectibles = GameController.Instance.Player.Collectibles;
        bool hasVoidMend;
        if (GameController.Instance.Player.HasAbility(PlayerAbility.VOIDMEND)) {
            hasVoidMend = true;
        } else {
            hasVoidMend = false;
        }
        voidButton.gameObject.SetActive(hasVoidMend);
        resetButton.interactable = false;
    }

    private void OnGUI() {
        if (EventSystem.current.currentSelectedGameObject != null) {
            try {
                descriptionText.text = EventSystem.current.currentSelectedGameObject.GetComponent<TalentPointButton>().TalentPoint.description;
            } catch (System.NullReferenceException) {
                if (EventSystem.current.currentSelectedGameObject == resetButton.gameObject) {
                    descriptionText.text = resetButtonInfo;
                } else if (EventSystem.current.currentSelectedGameObject == doneButton.gameObject) {
                    descriptionText.text = doneButtonInfo;
                } else {
                    descriptionText.text = "";
                }
            }
        }
    }

    private void CheckSelectedButton(TalentPointButton talentButton) {
        int currentPointsAdded = 0;
        for (int i = 0; i < currentAddedTalentPoints.Count; i++) {
            if (currentAddedTalentPoints[i].talentPointType == talentButton.TalentPoint.talentPointType) {
                ++currentPointsAdded;
            }
        }
        if (talentButton.PointsInvested + currentPointsAdded == talentButton.MaxPointsToInvest) {
            ErrorMessage("You cannot spend any more points in the " + talentButton.TalentPoint.talentPointType + " category!");
            return;
        }
        for (int i = 0; i < talentButton.TalentPoint.collectibleCost.Length; i++) {
            currentCollectibleCosts.Add(talentButton.TalentPoint.collectibleCost[i]);
        }
        CollectibleType[] collectibles = { CollectibleType.LIFEFORCE, CollectibleType.VOIDESSENCE };
        if (!PlayerCanAffordUpgrade(ref playerCollectibles, ref currentCollectibleCosts, collectibles, talentButton)) {
            return;
        }
        talentButton.SetTalentButtonText(talentButton.PointsInvested + (currentPointsAdded + 1));
        currentAddedTalentPoints.Add(talentButton.TalentPoint);
        SetCollectibleCostText();
        resetButton.interactable = true;
    }



    private void ResetButton() {
        int lessIterations;
        if (GameController.Instance.Player.HasAbility(PlayerAbility.VOIDMEND)) {
            lessIterations = 0;
        } else {
            lessIterations = 1;
        }
        for (int i = 0; i < talentPointButtons.Length - lessIterations; i++) {
            talentPointButtons[i].SetTalentButtonText(talentPointButtons[i].PointsInvested);
        }
        currentAddedTalentPoints.Clear();
        currentCollectibleCosts.Clear();
        StopAllCoroutines();
        errorText.text = "";
        SetCollectibleCostText();
        EventSystem.current.SetSelectedGameObject(doneButton.gameObject);
        resetButton.interactable = false;
    }

    private void DoneButton() {
        Collectible voidReduction = new Collectible(CollectibleType.VOIDESSENCE, -CalculateCollectibles(ref currentCollectibleCosts, CollectibleType.VOIDESSENCE));
        Collectible lifeForceReduction = new Collectible(CollectibleType.LIFEFORCE, -CalculateCollectibles(ref currentCollectibleCosts, CollectibleType.LIFEFORCE));
        PlayerCollectibleChange voidChange = new PlayerCollectibleChange {
            collectible = voidReduction
        };
        PlayerCollectibleChange lifeForceChange = new PlayerCollectibleChange {
            collectible = lifeForceReduction
        };
        voidChange.FireEvent();
        lifeForceChange.FireEvent();
        for (int i = 0; i < currentAddedTalentPoints.Count; i++) {
            TalentPointGainEvent talentGain = new TalentPointGainEvent {
                talentPoint = currentAddedTalentPoints[i]
            };
            talentGain.FireEvent();
        }
        VoidTalentScreenEvent closeScreen = new VoidTalentScreenEvent { };
        closeScreen.FireEvent();
    }

    private void SetCollectibleCostText() {
        voidEssenceCostText.text = CalculateCollectibles(ref currentCollectibleCosts, CollectibleType.VOIDESSENCE) + "";
        lifeForceCostText.text = CalculateCollectibles(ref currentCollectibleCosts, CollectibleType.LIFEFORCE) + "";
    }

    private bool PlayerCanAffordUpgrade(ref List<Collectible> playerListToCompare, ref List<Collectible> currentCostToCompare, CollectibleType[] collectibleTypes, TalentPointButton talentButton) {
        for (int c = 0; c < collectibleTypes.Length; c++) {
            int playerCollectibleValue = CalculateCollectibles(ref playerListToCompare, collectibleTypes[c]);
            int currentCollectibleCost = CalculateCollectibles(ref currentCostToCompare, collectibleTypes[c]);
            string collectibleName;
            if (collectibleTypes[c] == CollectibleType.VOIDESSENCE) {
                collectibleName = "Void Essence(s)";
            } else {
                collectibleName = "Lifeforce";
            }
            if (playerCollectibleValue < currentCollectibleCost) {
                ErrorMessage("You cannot afford this! You have: " + playerCollectibleValue + " " + collectibleName + ", You need: " + currentCollectibleCost);
                for (int i = 0; i < talentButton.TalentPoint.collectibleCost.Length; i++) {
                    currentCollectibleCosts.Remove(talentButton.TalentPoint.collectibleCost[i]);
                }
                return false;
            }
        }
        return true;

    }
    private int CalculateCollectibles(ref List<Collectible> collectibles, CollectibleType collectibleType) {
        int playerVoid = 0;
        int playerLifeforce = 0;
        for (int i = 0; i < collectibles.Count; i++) {
            switch (collectibles[i].collectibleType) {
                case CollectibleType.VOIDESSENCE:
                    playerVoid += collectibles[i].amount;
                    break;
                case CollectibleType.LIFEFORCE:
                    playerLifeforce += collectibles[i].amount;
                    break;
            }
        }
        if (collectibleType == CollectibleType.VOIDESSENCE) {
            return playerVoid;
        } else {
            return playerLifeforce;
        }
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
        currentCollectibleCosts.Clear();
    }

    private void OnDestroy() {

    }
}
