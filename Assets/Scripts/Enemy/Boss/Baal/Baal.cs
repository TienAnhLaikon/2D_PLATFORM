using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Baal : Boss
{
    public override void Attack()
    {
        base.Attack();
    }

    public override void Chase()
    {
        base.Chase();
    }

    public override void FlipDirection()
    {
        base.FlipDirection();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Patrol()
    {
        base.Patrol();
    }

    // Start is called before the first frame update
    public new void Awake()
    {
        base.Awake();
    }
    
    public new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
