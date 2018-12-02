using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShakeOptions
{
    public float smoothFactor = 0.8f, duration = 1f;
    public Vector3 shakesPerSec, startAmplitudes, endAmplitudes;
    public bool ignoreTimeScale = false;
    public AnimationCurve curve;
    public bool inverseCurve = false;
}

public class CameraShaker : MonoBehaviour {

    ShakeOptions currentShakeOptions = null;

    [SerializeField]
    ShakeOptions defaultShakeOptions;

    public enum Axis { X, Y, Z }
    float shakeTimer = 0f;
    Coroutine shakeCoroutine = null;
    Vector3 startPos;
    Vector3 shakeTimers;

    public void LaunchShake() { LaunchShake(null); }
    public void LaunchShake (ShakeOptions shakeOption)
    {
        if(shakeOption == null)
        {
            currentShakeOptions = defaultShakeOptions;
        }
        else
        {
            currentShakeOptions = shakeOption;
        }


        Reset();
        shakeCoroutine = StartCoroutine(Shake());
    }

    public void Reset ()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.position = startPos;
        }
    }

    IEnumerator Shake ()
    {
        float x = 0f, y = 0f, z = 0f;
        shakeTimer = 0f;
        startPos = transform.position;
        shakeTimers = Vector3.zero;

        Vector3 oneShakeDurations = new Vector3(1f/currentShakeOptions.shakesPerSec.x, 1f / currentShakeOptions.shakesPerSec.y, 1f / currentShakeOptions.shakesPerSec.z);

        while (shakeTimer <= currentShakeOptions.duration)
        {
            float ratioX = GetAmplitudeFactor(oneShakeDurations.x, Axis.X);
            float ratioY = GetAmplitudeFactor(oneShakeDurations.y, Axis.Y);
            float ratioZ = GetAmplitudeFactor(oneShakeDurations.z, Axis.Z);

            float timeRatio = Mathf.InverseLerp(0f, currentShakeOptions.duration, shakeTimer);

            float amplitudeX = Mathf.Lerp(currentShakeOptions.startAmplitudes.x, currentShakeOptions.endAmplitudes.x, currentShakeOptions.curve.Evaluate(currentShakeOptions.inverseCurve ? 1f - timeRatio : timeRatio));
            float amplitudeY = Mathf.Lerp(currentShakeOptions.startAmplitudes.y, currentShakeOptions.endAmplitudes.y, currentShakeOptions.curve.Evaluate(currentShakeOptions.inverseCurve ? 1f - timeRatio : timeRatio));
            float amplitudeZ = Mathf.Lerp(currentShakeOptions.startAmplitudes.z, currentShakeOptions.endAmplitudes.z, currentShakeOptions.curve.Evaluate(currentShakeOptions.inverseCurve ? 1f - timeRatio : timeRatio));

            x = ratioX * amplitudeX;
            y = ratioY * amplitudeY;
            z = ratioZ * amplitudeZ;

            Vector3 targetPos = new Vector3(startPos.x + x, startPos.y + y, startPos.z + z);

            transform.position = Vector3.Lerp(transform.position, targetPos, currentShakeOptions.smoothFactor);

            shakeTimer += currentShakeOptions.ignoreTimeScale ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }        

        transform.position = startPos;
    }

    #region PerAxisUtils
    float GetAmplitudeFactor (float oneShakeDuration, Axis axis)
    {
        float ampFactor = 0f;
        float oneShakeTimer = GetShakeTimer(axis);
                
        oneShakeTimer += currentShakeOptions.ignoreTimeScale ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;

        if (oneShakeTimer >= oneShakeDuration)
        {
            oneShakeTimer = 0f;
        }

        SetShakeTimer(axis, oneShakeTimer);

        float oneShakeRatio = Mathf.InverseLerp(0f, oneShakeDuration, oneShakeTimer);

        float sin = Mathf.Sin(Mathf.Lerp(0f, 2f * Mathf.PI, oneShakeRatio));

        ampFactor = sin;

        return ampFactor;
    }

    void SetShakeTimer (Axis axis, float timer)
    {
        switch (axis)
        {
            case Axis.X:
                shakeTimers.x = timer;
                break;
            case Axis.Y:
                shakeTimers.y = timer;
                break;
            case Axis.Z:
                shakeTimers.z = timer;
                break;
        }
    }

    float GetShakeTimer (Axis axis)
    {
        float timer = 0f;
        switch (axis)
        {
            case Axis.X:
                timer = shakeTimers.x;
                break;
            case Axis.Y:
                timer = shakeTimers.y;
                break;
            case Axis.Z:
                timer = shakeTimers.z;
                break;
        }

        return timer;
    }
    #endregion

}
