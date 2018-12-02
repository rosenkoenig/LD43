using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Character {

    [SerializeField] PlayerMover _playerMover;
    [SerializeField] PlayerHeadBehaviour _playerHeadBehaviour;

    [SerializeField] float _interactRange;
    [SerializeField] LayerMask _interactLayerMask;

    [SerializeField] Animator arm;

    public PlayerMover GetPlayerMover { get { return _playerMover; } }
    public PlayerHeadBehaviour GetPlayerHeadBehaviour { get { return _playerHeadBehaviour; } }

    bool isInteractActive = true;

    ChildCharacter hoverChild;

    public void SetInteractActive()
    {
        isInteractActive = true;
    }
    
    public void Interact()
    {
        if (isInteractActive == false)
            return;

        //RaycastHit rcHit = new RaycastHit();
        //if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _interactLayerMask))
        //{
            //InterestPoint ip = rcHit.collider.GetComponentInParent<InterestPoint>();

            //ChildCharacter child = rcHit.collider.GetComponentInParent<ChildCharacter>();

            if (hoverChild)
            {
                hoverChild.Freeze(true);
                GameMaster.Instance.uIMaster.DisplayMenuInteractChild(hoverChild, SetInteractActive);
            }
            else if (hoverIp)
            {
                if(hoverIp.Interact(this))
                {
                    OnInteractionBegin();
                }
            }
        //}
    }

    void Update ()
    {
        UpdateInteractionDisplay();
        RefreshHoveredChild();
    }

    void RefreshHoveredChild()
    {
        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _interactLayerMask))
        {
            InterestPoint ip = rcHit.collider.GetComponentInParent<InterestPoint>();

            ChildCharacter child = rcHit.collider.GetComponentInParent<ChildCharacter>();

            if (child)
            {
                hoverChild = child;
            }
            else
                hoverChild = null;
        }
        else
            hoverChild = null;

        if (hoverChild != null)
            GameMaster.Instance.uIMaster.DisplayChildStatsMenu(hoverChild);
        else
            GameMaster.Instance.uIMaster.HideChildStatsMenu();
    }

    InterestPoint hoverIp = null;
    void UpdateInteractionDisplay ()
    {
        InterestPoint ip = GetHoveredIP();

        if (ip == hoverIp) return;

        if (ip)
        {
            hoverIp = ip;
            GameMaster.Instance.uIMaster.DisplayPlayerInteraction(true, ip.ipName);
        }
        else if (ip == null)
        {
            GameMaster.Instance.uIMaster.DisplayPlayerInteraction(false, "");
            hoverIp = ip;
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


    public InterestPoint GetHoveredIP ()
    {
        InterestPoint ip = null;

        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _interactLayerMask))
        {
            ip = rcHit.collider.GetComponentInParent<InterestPoint>();
            if (ip != null && (ip.onlyUsableByChild == true || (ip && ip.activity.IsAvailable() == false)))
                ip = null;

        }
            return ip;
    }

    public ChildCharacter GetHoveredChildCharacter ()
    {
        ChildCharacter child = null;

        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _interactLayerMask))
        {
            child = rcHit.collider.GetComponentInParent<ChildCharacter>();

        }
        return child;
    }

    bool hasStartedSlap = false;

    public void BeginSlap ()
    {
        if (GameMaster.Instance.uIMaster.childInteractionMenuIsDisplayed) return;

        hasStartedSlap = true;
        arm.Play("Prepare");
    }

    public void LaunchSlap ()
    {
        if (!hasStartedSlap) return;

        hasStartedSlap = false;
        arm.Play("SlapIt");
        ChildCharacter child = GetHoveredChildCharacter();
        if(child)
        {
            child.IsSlapped();
        }
    }

    public void CancelSlap ()
    {
        arm.Play("Idle");
    }
}
