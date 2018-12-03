﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { TABLE, MORNING_TRANSITIOn, DAY, NIGHT_TRANSITION }
public class GameFlow : MonoBehaviour {
    public GameMaster gm;

    public DayMaster dm = null;

    [SerializeField]
    GameState _gameState = GameState.DAY;

    public GameState GetGameState {  get { return _gameState; } }

    float stateStartTime = 0f;

    [SerializeField]
    float morningTransitionDuration = 3f;

    [SerializeField]
    float nightTransitionDuration = 3f;

	public void Init ()
    {
        dm.gf = this;
        dm.Init();
    }

    public void OnDayStarts ()
    {
        Debug.Log("Day starts");
        GameMaster.Instance.log.AddLog("Day Starts");
        SetGameState(GameState.DAY);
        gm.hm.SetDoorLockedClosed(false);
        gm.player.BeginDayPhase();
        gm.player.SetInteractActive(true);
        gm.player.LockMovement(false);
        gm.vm.GetChildrenOutOfTable();
        gm.hm.ClearPlates();
    }

    public void OnDayEnds ()
    {
        GameMaster.Instance.log.AddLog("Day Ends");
        SetGameState(GameState.NIGHT_TRANSITION);
        gm.player.SetInteractActive(false);
        gm.player.LockMovement(true);
    }

    public void BeginTablePhase ()
    {
        SetGameState(GameState.TABLE);
        gm.uIMaster.OnTableStarts();
        gm.vm.GiveBirth();
        gm.player.BeginTablePhase();
        gm.hm.UpdateTableRoomSize();
        gm.hm.SetChildrenOnTable();
        gm.hm.SetDoorLockedClosed(true);
        gm.player.LockMovement(false);
        gm.player.SetInteractActive(true);
    }

    public void EndTablePhase ()
    {
        SetGameState(GameState.MORNING_TRANSITIOn);
        gm.uIMaster.OnTableEnds();
        gm.hm.SetDoorLockedClosed(false);
        gm.player.SetInteractActive(false);
        gm.player.LockMovement(true);
    }

    void SetGameState (GameState newState)
    {
        if(newState != _gameState)
        {
            stateStartTime = Time.time;
            _gameState = newState;
        }
    }

    void Update ()
    {
        switch(_gameState)
        {
            case GameState.MORNING_TRANSITIOn:
                UpdateMorningTransition();
                break;
            case GameState.NIGHT_TRANSITION:
                UpdateNightTransition();
                break;
        }
    }

    void UpdateMorningTransition ()
    {
        if(Time.time - stateStartTime >= morningTransitionDuration)
        {
            dm.StartDay();
        }
    }

    void UpdateNightTransition ()
    {
        if (Time.time - stateStartTime >= nightTransitionDuration)
        {
            BeginTablePhase();
        }
    }
}
