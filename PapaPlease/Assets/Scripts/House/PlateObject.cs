using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateObject : MonoBehaviour {

    [SerializeField]
    GameObject isFullFeedback = null;


    bool _isFilled = false;
    public bool IsFilled { get { return _isFilled; } }

    [SerializeField]
    ChildStatsModificator eatStatModificator;

    [SerializeField] List<Collider> _colliders;

    [SerializeField]
    float eatingDuration = 1f;
    float eatTimer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init ()
    {
        isFullFeedback.SetActive(false);
    }

    public void DeactivateColliders()
    {
        foreach (var item in _colliders)
            item.enabled = false;
    }

    public void Fill ()
    {
        isFullFeedback.transform.localScale = Vector3.one;
        isFullFeedback.SetActive(true);

        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        delayCoroutine = StartCoroutine(waitDelayBeforeCanEat());
    }

    Coroutine delayCoroutine = null;
    IEnumerator waitDelayBeforeCanEat ()
    {
        yield return new WaitForSecondsRealtime(.5f);
        RealFill();
        delayCoroutine = null;
    }

    void RealFill()
    {
        _isFilled = true;
        eatTimer = 0f;
    }

    public void IsBeingEaten (ChildCharacter child)
    {
        if(eatTimer >= eatingDuration)
        {
            IsFullyEaten();
        }
        else
        {
            eatTimer += Time.deltaTime;
            UpdateFoodSize();
            eatStatModificator.TryModifyStats(child, true);
        }
    }

    void IsFullyEaten ()
    {
        _isFilled = false;
    }

    void UpdateFoodSize ()
    {
        isFullFeedback.transform.localScale = Vector3.one * (1f-EatRatio);
    }

    public float EatRatio { get { return Mathf.Clamp01(eatTimer / eatingDuration); } }
}
