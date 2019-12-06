//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class TalentPointButton : SelectedButton {

    public int MaxPointsToInvest { get; set; }
    public int PointsInvested { get; set; }
    public TalentPoint TalentPoint { get; set; }
    public TextMeshProUGUI PointsInvestedText { get; set; }
    public GameObject CostBox { get => costBox; set => costBox = value; }
    public TextMeshProUGUI CostVoidEssenceText { get => costVoidEssenceText; set => costVoidEssenceText = value; }
    public TextMeshProUGUI CostLifeforceText { get => costLifeforceText; set => costLifeforceText = value; }

    [Tooltip("The maximum number of points that can be invested in this talentpointcategory")]
    [SerializeField] private int maxPointsToInvest;
    [SerializeField] private TalentPoint talentPoint;
    [SerializeField] private GameObject costBox;
    [SerializeField] private TextMeshProUGUI costVoidEssenceText;
    [SerializeField] private TextMeshProUGUI costLifeforceText;

    private void Awake() {
        MaxPointsToInvest = maxPointsToInvest;
        PointsInvestedText = GetComponentInChildren<TextMeshProUGUI>();     
        TalentPoint = talentPoint;
    }

    private void OnEnable() {
        int points = 0;
        for(int i = 0; i < GameController.Instance.Player.TalentPoints.Count; i++) {
            if (TalentPoint.talentPointType == GameController.Instance.Player.TalentPoints[i].talentPointType) {
                points++;
            }
        }
        PointsInvested = points;
        SetTalentButtonText(PointsInvested);
        CostVoidEssenceText.text = TalentPoint.collectibleCost[0].amount.ToString();
        CostLifeforceText.text = TalentPoint.collectibleCost[1].amount.ToString();
    }

    private void OnGUI() {
        if(EventSystem.current.currentSelectedGameObject != gameObject && CostBox.activeSelf) {
            CostBox.SetActive(false);
        }
    }

    public void SetTalentButtonText(int pointsInvested) {
        StringBuilder sb = new StringBuilder();
        string talentName = TalentPoint.talentPointType.ToString();
        bool isFirstLetter = true;
        for(int i = 0; i < talentName.Length; i++) {
            if (!isFirstLetter) {
                sb.Append(char.ToLower(talentName[i]));
            } else {
                sb.Append(talentName[i]);
                isFirstLetter = false;
            }
        }
        PointsInvestedText.text = sb + "\n" + pointsInvested + "/" + MaxPointsToInvest;
    }

}
