using UnityEngine;
using Zenject;

public class TestMusic : MonoBehaviour
{
    private IAudioService audioService;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        this.audioService = audioService;
    }

	private void Start()
	{
        audioService.PlayMusic(MusicId.Menu);
	}
}
