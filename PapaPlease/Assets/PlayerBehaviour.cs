using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Character {

    [SerializeField] PlayerMover _playerMover;
    [SerializeField] PlayerHeadBehaviour _playerHeadBehaviour;

    [SerializeField] float _interactRange;
    [SerializeField] LayerMask _interactLayerMask;

    public PlayerMover GetPlayerMover { get { return _playerMover; } }
    public PlayerHeadBehaviour GetPlayerHeadBehaviour { get { return _playerHeadBehaviour; } }

    public void Interact()
    {
        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _interactLayerMask))
        {
            Debug.Log("Paul ! J'interagis !");

            InterestPoint ip = rcHit.collider.GetComponent<InterestPoint>();
            if (ip)
            {
                if(ip.Interact(this))
                {
                    OnInteractionBegin();
                }
            }
        }
    }

    void OnInteractionBegin ()
    {
        LockMovement(true);
    }

    public override void OnActivityEnds()
    {
        base.OnActivityEnds();

        LockMovement(false);
    }

    void LockMovement (bool state)
    {
        _playerHeadBehaviour.SetFreezeHeadControl(state);
        _playerMover.FreezeMovement(state);
    }

}
