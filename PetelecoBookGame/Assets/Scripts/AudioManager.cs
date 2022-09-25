using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Enums;
using Assets.Constants;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Audios

	// Sources

    [SerializeField]
    private AudioSource MusicSource;

	[SerializeField]
	private AudioSource SoundSource;

	// Mixers groups

	[SerializeField]
	private AudioMixerGroup MusicGroup;

	[SerializeField]
	private AudioMixerGroup SoundGroup;

	// Audios clips

    [SerializeField]
    private AudioClip MainLoopClip;

	[SerializeField]
	private AudioClip StartBattleClip;

	[SerializeField]
	private AudioClip CalmLoopClip;

	[SerializeField]
	private AudioClip MemoryLoopClip;

	[SerializeField]
	private AudioClip BtnClickClip;

	#endregion

	#region SingletonControl

	private static AudioManager instance = null;

    public static AudioManager Instance
	{
        get { return instance; }
    }

	#endregion

	private AudioSources CurrentMusic = AudioSources.UNDEFINED;
	private AudioSources CurrentSound = AudioSources.UNDEFINED;

	// Starts the singleon
	void Awake()
    {
		if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
			instance = this;
        }
    }

	public float StartSoundSystem()
	{
		float sound = PlayerPrefs.GetFloat(PlayerPrefsKeys.NormalizedSoundVolume);
		SoundGroup.audioMixer.SetFloat(
			"SoundVolume",
			Mathf.Log10(sound) * 20
		);
		return sound;
	}

	public float StartMusicSystem()
	{
		float music = PlayerPrefs.GetFloat(PlayerPrefsKeys.NormalizedMusicVolume);
		SoundGroup.audioMixer.SetFloat(
			"MusicVolume",
			Mathf.Log10(music) * 20
		);
		return music;
	}

	public void SetSoundVolume(float value)
	{
		SoundGroup.audioMixer.SetFloat("SoundVolume", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat(PlayerPrefsKeys.NormalizedSoundVolume, value);
	}

	public void SetMusicVolume(float value)
	{
		SoundGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat(PlayerPrefsKeys.NormalizedMusicVolume, value);
	}

	public void PlayAudio(AudioSources source, bool loop)
    {
		switch (source)
		{
			case AudioSources.MAIN_LOOP:
				MusicSource.Stop();
				MusicSource.clip = MainLoopClip;
				CurrentMusic = AudioSources.MAIN_LOOP;
				MusicSource.loop = loop;
				MusicSource.Play();
				break;
			case AudioSources.CALM_LOOP:
				MusicSource.Stop();
				MusicSource.clip = CalmLoopClip;
				CurrentMusic = AudioSources.CALM_LOOP;
				MusicSource.loop = loop;
				MusicSource.Play();
				break;
			case AudioSources.MEMORY_LOOP:
				MusicSource.Stop();
				MusicSource.clip = MemoryLoopClip;
				CurrentMusic = AudioSources.MEMORY_LOOP;
				MusicSource.loop = loop;
				MusicSource.Play();
				break;
			case AudioSources.GAME_START:
				SoundSource.Stop();
				SoundSource.clip = StartBattleClip;
				CurrentSound = AudioSources.GAME_START;
				SoundSource.loop = loop;
				SoundSource.Play();
				break;
			case AudioSources.BTN_EFFECT:
				SoundSource.Stop();
				SoundSource.clip = BtnClickClip;
				CurrentSound = AudioSources.BTN_EFFECT;
				SoundSource.loop = loop;
				SoundSource.Play();
				break;
		}
	}

	public AudioSources GetCurrentMusic()
	{
		return CurrentMusic;
	}

	public AudioSources GetCurrentSound()
	{
		return CurrentSound;
	}

	public void StopAudio(AudioSources source)
    {
		switch (source)
		{
			case AudioSources.MAIN_LOOP:
			case AudioSources.CALM_LOOP:
			case AudioSources.MEMORY_LOOP:
				CurrentMusic = AudioSources.UNDEFINED;
				MusicSource.Stop();
				break;
			case AudioSources.GAME_START:
			case AudioSources.BTN_EFFECT:
				CurrentSound = AudioSources.UNDEFINED;
				SoundSource.Stop();
				break;
		}
	}

	public void StopAudio()
	{
		CurrentMusic = AudioSources.UNDEFINED;
		CurrentSound = AudioSources.UNDEFINED;
		MusicSource.Stop();
		SoundSource.Stop();
	}
}
