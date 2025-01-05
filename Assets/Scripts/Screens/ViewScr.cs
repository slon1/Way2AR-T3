using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using VContainer;


public class ViewScr : ScrAbs 
{
	[Inject] ARPlaneManager planeManager;
	[Inject] ARRaycastManager raycastManager;
	[SerializeField]
	private Text message;
	[SerializeField] 
	private EventSystem eventSystem;
	[SerializeField] 
	private GraphicRaycaster graphicRaycaster;


	private const string planeOk = "Tap to place";
	private const string planeError = "Plane not detected";
	private bool init=false;	


	private void Touch_onFingerDown(UnityEngine.InputSystem.EnhancedTouch.Finger obj) {
		if (IsPointerOverUI(obj.currentTouch.screenPosition)) {			
			return;
		}
		List<ARRaycastHit> hits = new List<ARRaycastHit>();
	
		Ray ray = Camera.main.ScreenPointToRay(obj.currentTouch.screenPosition);
		if (raycastManager.Raycast(obj.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)) {
			if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.GetComponent<ModelView>()) { 
				print(hit.transform.name);
				return;
			}
			Pose hitPose = hits[0].pose;			
		
			EventBus.Instance.Invoke(EventId.PlaceSelected, hitPose);
		}
	}
	
	private bool IsPointerOverUI(Vector2 screenPosition) {
		PointerEventData pointerData = new PointerEventData(eventSystem) {
			position = screenPosition
		};

		List<RaycastResult> results = new List<RaycastResult>();
		graphicRaycaster.Raycast(pointerData, results);
		return results.Count > 0;
	}

	public override void Show() {
		base.Show();
		planeManager.trackablesChanged.AddListener(OnTrackablesChanged);			
		init = true;

	}
	
	public override void Hide() {
		base.Hide();
		planeManager.trackablesChanged.RemoveListener(OnTrackablesChanged);		
		init = false;
	}

	private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> plane) {
		message.text=(planeManager.trackables.count > 0) ? planeOk: planeError;
	}

	public override void Start() {
		base.Start();
		message.text = planeError;
		UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Enable();
		UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
		UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += Touch_onFingerDown;

	}

	private void OnDestroy() {
		UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= Touch_onFingerDown;
		UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Disable();
		UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Disable();

	}
}
