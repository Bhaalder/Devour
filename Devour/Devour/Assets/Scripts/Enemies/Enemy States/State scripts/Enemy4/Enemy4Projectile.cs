using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Projectile : MonoBehaviour
{

    [SerializeField] private float timeToDestruction = 2f;
    [SerializeField] private float damageToPLayer = 5f;
    public float Count { get; set; }
    public Vector2 StartPoint { get; set; }
    public Vector2 EndPoint { get; set; }
    public Vector2 MiddlePoint { get; set; }

    private float countUp;
    private float currentTimeToDestruction;


    void Start()
    {
        currentTimeToDestruction = timeToDestruction;
    }

    void Update()
    {
        Parabola();
        TimeToDestruction();
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
        else
        {
            Vector2 m1 = Vector2.Lerp(StartPoint, MiddlePoint, countUp);
            Vector2 m2 = Vector2.Lerp(MiddlePoint, EndPoint, countUp);
            gameObject.transform.position = Vector3.Lerp(m1, m2, countUp);
        }

        if (EndPoint.x < StartPoint.x)
        {
            EndPoint = new Vector2(EndPoint.x - 0.1f, EndPoint.y - 0.4f);
        }
        else if (EndPoint.x > StartPoint.x)
        {
            EndPoint = new Vector2(EndPoint.x + 0.1f, EndPoint.y - 0.4f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.tag == "Player")
        {
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = damageToPLayer,
                enemyPosition = gameObject.transform.position
            };
            ptde.FireEvent();
            Destruction();
        }
    }

    private void TimeToDestruction()
    {
        currentTimeToDestruction -= Time.deltaTime;

        if (currentTimeToDestruction > 0)
        {
            return;
        }

        Destruction();
    }

    private void Destruction()
    {
        Destroy(gameObject);
    }
}   
