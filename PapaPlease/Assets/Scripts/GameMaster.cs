﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    public VirtualMother vm;
    public HouseMaster hm;
    public UIMaster uIMaster;
    public PlayerBehaviour player;
    public GameFlow gf;
    public Wallet wallet;
    public LogMaster log;
    public MissionMaster mm;

    static GameMaster _instance;

    public static GameMaster Instance { get { return _instance; } }

    public IPType ipTypeFun = null;
    public ChildStatID healthStat = null;

    public float GetAllBillsCost { get { return wallet.GetAllBillsCost; } } 

    // Use this for initialization
    void Awake ()
    {
        _instance = this;
        vm.gm = this;
        hm.gm = this;
        gf.gm = this;
        if(log) log.Init();
        uIMaster.Init();
        vm.Init();
        wallet.Init();
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    void Start () {
        if(SceneManager.GetSceneByName("MenuScene").isLoaded)
            SceneManager.UnloadSceneAsync("MenuScene");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameObject.scene.name));
        StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame ()
    {
        vm.SpawnBaseChilds();

        hm.Init();
        gf.Init();
    }

    public void PlayerWantsToEndTablePhase ()
    {
        hm.SetDoorLockedClosed(false);
        gf.EndTablePhase();
    }

    public void AddLog(string text, bool isUrgent = false)
    {
        if (log)
            log.AddLog(text, isUrgent);
    }

    public void SpendMoneyForBills ()
    {

    }
}
