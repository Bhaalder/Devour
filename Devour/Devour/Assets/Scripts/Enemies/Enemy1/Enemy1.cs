using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1 : Enemy
{


    // Start is called before the first frame update
    void Start()
    {

    }

    protected override void Awake() {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
        //Movement();
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
    }

}
