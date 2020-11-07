using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nova : Star
{
    private Animator m_animator;
    private SpaceShipController spaceShip;
    
    protected override void Awake()
    {
        base.Awake();
        spaceShip = GameObject.FindWithTag("Player").GetComponent<SpaceShipController>();
        m_animator = GetComponent<Animator>();
        m_animator.speed = 0.0f;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        m_animator.speed = spaceShip.Speed;
        if (other.gameObject.tag == "Player")
        {
            isOn = true;
        }
    }
}
