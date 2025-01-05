using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityGLTF;

[System.Serializable]
public class FileListWrapper {
	public string[] files;
}

public class ModelLoader : IModelLoader, IDisposable {
	private GLTFComponent component;
	private GameObject root;
	private Dictionary<string, ModelView> componentsDict = new Dictionary<string, ModelView>();
	public float MaxModelScale = 1;
	public List<string> Names => componentsDict.Keys.ToList();
	public ModelLoader(GLTFComponent component) {
		this.component = component;

	}

	public ModelView GetModel(string modelName) {
		return componentsDict[modelName];
	}

	public async UniTask<ModelView> Load(string fileName) {
		string modelName = Path.GetFileNameWithoutExtension(fileName);
		if (componentsDict.ContainsKey(modelName)) {
			return componentsDict[modelName];
		}

		component.GLTFUri = fileName;

		try {
			await component.Load();

			if (component.LastLoadedScene != null) {
				var modelView = component.LastLoadedScene.AddComponent<ModelView>();
				modelView.modelName = modelName;
				componentsDict[modelName] = modelView;
				SetBoxCollider(modelView.gameObject);
				return modelView;
			}
			else {
				return null;
			}
		}
		catch (Exception ex) {
			Debug.LogError($"Error loading model: {fileName}\n{ex}");
			return null;
		}
	}
	private BoxCollider SetBoxCollider(GameObject root) {

		var renderers = root.GetComponentsInChildren<Renderer>();

		if (renderers.Length == 0) {
			return null;
		}
		Bounds bounds = renderers[0].bounds;

		foreach (var renderer in renderers) {
			bounds.Encapsulate(renderer.bounds);
		}

		Vector3 localCenter = root.transform.InverseTransformPoint(bounds.center);
		Vector3 localSize = bounds.size;

		var boxCollider = root.GetComponent<BoxCollider>();
		if (boxCollider == null) {
			boxCollider = root.AddComponent<BoxCollider>();
		}

		boxCollider.center = localCenter;
		boxCollider.size = localSize * 0.75f;
		if (localSize.sqrMagnitude > MaxModelScale) {
			root.transform.localScale /= localSize.magnitude;
		}
		return boxCollider;
	}

	public string[] GetModelPaths() {
		return Directory.GetFiles(Application.streamingAssetsPath, "*.glb", SearchOption.TopDirectoryOnly);
	}

	private async UniTask<string> LoadTextFileFromStreamingAssets(string relativePath) {

		string fullPath = GetStreamingAssetPath(relativePath);

		using (var request = UnityWebRequest.Get(fullPath)) {
			await request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success) {
				return request.downloadHandler.text;
			}
			else {
				Debug.LogError($"Failed to load text file from StreamingAssets: {fullPath}. Error: {request.error}");
				return null;
			}
		}
	}

	private static string GetStreamingAssetPath(string relativePath) {

		string basePath = Application.streamingAssetsPath;


		if (Application.platform == RuntimePlatform.Android) {
			return $"{basePath}/{relativePath}";
		}
		else {
			return Path.Combine(basePath, relativePath);
		}
	}

	public async UniTask<string[]> GetModelNames() {

		string fileListJson = await LoadTextFileFromStreamingAssets("filelist.json");

		if (string.IsNullOrEmpty(fileListJson)) {
			Debug.LogError("Failed to load file list.");
			return null;
		}		

		FileListWrapper fileListWrapper = JsonUtility.FromJson<FileListWrapper>(fileListJson);
		if (fileListWrapper?.files == null || fileListWrapper.files.Length == 0) {
			Debug.LogError("File list is empty or invalid.");
			return null;
		}
		
		return fileListWrapper.files;
	}	

	public void Unload(string modelName) {
		if (componentsDict.TryGetValue(modelName, out var modelView)) {
			GameObject.Destroy(modelView.gameObject);
			componentsDict.Remove(modelName);			
		}		
	}

	public void Dispose() {
		GameObject.Destroy(component);
		GameObject.Destroy(root);
		component = null;
		root = null;
	}
}
