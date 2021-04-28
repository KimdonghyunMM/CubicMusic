using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sfx;
    public Sound[] bgm;

    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;

    private void Awake()
    {
        if (instance != this) instance = this;
    }
    public void PlayerBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_stxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_stxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 오디오 플레이어가 재생중이다데스");
                return;
            }
        }

        Debug.Log(p_stxName + "이름의 효과음이 없다데스");
    }
}
