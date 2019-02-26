﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofle;

public class OtherEnemy : MonoBehaviour
{
    public int _hp = 2;
    public enum eSprite
    {
        Idle,
        Run
    }
    // Start is called before the first frame update
    [SerializeField]
    private Rigidbody2D _rigidbody = null;
    [SerializeField]
    private SpriteRenderer _spriteIdle = null;
    [SerializeField]
    private SpriteRenderer _spriteRun = null;

    private StateMachine<OtherEnemy> _stateMachine = null;
    [SerializeField]
    private float _speed = 5.0f;
    private GameObject obj_player;
    public float player_pos;
    public float distance;

    void Start()
    {
        _stateMachine = new StateMachine<OtherEnemy>(this);
        StartCoroutine(_stateMachine.Coroutine<IdleState>());
        if(this.transform.root.name == "P1"){
            obj_player = GameObject.Find("Player1");
        }
        else if(this.transform.root.name == "P2"){
            obj_player = GameObject.Find("Player2");
        }
        Debug.Log(obj_player);
    }

    // Update is called once per frame
    void Update()
    {
            player_pos = obj_player.transform.position.x;
            distance = player_pos - transform.position.x;
            // Debug.Log(distance);
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Camera") {
            Debug.Log("Camera");
            Destroy(this.gameObject);
        }
    }

    private void Move(bool bLeft)
    {
        _rigidbody.AddForce(new Vector2((bLeft ? -_speed : _speed), 0));
    }
    private void LookAt(bool bLeft)
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, bLeft ? 180 : 0, transform.rotation.z));
    }

    private void HidleAllSprite()
    {
        _spriteIdle.enabled = false;
        _spriteRun.enabled = false;
    }
    private void ShowSprite(eSprite type)
    {
        HidleAllSprite();
        switch (type)
        {
            case eSprite.Idle:
                _spriteIdle.enabled = true;
                break;
            case eSprite.Run:
                _spriteRun.enabled = true;
                break;
        }
    }
    private class IdleState : State<OtherEnemy>
    {
        protected override void Begin()
        {
            Owner.ShowSprite(eSprite.Idle);
        }
        protected override void Update()
        {
            if (Owner.distance > -10)
            {
                Owner.LookAt(true);
                Invoke<RunState>();
            }
           
        }
        protected override void End()
        {

        }
    }
    private class RunState : State<OtherEnemy>
    {
        protected override void Begin()
        {
            Owner.ShowSprite(eSprite.Run);
        }
        protected override void Update()
        {
            if (Owner.distance < -3 && Owner.distance > -9)
            {
                Owner.Move(true);
                Owner.LookAt(true);
            }else
            {
                Invoke<IdleState>();
            }
            
        }
        protected override void End()
        {

        }
    }
}
