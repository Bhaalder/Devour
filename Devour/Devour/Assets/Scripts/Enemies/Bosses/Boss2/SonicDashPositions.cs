//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicDashPositions : MonoBehaviour
{
    [SerializeField] private GameObject[] positions;
    public GameObject[] Positions { get; set; }

    private void Start()
    {
        Positions = positions;
    }
}
