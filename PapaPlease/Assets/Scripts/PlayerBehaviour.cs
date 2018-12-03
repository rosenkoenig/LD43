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

    [SerializeField]
    ChildCharacter hoverChild;
    
    public void SetInteractActive()
    {
        SetInteractActive(true);
    }
    public void SetInteractActive(bool state)
    {
        isInteractActive = state;
    }
    
    public void Interact()
    {
        if (isInteractActive == false)
            return;
        
        DoorLocked hoveredDoor = GetHoveredDoor();
        PlateObject hoveredPlate = GetHoveredPlate();

        if (hoverChild)
        {

            if (hoverChild.ChildState != ChildAIState.AT_TABLE)
            {
                hoverChild.Freeze(true);
                SetInteractActive(false);
                GameMaster.Instance.uIMaster.DisplayMenuInteractChild(hoverChild, SetInteractActive);
            }
        }
        else if (hoverIp)
        {
            if (hoverIp.Interact(this))
            {
                OnInteractionBegin();
            }
        }
        else if (hoveredDoor)
        {
            if(GameMaster.Instance.gf.GetGameState == GameState.TABLE)
            {
                GameMaster.Instance.PlayerWantsToEndTablePhase();
            }
        }
        else if (hoveredPlate)
        {
            hoveredPlate.Fill();
        }
    }

    void Update ()
    {
        UpdateInteractionDisplay();
        RefreshHoveredChild();
    }

    void RefreshHoveredChild()
    {
        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _childLayerMask))
        {
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

    public void LockMovement (bool state)
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

    [SerializeField] LayerMask _childLayerMask;
    public ChildCharacter GetHoveredChildCharacter ()
    {
        ChildCharacter child = null;

        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _childLayerMask))
        {
            child = rcHit.collider.GetComponentInParent<ChildCharacter>();

        }
        return child;
    }

    [SerializeField] LayerMask _doorLayerMask;
    public DoorLocked GetHoveredDoor()
    {
        DoorLocked child = null;

        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _doorLayerMask))
        {
            child = rcHit.collider.GetComponentInParent<DoorLocked>();

        }
        return child;
    }

    [SerializeField] LayerMask _plateLayerMask;
    public PlateObject GetHoveredPlate ()
    {
        PlateObject child = null;

        RaycastHit rcHit = new RaycastHit();
        if (Physics.Raycast(_playerHeadBehaviour.GetCamera.transform.position, _playerHeadBehaviour.GetCamera.transform.forward, out rcHit, _interactRange, _plateLayerMask))
        {
            child = rcHit.collider.GetComponentInParent<PlateObject>();

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

    public void BeginDayPhase ()
    {
        transform.position = GameMaster.Instance.hm.playerDaySpawnPoint.position;
        transform.rotation = GameMaster.Instance.hm.playerDaySpawnPoint.rotation;

        SetInteractActive();
    }

    #region Table
    public void BeginTablePhase ()
    {
        transform.position = GameMaster.Instance.hm.playerTableSpawnPoint.position;
        transform.rotation = GameMaster.Instance.hm.playerTableSpawnPoint.rotation;

        SetInteractActive();
    }

    #endregion
}
