using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{

    public Transform needle = null;
    [SerializeField] Animation _anim;
    [SerializeField] AnimationClip _idleAnim;
    [SerializeField] AnimationClip _endOfDayAnim;
    [SerializeField] float _remainingTimeToScaleUpAndDown = 10f;
    //[SerializeField] float scaleToReachMin = 1.0f;
    //[SerializeField] float scaleToReachMax = 1.1f;
    //bool goingToScaleMax = false;
    
    //[SerializeField] float scaleSpeed = 0.1f;
    //[SerializeField] Transform transformToScale;


    public void UpdateDayCompletion(float ratio, float remainingTime)
    {
        float zRot = Mathf.Lerp(0f, 180f, ratio);
        needle.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, zRot));

        //float scaleChange = scaleSpeed * Time.deltaTime;

        //if (remainingTime < _remainingTimeToScaleUpAndDown && remainingTime > 0)
        //{
            //if (goingToScaleMax == false)
            //{
            //    if (transformToScale.localScale.x > scaleToReachMin)
            //        ChangeTransformScale(-scaleChange);
            //    else
            //        goingToScaleMax = true;
            //}
            //else
            //{
            //    if (transformToScale.localScale.x < scaleToReachMax)
            //        ChangeTransformScale(scaleChange);
            //    else
            //        goingToScaleMax = false;
            //}
            
            //else
            //{
            //    if (transformToScale.localScale.x > scaleToReachMin)
            //        ChangeTransformScale(-scaleChange);
            //    }
        //}
        if (remainingTime < _remainingTimeToScaleUpAndDown && remainingTime > 0)
        {
            if (_anim.IsPlaying(_endOfDayAnim.name) == false)
                _anim.Play(_endOfDayAnim.name);
        }
        else if (_anim.IsPlaying(_idleAnim.name) == false && goingToIdle == false)
            goingToIdle = true;
    }

    //private void ChangeTransformScale(float scaleChange)
    //{
    //    transformToScale.localScale = new Vector3(transformToScale.localScale.x + scaleChange, transformToScale.localScale.y + scaleChange,
    //        transformToScale.localScale.z);
    //}

    bool goingToIdle = false;

    void EndOfScaleAnimEvent()
    {
        if (goingToIdle)
        {
            _anim.Play(_idleAnim.name);
            goingToIdle = false;
        }
    }


    public void SetActive(bool state)
    {
        foreach (Graphic gr in GetComponentsInChildren<Graphic>())
        {
            gr.CrossFadeAlpha(state ? 1f : 0f, .5f, true);
        }
    }
}
