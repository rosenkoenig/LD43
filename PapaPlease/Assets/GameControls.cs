using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour {

    [SerializeField] PlayerBehaviour _player;

    bool _moveFrozen = false;
    bool _headFrozen = false;
    bool _interactionControlsFrozen = false;
    
    public void FreezeMove(bool b)
    {
        _moveFrozen = b;
    }

    public void FreezeHead(bool b)
    {
        _headFrozen = b;
    }

    public void FreezeAllControls(bool b)
    {
        _interactionControlsFrozen = b;
        FreezeMove(b);
        FreezeHead(b);
    }

    void FixedUpdate()
    {
        ManageMove();
        ManageHead();
    }

    private void ManageHead()
    {
        if (_headFrozen == false)
        {
            Vector2 rotateAxis = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            if (rotateAxis != Vector2.zero)
                _player.GetPlayerHeadBehaviour.RotateHead(rotateAxis);
        }
    }

    private void ManageMove()
    {
        if (_moveFrozen == false)
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            if (dir != Vector3.zero)
                _player.GetPlayerMover.Move(dir.normalized);
        }
    }

    void Update()
    {
        ManageInteraction();
        DebugPlayChildInteraction();
    }

    private void DebugPlayChildInteraction()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            GameMaster.Instance.uIMaster.DisplayMenuInteractChild();
        }
    }

    private void ManageInteraction()
    {
        if(Input.GetButtonDown("Interact"))
        {
            _player.Interact();
        }
    }
}
