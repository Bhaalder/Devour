using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum Enemy2State
{
    NONE, IDLE, MOVEMENT, HURT, DEATH
}
public class Enemy2 : Enemy
{
    [SerializeField] private bool idleOnly;
    public Vector2 StartPosition { get; set; }
    public bool IdleOnly { get; set; }
    public AudioSource FlySoundAudioSource { get; set; }
    public Enemy2State State { get; set; }
    public Animator Animator { get; set; }

    protected override void Awake() {
        base.Awake();
        StartPosition = rb.position;
        IdleOnly = idleOnly;
        Animator = GetComponent<Animator>();
    }
    void Start()
    {
        Player = GameController.Instance.Player;
        AudioGO = transform.Find("Audio").gameObject;
        FlySoundAudioSource = GetComponentInChildren<AudioSource>();
        Invoke("FlySound", Random.Range(0.2f, 0.9f));
    }

    protected override void Update()
    {
        base.Update();
        //Animator.SetInteger("State", (int)State);
    }

    public override Vector2 KnockBack(Vector2 knockback) {
        return knockback/2;
    }

    private void FlySound() {
        AudioPlaySoundAtLocationEvent flySound = new AudioPlaySoundAtLocationEvent {
            name = "Enemy2Fly",
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1,
            soundType = SoundType.SFX,
            gameObject = transform.Find("Head").gameObject
        };
        flySound.FireEvent();
    }

}
