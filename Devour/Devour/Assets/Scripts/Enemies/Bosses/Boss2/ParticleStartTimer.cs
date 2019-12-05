using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStartTimer : MonoBehaviour
{
    [SerializeField] private float startTimer;

    public float StartTimer { get; set; } = 1f;

    private ParticleSystem[] particle;

    void Start()
    {
        particle = GetComponentsInChildren<ParticleSystem>();


        if (startTimer > 0)
        {
            StartTimer = startTimer;
        }
        else
        {
            startTimer = StartTimer;
        }
    }

    void Update()
    {
        StartParticleTimer();
    }
    private void StartParticleTimer()
    {
        StartTimer -= Time.deltaTime;

        if (StartTimer > 0)
        {
            return;
        }

        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].Play();
        }

    }
}
