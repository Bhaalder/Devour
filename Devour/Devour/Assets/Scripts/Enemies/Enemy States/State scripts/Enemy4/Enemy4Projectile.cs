using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Projectile : MonoBehaviour
{

    public float Count { get; set; }
    public Vector2 StartPoint { get; set; }
    public Vector2 EndPoint { get; set; }
    public Vector2 MiddlePoint { get; set; }

    private float countUp;


    void Start()
    {
        
    }

    void Update()
    {
        Parabola();
    }

    private void Parabola()
    {
        if (countUp < Count)
        {
            countUp += Count * Time.deltaTime;

            Vector2 m1 = Vector2.Lerp(StartPoint, MiddlePoint, countUp);
            Vector2 m2 = Vector2.Lerp(MiddlePoint, EndPoint, countUp);
            gameObject.transform.position = Vector3.Lerp(m1, m2, countUp);
        }

    }
}   
