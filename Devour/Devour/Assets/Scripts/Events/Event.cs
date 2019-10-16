//Authors: quill18creates@youtube & FuzzyHobo@github
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event<T> where T : Event<T> {

    public string Description;

    private bool hasFired;
    public delegate void EventListener(T info);
    private static event EventListener listeners;

    public static void RegisterListener(EventListener listener) {
        listeners += listener;
    }

    public static void UnRegisterListener(EventListener listener) {
        listeners -= listener;
    }

    public void FireEvent() {
        if (hasFired) {
            throw new System.Exception("This event has already fired, to prevent infinite loops you can't refire an event");
        }
        hasFired = true;
        listeners?.Invoke(this as T);
    }
}

