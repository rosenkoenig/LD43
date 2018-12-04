using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInteraction : Popup {

    [SerializeField]
    Text text;

    public string currentActionName = "";

    public override void Init(object[] args)
    {
        base.Init(args);

        text.text = currentActionName = (string)args[0];
    }
}
