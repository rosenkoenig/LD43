﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
    public VirtualMother vm;
    public HouseMaster hm;
    public UIMaster uIMaster;
    public PlayerBehaviour player;

    static GameMaster _instance;

    public static GameMaster Instance { get { return _instance; } }

	// Use this for initialization
    void Awake ()
    {
        _instance = this;
        vm.gm = this;
        hm.gm = this;
        vm.Init();
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    void Start () {
        StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame ()
    {
        vm.SpawnBaseChilds();
    }


}
