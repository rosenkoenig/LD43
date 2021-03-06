﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCondom : Popup {

    [SerializeField]
    Text price = null;

    [SerializeField]
    Button YesButton = null;
    [SerializeField] AK.Wwise.Event BuyCondomYesSoundEvent = null;

    public override void Init(object[] args)
    {
        base.Init(args);

        price.text = ((float)args[0]).ToString() + " $";
        YesButton.interactable = GameMaster.Instance.wallet.CanBuyCondom;
        
    }

    public void OnButtonYes ()
    {
        if (BuyCondomYesSoundEvent != null)
            BuyCondomYesSoundEvent.Post(gameObject);
        GameMaster.Instance.wallet.BuyCondom();
        GameMaster.Instance.vm.hasCondom = true;
        GameMaster.Instance.uIMaster.OnHideCondomUI();
        ClosePopup();
    }

    public void OnButtonNo ()
    {
        GameMaster.Instance.uIMaster.OnHideCondomUI();
        ClosePopup();
    }



}
