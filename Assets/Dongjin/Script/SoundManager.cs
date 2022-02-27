using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] musicSource;
    public AudioSource[] SoundSource;
    [SerializeField] GameObject soundpanal;
    void Awake()
    {
        soundpanal.SetActive(true);
        for (int i = 0; i < musicSource.Length; i++)
            musicSource[i].volume = soundpanal.transform.GetChild(0).transform.GetChild(0).GetComponent<Slider>().value;
        for (int i = 0; i < SoundSource.Length; i++)
            SoundSource[i].volume = soundpanal.transform.GetChild(0).transform.GetChild(0).GetComponent<Slider>().value;
        soundpanal.SetActive(false);
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
    public void musicSound(int soundidx)
    {
        for (int i = 0; i < musicSource.Length; i++)
            musicSource[i].GetComponent<AudioSource>().Stop();
        musicSource[soundidx].GetComponent<AudioSource>().Play();
    }
    public void SESound(int soundidx)
    {
        SoundSource[soundidx].GetComponent<AudioSource>().Play();
    }
}
