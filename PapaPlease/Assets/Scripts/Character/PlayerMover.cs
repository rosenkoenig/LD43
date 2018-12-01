﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidB;
    [SerializeField] float _moveSpeed;

    private void FixedUpdate()
    {
        _rigidB.velocity = new Vector3(0, (_rigidB.velocity.y <= 0 ? _rigidB.velocity.y : 0), 0);
    }

    public void Move(Vector3 dir)
    {
        _rigidB.MovePosition(transform.position + (transform.right * dir.x + transform.forward * dir.z).normalized * _moveSpeed * Time.fixedDeltaTime);
    }
}