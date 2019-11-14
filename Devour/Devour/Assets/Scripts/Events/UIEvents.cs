//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenEvent : Event<FadeScreenEvent> {

    public bool isFadeOut;
    public bool isFadeIn;

}

public class VoidTalentScreenEvent : Event<VoidTalentScreenEvent> {

}

public class ShowTipTextEvent : Event<ShowTipTextEvent> {

    public string tipText;
    public float tipDuration;
    public bool isOneTimeTip;

}

public class HideTipTextEvent : Event<HideTipTextEvent> {

}