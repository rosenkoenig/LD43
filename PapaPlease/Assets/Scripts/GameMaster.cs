using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
    public VirtualMother vm;
    public HouseMaster hm;

	// Use this for initialization
    void Awake ()
    {
        vm.gm = this;
        hm.gm = this;
        vm.Init();
    }

	void Start () {
        StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame ()
    {
        //  vm.GiveBirth();
    }


}
