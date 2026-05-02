using System;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundId
{
	Click,
	ButtonSelect,
	Shooting,
	HitEnemy,
	HitPlayer,

	Win,
	Lose
}
public enum MusicId
{
	Menu,
	Gameplay,
	Win,
	Lose
}

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Configs/AudioConfig")]
public class AudioConfig : ScriptableObject
{
	public SoundEntry[] Sounds;
	public MusicEntry[] Musics;

	public AudioMixerGroup SfxMixerGroup;
	public AudioMixerGroup MusicMixerGroup;
}


[Serializable]
public class SoundEntry
{
	public SoundId Id;
	public AudioClip Clip;
	[Range(0f, 1f)] public float Volume = 1f;
}
[Serializable]
public class MusicEntry
{
	public MusicId Id;
	public AudioClip Clip;
	[Range(0f, 1f)] public float Volume = 1f;
}