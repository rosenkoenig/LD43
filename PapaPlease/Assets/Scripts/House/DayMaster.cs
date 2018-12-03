using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayMaster : MonoBehaviour {

    [SerializeField]
    float dayDuration = 50f;
    float dayTime = 0f;

    public GameFlow gf;

    public System.Action onDayStarts, onDayEnds;

    public void Init ()
    {
        dayTime = dayDuration;
    }

	void Update()
    {
        if(gf.GetGameState == GameState.DAY)
            UpdateDayTime();
    }

    public void StartDay ()
    {
        dayTime = 0f;
        DayStarts();
    }

    void UpdateDayTime ()
    {
        dayTime += Time.deltaTime;

        CheckIfDayEnds();
    }

    void CheckIfDayEnds()
    {
        if(dayTime >= dayDuration)
        {
            FakeDayEnds();
        }
    }

    void DayStarts ()
    {
        gf.OnDayStarts();
        if (onDayStarts != null) onDayStarts();
    }


    void FakeDayEnds ()
    {
        if (waitActivitiesCoroutine == null) waitActivitiesCoroutine = StartCoroutine(waitAllAcitivitiesToComplete());

    }

    Coroutine waitActivitiesCoroutine = null;
    IEnumerator waitAllAcitivitiesToComplete()
    {
        while (gf.gm.hm.NoActivityIsRunning() == false)
        {
            yield return new WaitForEndOfFrame();
        }

        RealDayEnds();
        waitActivitiesCoroutine = null;
    }

    void RealDayEnds ()
    {
        gf.OnDayEnds();
        if (onDayEnds != null) onDayEnds();
    }

    public float GetRemainingDayTime ()
    {
        return dayDuration - dayTime;
    }

    public float GetDayTimeRatio ()
    {
        return dayTime / dayDuration;
    }
}
