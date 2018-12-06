using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityProgressInfo : MonoBehaviour
{

    [SerializeField] Transform _gauge;
    [SerializeField] Transform _gaugeParent;
    [SerializeField] Vector3 _scaleForChild;
    [SerializeField] Vector3 _scaleForPlayer;
    [SerializeField] Vector3 _playerHeadDistance;
    public Vector3 GetPlayerHeadDistance { get { return _playerHeadDistance; } }
    
    public void SetScaleForChild()
    {
        transform.localScale = _scaleForChild;
    }

    public void SetScaleForPlayer()
    {
        transform.localScale = _scaleForPlayer;
    }

    internal void Refresh(float getCompletionRatio)
    {
        //_gauge.sizeDelta = new Vector2(_gaugeParent.sizeDelta.x * getCompletionRatio,
        //            _gauge.sizeDelta.y);
        _gauge.localScale = new Vector3(getCompletionRatio - 0.001f, _gauge.localScale.y, _gauge.localScale.z);
    }
}
