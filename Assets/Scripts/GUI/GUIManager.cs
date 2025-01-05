using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using VContainer;

public class GUIManager : MonoBehaviour, IGUIManager {

	[Inject] private IModelLoader loader;
	[Inject] private ARPlaneManager planeManager ;

	private Dictionary<PanelId, IPage> panels;
	[SerializeField]
	private List<ScrAbs> screens;
	private List<ARPlaneMeshVisualizer> meshVisualizers;
	public void Initialize() {
		panels = screens.ToDictionary(panel => panel.PanelID, panel => (IPage)panel);
		EventBus.Instance.AddListener<ButtonId>(EventId.MenuEvent, OnMenuEvent);
	}

	private void Start() {
		Initialize();
		ShowPanel( PanelId.Menu);

	}

	private async void OnMenuEvent(ButtonId id) {
		print(id);	
		switch (id) {
			case ButtonId.Menu:
				ShowPanel(PanelId.Menu);
				break;
			case ButtonId.Plane:
				TogglePlane();
				break;
			case ButtonId.Model:				
				Execute (PanelId.Model, PageActionId.LoadModel, await loader.GetModelNames());
				ShowPanel(PanelId.Model);				
				break;
			case ButtonId.Color:
				ShowPanel(PanelId.Color);
				break;
			case ButtonId.View:
				ShowPanel(PanelId.View);
				break;
			case ButtonId.Delete:
				EventBus.Instance.Invoke(EventId.Delete);
				ShowPanel( PanelId.Model);
				break;			
			default:
				break;
		}
	}	
		
	public void ShowPanelModal(PanelId panelId, bool show) {
		if (show) {
			panels[panelId].Show();
		}
		else {
			panels[panelId].Hide();
		}
	}

	public void ShowPanel(PanelId panelId) {
		foreach (var panel in panels.Values) {
			if (panel.IsStatic()) {
				continue;
			}
			if (panel.PanelID == panelId) {
				panel.Show();
			}
			else {
				panel.Hide();
			}
		}
	}

	private void OnDestroy() {		
		panels = null;
		EventBus.Instance.RemoveListener<ButtonId>(EventId.MenuEvent, OnMenuEvent);
	}

	public void Execute<T>(PanelId panelId, PageActionId action, T param) {
		panels[panelId].Execute(action, param);
	}

	public void Execute(PanelId panelId, PageActionId action) {
		panels[panelId].Execute(action);
	}	

	private void TogglePlane() {
		foreach (ARPlane plane in planeManager.trackables) {
			var mv = plane.GetComponent<ARPlaneMeshVisualizer>();
			mv.enabled = !mv.enabled;
		}
	}
}
