﻿//Author: Patrik Ahlgren
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

    [Tooltip("The maximum number of points that can be invested in this talentpointcategory")]
    [SerializeField] private int maxPointsToInvest;
    [SerializeField] private TalentPoint talentPoint;

    private void Awake() {
        MaxPointsToInvest = maxPointsToInvest;
        PointsInvestedText = GetComponentInChildren<TextMeshProUGUI>();     
        TalentPoint = talentPoint;
    }

    private void OnEnable() {
        int points = 0;
        foreach(TalentPoint point in GameController.Instance.Player.TalentPoints) {
            if(TalentPoint.talentPointType == point.talentPointType) {
                points++;
            }
        }
        PointsInvested = points;
        SetTalentButtonText(PointsInvested);
    }

    public void SetTalentButtonText(int pointsInvested) {
        StringBuilder sb = new StringBuilder();
        string talentName = TalentPoint.talentPointType.ToString();
        bool isFirstLetter = true;
        foreach (char c in talentName) {
            if (!isFirstLetter) {
                sb.Append(char.ToLower(c));
            } else {
                sb.Append(c);
                isFirstLetter = false;
            }
        }
        PointsInvestedText.text = sb + "\n" + pointsInvested + "/" + MaxPointsToInvest;
    }

}
