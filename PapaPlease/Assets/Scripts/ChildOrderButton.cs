using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildOrderButton : MonoBehaviour {

    public Button _button;
    public IPTypeInfo _ipTypeInfo;
    [SerializeField] Text _text;

    public void MakeAction()
    {
        PlayOrderEvent(_ipTypeInfo);
    }

    public System.Action<IPTypeInfo> PlayOrderEvent;

    internal void InitButton(IPTypeInfo ipTypeInfo, Action<IPTypeInfo> makeGiveOrder)
    {
        _ipTypeInfo = ipTypeInfo;
        _text.text = ipTypeInfo.IPType.GetOrderName;
        PlayOrderEvent = makeGiveOrder;
    }
}
