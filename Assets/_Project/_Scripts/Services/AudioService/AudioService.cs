using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioService : IAudioService
{
	private AudioConfig config;
	private AudioSource sfxSource;
	private AudioSource musicSource;
	private AudioMixer mixer;
	private AudioMixerGroup sfxGroup;
	private AudioMixerGroup MusicGroup;
	private Dictionary<SoundId, SoundEntry> sounds;
	private Dictionary<MusicId, MusicEntry> musics;

	private IConfigProvider configProvider;

	public AudioService(
		IConfigProvider configProvider,
		AudioMixer mixer)
	{
		this.configProvider = configProvider;
		this.mixer = mixer;
	}
	public void Init()
	{
		config = configProvider.GetAudio();

		var root = new GameObject("[Audio]");
		Object.DontDestroyOnLoad(root);

		sfxSource = root.AddComponent<AudioSource>();
		sfxSource.outputAudioMixerGroup = config.SfxMixerGroup;

		musicSource = root.AddComponent<AudioSource>();
		musicSource.outputAudioMixerGroup = config.MusicMixerGroup;
		musicSource.loop = true;

		if (config.Sounds != null)
			sounds = config.Sounds.ToDictionary(x => x.Id, x => x);
		if (config.Musics != null)
			musics = config.Musics.ToDictionary(x => x.Id, x => x);
	}

	public void PlaySound(SoundId id)
	{
		if (!sounds.TryGetValue(id, out var sound) || sound.Clip == null)
		{
			Debug.Log($"Sound not found: {id}");
			return;
		}

		sfxSource.PlayOneShot(sound.Clip, sound.Volume);
	}

	public void PlayMusic(MusicId id)
	{
		if (config.Musics == null) return;
		if (musics[id].Clip == null) return;
		if (musicSource.clip == musics[id].Clip && musicSource.isPlaying)
			return;

		musicSource.clip = musics[id].Clip;
		musicSource.Play();
	}
	public void PlayMusic()
	{
		if (config.Musics == null) return;
		if (musicSource.clip != null && musicSource.isPlaying)
			return;
		if (musicSource.clip == null) return;

		musicSource.Play();
	}
	public void StopMusic()
	{
		musicSource.Stop();
	}
}
