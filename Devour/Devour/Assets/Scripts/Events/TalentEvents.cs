//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentPointGainEvent : Event<TalentPointGainEvent> {

    public TalentPoint talentPoint;

}

public class TalentPointLimitChangeEvent : Event<TalentPointLimitChangeEvent> { //FÖR SPELTEST 4

    public int amount;

}