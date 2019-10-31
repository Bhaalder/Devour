using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy2 : Enemy
{
    public Vector2 StartPosition { get; set; }

    protected override void Awake() {
        base.Awake();
        StartPosition = rb.position;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override Vector2 KnockBack(Vector2 knockback) {
        return knockback/2;
    }

}
