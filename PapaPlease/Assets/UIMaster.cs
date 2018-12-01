using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    [SerializeField] GameObject _menuInteractChild;
    [SerializeField] GameObject _centerCursor;

    private void Start()
    {
        ShowOrHideCursor(false);
    }
    
    public bool DisplayMenuInteractChild()
    {
        if(_menuInteractChild.gameObject.activeSelf == false)
        { 
            _menuInteractChild.gameObject.SetActive(true);
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
        _menuInteractChild.gameObject.SetActive(false);
        ShowOrHideCursor(false);
        _centerCursor.SetActive(true);
        GameMaster.Instance.player.GetPlayerMover.FreezeMovement(false);
        GameMaster.Instance.player.GetPlayerHeadBehaviour.SetFreezeHeadControl(false);
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
}
