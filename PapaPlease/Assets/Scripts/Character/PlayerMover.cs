using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidB;
    [SerializeField] float _moveSpeed;

    [SerializeField] AK.Wwise.Event _walkStart;
    [SerializeField] AK.Wwise.Event _walkStop;

    bool isMoveStarted = false;

    bool canMove = true;

    private void FixedUpdate()
    {
        _rigidB.velocity = new Vector3(0, (_rigidB.velocity.y <= 0 ? _rigidB.velocity.y : 0), 0);
    }

    public void Move(Vector3 dir)
    {
        if(canMove)
            _rigidB.MovePosition(transform.position + (transform.right * dir.x + transform.forward * dir.z).normalized * _moveSpeed * Time.fixedDeltaTime);
    }

    public void SwitchWalkSound(bool b)
    {
        if(b && isMoveStarted == false)
        {
        if (_walkStart != null && _walkStart.IsValid())
                _walkStart.Post(gameObject);
            isMoveStarted = true;
        }
        else if(b == false && isMoveStarted)
        {
        if (_walkStop != null && _walkStop.IsValid())
            _walkStop.Post(gameObject);
            isMoveStarted = false;
        }
    }

    public void FreezeMovement (bool v)
    {
        canMove = !v;
    }
}
