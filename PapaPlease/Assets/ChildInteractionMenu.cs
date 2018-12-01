using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildInteractionMenu : MonoBehaviour
{

    [SerializeField] ChildOrderButton _childOrderButtonRef;
    List<ChildOrderButton> _childOrderButtons;
    [SerializeField] RectTransform childOrderButtonsParent;
    List<IPTypeInfo> allIPTypeInfos;

    ChildCharacter curChild;

    private void Awake()
    {
        _childOrderButtons = new List<ChildOrderButton>();
        allIPTypeInfos = GameMaster.Instance.hm.GetAllIPTypeInfos();
        foreach (var item in allIPTypeInfos)
        {
            ChildOrderButton childOrderButton = Instantiate(_childOrderButtonRef, childOrderButtonsParent);
            childOrderButton.InitButton(item, MakeGiveOrder);
            _childOrderButtons.Add(childOrderButton);
        }
    }

    public void SetupMenu(ChildCharacter child)
    {
        curChild = child;
        for (int i = 0; i < allIPTypeInfos.Count; i++)
        {
            _childOrderButtons[i]._button.interactable = allIPTypeInfos[i].isAvailable;
        }
    }

    public void MakeGiveOrder(IPTypeInfo ipTypeInfo)
    {
        Debug.Log("Give Order " + ipTypeInfo.IPType.name + " to " + curChild.childName);
        curChild.GiveOrder(ipTypeInfo.IPType);
    }
}
