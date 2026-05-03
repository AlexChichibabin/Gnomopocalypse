using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
	[Inject] private IAudioService audioService;

	public void OnPointerClick(PointerEventData eventData)
	{
		audioService.PlaySound(SoundId.Click);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		audioService.PlaySound(SoundId.ButtonSelect);
	}
}
