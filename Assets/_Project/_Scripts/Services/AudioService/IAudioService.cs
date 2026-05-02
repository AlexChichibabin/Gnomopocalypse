public interface IAudioService
{
	void Init();
	void PlayMusic();
	void PlayMusic(MusicId id);
	void PlaySound(SoundId id);
	void StopMusic();
}