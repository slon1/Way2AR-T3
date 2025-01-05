using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
public class SpawnPoint {
	public Pose Pose;
	public string ModelName;
}
public class ModelScr : ScrAbs{
	
	[SerializeField]
	private Transform contentPanel;
	private GameObject buttonTemplate;
	private List<string> modelPaths;

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

	void CreateButton(string modelPath) {		
		GameObject newButton = Instantiate(buttonTemplate, contentPanel);
		newButton.SetActive(true); 
		string name = Path.GetFileNameWithoutExtension(modelPath);
		print(name);
		newButton.GetComponentInChildren<Text>().text = name;

		var button = newButton.GetComponent<Button>();
		if (button == null) {
			button= newButton.AddComponent<Button>();
		}
		else {
			button.onClick.RemoveAllListeners();
		}
		newButton.GetComponent<Button>().onClick.AddListener(() => {
			EventBus.Instance.Invoke(EventId.ModelSelected, modelPath);		
			
		});
	}
	
	public override void Show() {
		base.Show();

	}

	private void ClearMenu() {		
		for (int i = contentPanel.childCount - 1; i >= 0; i--) {
			var child = contentPanel.GetChild(i).gameObject;
			if (child != buttonTemplate) {
				Destroy(child);
			}
		}
	}

	public void RefreshMenu(List<string> modelPaths ) {
		ClearMenu(); 		
		foreach (string modelPath in modelPaths) {
			CreateButton(modelPath);
		}
	}

	public override void Execute<T>(PageActionId action, T param) {
		base.Execute(action, param);
		if (action == PageActionId.LoadModel && param is string[] Paths) {
			modelPaths = Paths.ToList() ;
			RefreshMenu(modelPaths);
		}
	}
}
