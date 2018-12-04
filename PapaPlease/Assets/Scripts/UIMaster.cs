using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour
{
    [SerializeField] ChildInteractionMenu _childInteractionMenu;
    [SerializeField] ChildStatsMenu _childStatsMenu;

    [SerializeField] GameObject _centerCursor;
    [SerializeField]
    UIPlayerInteraction playerInteractionPanel;

    [SerializeField] ActivityProgressInfo _activityProgressInfoRef;

    [SerializeField] UIClock clockParent;

    [SerializeField] UITransition dayNightTransition;

    [SerializeField]UIWallet uiWallet;

    [SerializeField] UIMissionPanel missionPanel;

    [SerializeField] ChildInteractionMission _childInteractionMission;

    [SerializeField] UiCondom condomUI;

    [SerializeField] UIGameOver gameoverUI;

    ChildCharacter curChild;
    
    public Action HideMenuInteractChildEvent;

    public ActivityProgressInfo GetActivityProgressInfoRef { get { return _activityProgressInfoRef; } }
    public bool childInteractionMenuIsDisplayed {  get { return _childInteractionMenu.isActiveAndEnabled; } }
    public bool childInteractionMissionIsDisplayed {  get { return _childInteractionMission.IsDisplayed; } }

    public void Init ()
    {
        GameMaster.Instance.gf.dm.onDayStarts += OnDayStart;
        GameMaster.Instance.gf.dm.onDayEnds += OnDayEnds;
    }

    private void Start()
    {
        ShowOrHideCursor(false);
        DisplayPlayerInteraction(false, "");

    }

    void Update ()
    {
        clockParent.UpdateDayCompletion(GameMaster.Instance.gf.dm.GetDayTimeRatio());
    }

    public void OnDayStart ()
    {
        DisplayClock(true);
        dayNightTransition.End();
    }

    public void OnDayEnds ()
    {
        Debug.Log("Day Ends");
        DisplayClock(false);
        dayNightTransition.Begin();
    }

    public void OnTableStarts ()
    {

        dayNightTransition.End();
    }

    public void OnTableEnds ()
    {

        dayNightTransition.Begin();
    }

    public void DisplayClock (bool state)
    {
        clockParent.SetActive(state);
    }
    
    public void HideChildStatsMenu()
    {
        if(childInteractionMenuIsDisplayed == false && _childStatsMenu.gameObject.activeSelf)
            _childStatsMenu.gameObject.SetActive(false);
    }

    public bool DisplayMenuInteractChild(ChildCharacter child, Action hideMenuEvent)
    {
        HideMenuInteractChildEvent = hideMenuEvent;

        if (_childInteractionMenu.gameObject.activeSelf == false)
        {
            DisplayChildStatsMenu(child);
            _childInteractionMenu.gameObject.SetActive(true);
            _childInteractionMenu.SetupMenu(child);
            ShowOrHideCursor(true);
            _centerCursor.SetActive(false);
            GameMaster.Instance.player.GetPlayerMover.FreezeMovement(true);
            GameMaster.Instance.player.GetPlayerHeadBehaviour.SetFreezeHeadControl(true);
            return true;
        }
        return false;
    }
    
    public void HideMenuInteractChild()
    {
        _childInteractionMenu.gameObject.SetActive(false);
        ShowOrHideCursor(false);
        _centerCursor.SetActive(true);
        GameMaster.Instance.player.GetPlayerMover.FreezeMovement(false);
        GameMaster.Instance.player.GetPlayerHeadBehaviour.SetFreezeHeadControl(false);
        HideMenuInteractChildEvent();

        HideMenuInteractChildEvent = null;
    }

    public void ShowOrHideCursor(bool b)
    {
        if (b)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void DisplayPlayerInteraction (bool state, string actionName)
    {
        if (playerInteractionPanel.isActiveAndEnabled == state && playerInteractionPanel.currentActionName == actionName) return;
        if(state)
        {
            _centerCursor.SetActive(false);
            playerInteractionPanel.Init(new object[1] { actionName });
        }
        else
        {
            _centerCursor.SetActive(true);
            playerInteractionPanel.ClosePopup();
        }
    }

    public void DisplayChildStatsMenu(ChildCharacter child)
    {
        if (childInteractionMenuIsDisplayed == false)
        {
            if (_childStatsMenu.gameObject.activeSelf == false)
                _childStatsMenu.gameObject.SetActive(true);

            if (child != curChild)
            {
                curChild = child;
                _childStatsMenu.SetupMenu(child);
            }
        }
    }

    public void DisplayCondomUI ()
    {
        condomUI.Init(new object[1] { GameMaster.Instance.wallet.condomCost });
    }

    public void OnHideCondomUI ()
    {
        GameMaster.Instance.gf.EndNightTransition();
        GameMaster.Instance.uIMaster.ShowOrHideCursor(false);
    }

    public void UpdateWalletUI (float money)
    {
        uiWallet.UpdateWalletText(money);
    }

    public void DisplayMissionPanel ()
    {
        if(missionPanel.gameObject.activeInHierarchy == false)
        {
            missionPanel.gameObject.SetActive(true);
            missionPanel.Init(null);
            GameMaster.Instance.uIMaster.ShowOrHideCursor(true);
        }
    }

    public void OnMissionPanelCloses ()
    {
        GameMaster.Instance.mm.ApplyAll();
        DisplayCondomUI();
    }

    public bool DisplayChildInteractionMission(ChildCharacter child, Action hideMenuEvent)
    {
        HideMenuInteractChildEvent = hideMenuEvent;

        if (_childInteractionMission.IsDisplayed == false)
        {
            _childInteractionMission.Init(child);
            ShowOrHideCursor(true);
            _centerCursor.SetActive(false);
            GameMaster.Instance.player.GetPlayerMover.FreezeMovement(true);
            GameMaster.Instance.player.GetPlayerHeadBehaviour.SetFreezeHeadControl(true);
            return true;
        }
        return false;
    }

    public void HideChildInteractionMission ()
    {
        _childInteractionMission.CloseMenu();
        ShowOrHideCursor(false);
        _centerCursor.SetActive(true);
        GameMaster.Instance.player.GetPlayerMover.FreezeMovement(false);
        GameMaster.Instance.player.GetPlayerHeadBehaviour.SetFreezeHeadControl(false);
        HideMenuInteractChildEvent();

        HideMenuInteractChildEvent = null;
    }

    public void OnGameOver ()
    {
        gameoverUI.Display();
    }
}
