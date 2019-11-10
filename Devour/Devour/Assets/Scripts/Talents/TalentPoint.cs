//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalentPointType {
    DAMAGE, SURVIVAL, SPEED, VOID
}

[System.Serializable]
public class TalentPointChange {
    public string variableChangeName;
    public float amount;

    public TalentPointChange(float amount) {
        this.amount = amount;
    }

}

[System.Serializable]
public class TalentPoint {

    public TalentPointType talentPointType; //{ get; set; }
    public TalentPointChange[] variablesToChange;
    [TextArea(0, 5)]
    public string description;
    public Collectible[] collectibleCost;

    public TalentPoint(TalentPointType talentPointType, TalentPointChange[] variablesToChange) {
        this.talentPointType = talentPointType;
        this.variablesToChange = variablesToChange;
    }

}
