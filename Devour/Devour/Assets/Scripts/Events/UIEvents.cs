//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenEvent : Event<FadeScreenEvent> {

    public bool isFadeOut;
    public bool isFadeIn;
    public float fadeSpeed;

}

public class VoidTalentScreenEvent : Event<VoidTalentScreenEvent> {

}

public class InGameMenuEvent : Event<InGameMenuEvent>
{

}

public class MainMenuEvent : Event<MainMenuEvent> {

}

public class ShowTipTextEvent : Event<ShowTipTextEvent> {

    public string tipText;
    public float tipDuration;
    public bool isOneTimeTip;

}

public class HideTipTextEvent : Event<HideTipTextEvent> {

}