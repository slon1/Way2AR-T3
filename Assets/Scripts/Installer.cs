using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityGLTF;
using VContainer;
using VContainer.Unity;

public class Installer : LifetimeScope
{
	[SerializeField] private GameObject xrOrigin;
	protected override void Configure(IContainerBuilder builder) {

		var planeManager = xrOrigin.GetComponent<ARPlaneManager>();
		var raycastManager = xrOrigin.GetComponent<ARRaycastManager>();


		builder.Register<IModelLoader, ModelLoader>(Lifetime.Singleton)
			   .WithParameter(GetComponent<GLTFComponent>());

		builder.RegisterComponent(planeManager);
		builder.RegisterComponent(raycastManager);
		
		builder.RegisterComponent<IGUIManager>(GetComponent<GUIManager>());
		builder.RegisterComponent<ISelectable>(GetComponent<ModelSelector>());		

	}
}
