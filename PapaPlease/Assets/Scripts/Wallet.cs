﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour {

    [SerializeField]
    float startMoney = 100f;

    [SerializeField]
    float foodCost = 2f;

    float _money = 0f;


    public void Init()
    {
        _money = startMoney;
        GameMaster.Instance.uIMaster.UpdateWalletUI(_money);
    }

    public float Money { get { return _money; } }

    public void SpendMoney (float delta)
    {
        if(HasEnoughMoneyFor(Mathf.Abs(delta)))
        {
            _money -= Mathf.Abs(delta);
            GameMaster.Instance.uIMaster.UpdateWalletUI(_money);
        }
    }

    public void EarnMoney (float delta)
    {
        _money += delta;
        GameMaster.Instance.uIMaster.UpdateWalletUI(_money);
    }

    public bool HasEnoughMoneyFor (float delta)
    {
        return _money - Mathf.Abs(delta) >= 0f;
    }

    public bool TryBuyFood ()
    {
        bool can = HasEnoughMoneyFor(foodCost);

        if(can)
        {
            SpendMoney(foodCost);
        }

        return can;
    }
}