using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private RawImage rawImage; 
	private Texture2D texture;


	void Start() {		
		texture = rawImage.texture as Texture2D;
	}

	public void OnPointerClick(PointerEventData eventData) {		
		Vector2 localPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, eventData.position, null, out localPos);
				
		Vector2 textureCoord = new Vector2(
			(localPos.x + rawImage.rectTransform.rect.width / 2) / rawImage.rectTransform.rect.width,
			(localPos.y + rawImage.rectTransform.rect.height / 2) / rawImage.rectTransform.rect.height
		);
				
		Color color = texture.GetPixelBilinear(textureCoord.x, textureCoord.y);
		EventBus.Instance.Invoke( EventId.SetColor, color);
				
	}
}
