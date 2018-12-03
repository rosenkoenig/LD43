using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWallet : MonoBehaviour {

    [SerializeField]
    Text text;


    [SerializeField]
    AnimateScale updateMoneyAnim = null;


    public void UpdateWalletText (float money)
    {
        text.text = money.ToString();
        updateMoneyAnim.StartAnimating();
    }
}
