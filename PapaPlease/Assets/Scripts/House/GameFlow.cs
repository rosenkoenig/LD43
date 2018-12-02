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
        SetGameState(GameState.DAY);
    }

    public void OnDayEnds ()
    {
        SetGameState(GameState.NIGHT_TRANSITION);
    }

    public void BeginTablePhase ()
    {
        SetGameState(GameState.TABLE);
    }

    public void EndTablePhase ()
    {
        SetGameState(GameState.MORNING_TRANSITIOn);
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