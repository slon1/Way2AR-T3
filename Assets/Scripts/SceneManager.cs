using UnityEngine;
using VContainer;


public class SceneManager : MonoBehaviour
{
	[Inject] private IModelLoader modelLoader;		
	[Inject] private IGUIManager gui;
	[Inject] private ISelectable selector;


	private string activeModel;
	private const string ext = ".glb";
	

	private void OnDeleteActive() {
		modelLoader.Unload(activeModel);
		activeModel = null;
		selector.DisableVFX();
	}

	private async void OnSetColor(Color color) {
		var model = await modelLoader.Load(activeModel + ext);
		if (model) {
			model.ChangeModelColor(color);
		}
	}

	private async void OnPlayAnimation(string anim) {
		var model = await modelLoader.Load(activeModel + ext);
		if (model) {
			model.PlayAnimation(anim);
		}
	}

	private void OnModelTap(string modelName) {
		var model=modelLoader.GetModel(modelName);
		if (model) {			
			gui.ShowPanel(PanelId.Color);
			gui.Execute(PanelId.Color, PageActionId.LoadAnimations,model.GetAnimations());
			activeModel = modelName;
			selector.EnableVFX(model);
		}
	}

	private async void OnPlaceSelected(Pose pose) {
		var model = await modelLoader.Load(activeModel+ext);
		if (model) {
			model.SetPose(pose);
			selector.EnableVFX(model);
		}
		
	}

	private void OnModelSelected(string modelName) {		
		activeModel = modelName;		
		gui.ShowPanel(PanelId.View);
	}
	void Start() {
		EventBus.Instance.AddListener<string>(EventId.ModelSelected, OnModelSelected);
		EventBus.Instance.AddListener<Pose>(EventId.PlaceSelected, OnPlaceSelected);
		EventBus.Instance.AddListener<string>(EventId.ModelTap, OnModelTap);
		EventBus.Instance.AddListener<string>(EventId.PlayAnimation, OnPlayAnimation);
		EventBus.Instance.AddListener<Color>(EventId.SetColor, OnSetColor);
		EventBus.Instance.AddListener(EventId.Delete, OnDeleteActive);
	}
	private void OnDestroy() {
		EventBus.Instance.RemoveListener<string>(EventId.ModelSelected, OnModelSelected);
		EventBus.Instance.RemoveListener<Pose>(EventId.PlaceSelected, OnPlaceSelected);
		EventBus.Instance.RemoveListener<string>(EventId.ModelTap, OnModelTap);
		EventBus.Instance.RemoveListener<string>(EventId.PlayAnimation, OnPlayAnimation);
		EventBus.Instance.RemoveListener<Color>(EventId.SetColor, OnSetColor);
		EventBus.Instance.RemoveListener(EventId.Delete, OnDeleteActive);
		
	}
}
