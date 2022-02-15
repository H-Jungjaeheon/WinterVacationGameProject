using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] musicSource;
    public AudioSource[] SoundSource;
    void Awake()
    {
        for (int i = 0; i < musicSource.Length; i++)
            musicSource[i].volume = GameObject.Find("BG Slider").GetComponent<Slider>().value;
        for (int i = 0; i < SoundSource.Length; i++)
            SoundSource[i].volume = GameObject.Find("SE Slider").GetComponent<Slider>().value;
        this.gameObject.SetActive(false);
    }
    public void SetMusicVolume(float volume)
    {
        for(int i = 0; i<musicSource.Length;i++)
        {
           musicSource[i].volume = volume;  
        }
    }
    public void SetSoundEffect(float volume)
    {
        for (int i = 0; i < SoundSource.Length; i++)
        {
            SoundSource[i].volume = volume;
        }
    }
}
