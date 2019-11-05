using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy2 : Enemy
{
    [SerializeField] private bool idleOnly;
    public Vector2 StartPosition { get; set; }
    public bool IdleOnly { get; set; }
    public AudioSource FlySoundAudioSource { get; set; }

    protected override void Awake() {
        base.Awake();
        StartPosition = rb.position;
        IdleOnly = idleOnly;
    }
    // Start is called before the first frame update
    void Start()
    {
        FlySoundAudioSource = GetComponentInChildren<AudioSource>();
        Invoke("FlySound", Random.Range(0.2f, 0.9f));
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
