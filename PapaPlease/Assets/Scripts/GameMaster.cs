using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
    public VirtualMother vm;
    public HouseMaster hm;
    public UIMaster uIMaster;
    public PlayerBehaviour player;
    public GameFlow gf;

    static GameMaster _instance;

    public static GameMaster Instance { get { return _instance; } }

    public IPType ipTypeFun = null;

	// Use this for initialization
    void Awake ()
    {
        _instance = this;
        vm.gm = this;
        hm.gm = this;
        gf.gm = this;
        uIMaster.Init();
        gf.Init();
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
