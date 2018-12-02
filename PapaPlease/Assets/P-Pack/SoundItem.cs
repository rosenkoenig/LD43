using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundItem : MonoBehaviour {

    public AudioSource src = null;

    public AudioClip[] clips = null;

    AudioClip[] currentClips = null;

    public enum Modes
    {
        Once,
        Random,
        Sequence,
        LoopingSequence,
        PingPongSequence,
        Mix
    }
    public Modes mode = Modes.Once;

    public bool loop = false;

    [Tooltip("Only applies when the loop check box is checked.")]
    public float delayBetweenLoop = 0f;

    public bool playOnEnable = false;

    int sequenceIdx = 0;
    int sequenceSign = 1;

    Coroutine loopCoroutine = null;

    bool isPaused;

    void Awake ()
    {
        if (src == null)
        {
            src = GetComponentInChildren<AudioSource>();
        }
        else
        {
            src = gameObject.AddComponent<AudioSource>();
        }

        src.playOnAwake = false;
        src.loop = false;
        src.clip = null;
    }

    void OnEnable ()
    {
        if (playOnEnable) PlaySound();
    }

    public void StopSound ()
    {
        src.Stop();
        StopCoroutine(loopCoroutine);
        isPaused = false;
    }

    public void StopAndResetSound ()
    {
        src.Stop();
        sequenceIdx = 0;
        currentClips = null;
    }

    public void PauseSound ()
    {
        src.Pause();
        isPaused = true;
    }

    public void UnPauseSound ()
    {
        src.UnPause();
        isPaused = false;
    }

    public void PlaySound ()
    {
        if(isPaused)
        {
            isPaused = false;
            StopCoroutine(loopCoroutine);
        }

        if (clips != null)
        {
            AttributeClip();
        }

        if (currentClips == null) return;

        float[] mixVolumes = null;
        if(mode == Modes.Mix)
        {
            mixVolumes = GetMixVolumes();
        }
        
        for (int i = 0; i < currentClips.Length; i++)
        {
            if (mode == Modes.Mix)
            {
                src.PlayOneShot(currentClips[i], mixVolumes[i]);
                    
            }
            else
            {
                src.PlayOneShot(currentClips[i]);
            }
            Debug.Log(currentClips[i].name);
        }

        if (loop) loopCoroutine = StartCoroutine(waitSoundIsOverToLoop(GetSoundDuration()));
        
    }

    void AttributeClip()
    {
        switch(mode)
        {
            case Modes.Once:
                currentClips = new AudioClip[] { clips[0] };
                break;
            case Modes.Sequence:
                sequenceIdx++;
                
                if(sequenceIdx < clips.Length)
                {
                    currentClips = new AudioClip[] { clips[sequenceIdx] };
                }
                else
                {
                    currentClips = null;
                }
                break;
            case Modes.LoopingSequence:
                sequenceIdx++;
                if (sequenceIdx >= clips.Length) sequenceIdx = 0;
                currentClips = new AudioClip[] { clips[sequenceIdx] };
                break;
            case Modes.PingPongSequence:
                if (sequenceIdx + sequenceSign >= clips.Length)
                {
                    sequenceSign *= -1;
                }
                else if(sequenceIdx + sequenceSign < 0)
                {
                    sequenceSign *= -1;
                }

                sequenceIdx += sequenceSign;

                currentClips = new AudioClip[] { clips[sequenceIdx] };
                break;
            case Modes.Random:
                currentClips = new AudioClip[] { clips[Random.Range(0, clips.Length)] };
                break;
            case Modes.Mix:
                currentClips = clips;
                break;
        }
    }

    float[] GetMixVolumes ()
    {
        float[] res = new float[currentClips.Length];

        float total = 0f;
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = Random.Range(0f, 100f);
            total += res[i];
        }

        for (int i = 0; i < res.Length; i++)
        {
            res[i] /= total;
        }
        Debug.Log(total.ToString());
        return res;
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(new Vector2(0f,0f), new Vector2(200f, 30f)), "Play Sound"))
        {
            PlaySound();
        }

        if (GUI.Button(new Rect(new Vector2(0f, 35f), new Vector2(200f, 30f)), "PauseSound"))
        {
            PauseSound();
        }

        if (GUI.Button(new Rect(new Vector2(0f, 70f), new Vector2(200f, 30f)), "UnPauseSound"))
        {
            UnPauseSound();
        }

        if (GUI.Button(new Rect(new Vector2(0f, 105f), new Vector2(200f, 30f)), "StopSound"))
        {
            StopSound();
        }
    }

    float GetSoundDuration ()
    {
        float longest = 0f;

        for (int i = 0; i < currentClips.Length; i++)
        {
            if(currentClips[i].length >= longest)
            {
                longest = currentClips[i].length;
            }
        }

        return longest;
    }

    IEnumerator waitSoundIsOverToLoop (float soundDuration)
    {
        float timer = 0f;
        Debug.Log("start coroutine");
        while (timer < (soundDuration + delayBetweenLoop))
        {
            if(!isPaused)
                timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        PlaySound();
    }
}
