using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGLTF;

public class ModelView : MonoBehaviour, IPointerDownHandler {
	public string modelName;
	private Animation anim;
	private BoxCollider box;
	public float Height => CalculateHeight();

	public ModelView Clone() {
		var instantiatedObject = GetComponent<InstantiatedGLTFObject>();
		if (instantiatedObject == null) {			
			return null;
		}				
		var duplicatedObject = instantiatedObject.Duplicate();
		if (duplicatedObject == null) {		
			return null;
		}
		var modelView = duplicatedObject.GetComponent<ModelView>();
		if (modelView == null) {
			return null;
		}
		return modelView;
	}

	public List<string> GetAnimations() {
		List<string> ret = new();
		if (anim != null) {
			foreach (AnimationState state in anim) {
				ret.Add(state.name);
			}
		}
		return ret;
	}

	public void ChangeModelColor(Color newColor) {
		
		var renderers = transform.GetComponentsInChildren<Renderer>();
		if (renderers.Length == 0) {			
			return;
		}

		foreach (var renderer in renderers) {
			foreach (var material in renderer.materials) {
				if (material.HasProperty("baseColorFactor")) 
				{
					material.SetColor("baseColorFactor", newColor);				
				}				
			}
		}
		
	}

	public void Show(bool v) {
		gameObject.SetActive(v);
	}

	public void SetPose(Pose pose) {
		transform.position = pose.position;		
		Vector3 directionToCamera = Camera.main.transform.position - pose.position;
		directionToCamera.y = 0;

		if (directionToCamera.sqrMagnitude > 0.01f) {
			transform.rotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
		}
	}
		
	void Start() {
		anim = GetComponent<Animation>();
	}

	public void OnPointerDown(PointerEventData eventData) {
		EventBus.Instance.Invoke(EventId.ModelTap, modelName);
	}


	private void OnDestroy() {
		anim = null;
	}

	public float CalculateHeight() {
		if (box == null) {
			box = GetComponent<BoxCollider>();
		}
		return box.center.y + box.size.y*0.5f;
	}

	public void PlayAnimation(string clipName) {
		anim.Stop();
		anim[clipName].wrapMode = WrapMode.Once;
		anim.Play(clipName);
	}

}
