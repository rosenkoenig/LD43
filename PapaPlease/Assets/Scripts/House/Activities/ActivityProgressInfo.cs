using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityProgressInfo : MonoBehaviour
{

    [SerializeField] Transform _gauge;
    [SerializeField] Transform _gaugeParent;

    internal void Refresh(float getCompletionRatio)
    {
        //_gauge.sizeDelta = new Vector2(_gaugeParent.sizeDelta.x * getCompletionRatio,
        //            _gauge.sizeDelta.y);
        _gauge.localScale = new Vector3(getCompletionRatio, _gauge.localScale.y, _gauge.localScale.z);
    }
}
