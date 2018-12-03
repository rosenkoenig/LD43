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

    ChildCharacter curChild;
    
    public Action HideMenuInteractChildEvent;

    public ActivityProgressInfo GetActivityProgressInfoRef { get { return _activityProgressInfoRef; } }
    public bool childInteractionMenuIsDisplayed {  get { return _childInteractionMenu.isActiveAndEnabled; } }

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

    private static void ShowOrHideCursor(bool b)
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
        if (playerInteractionPanel.isActiveAndEnabled == state) return;
        Debug.Log("popup " + state);
        if(state)
        {
            playerInteractionPanel.Init(new object[1] { actionName });
        }
        else
        {
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

    public void UpdateWalletUI (float money)
    {
        uiWallet.UpdateWalletText(money);
    }
}
