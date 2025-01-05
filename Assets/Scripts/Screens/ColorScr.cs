using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorScr : ScrAbs
{
    [SerializeField]
    private RawImage color;
	[SerializeField]
	private Transform contentPanel;
	private GameObject buttonTemplate;
	private List<string> anims;
	

	public override void Start() {
		base.Start();
		if (contentPanel.childCount > 0) {
			buttonTemplate = contentPanel.GetChild(0).gameObject;
			buttonTemplate.SetActive(false); 
		}
		else {			
			return;
		}
		ClearMenu();
		
	}

	void CreateButton(string anim) {
		
		GameObject newButton = Instantiate(buttonTemplate, contentPanel);
		newButton.SetActive(true); 		
		
		newButton.GetComponentInChildren<Text>().text = anim;

		var button = newButton.GetComponent<Button>();
		if (button == null) {
			button = newButton.AddComponent<Button>();
		}
		else {
			button.onClick.RemoveAllListeners();
		}
		newButton.GetComponent<Button>().onClick.AddListener(() => {
			EventBus.Instance.Invoke( EventId.PlayAnimation, anim);

		});
	}


	public void RefreshMenu(List<string> modelPaths) {
		ClearMenu();
		foreach (string modelPath in modelPaths) {
			CreateButton(modelPath);
		}
	}

	private void ClearMenu() {
		for (int i = contentPanel.childCount - 1; i >= 0; i--) {
			var child = contentPanel.GetChild(i).gameObject;
			if (child != buttonTemplate) {
				Destroy(child);
			}
		}
	}

	public override void Execute<T>(PageActionId action, T param) {
		base.Execute(action, param);
		if (action== PageActionId.LoadAnimations && param is List<string> anims) {
		this.anims = anims;
			RefreshMenu(anims);
		}
	}
}
