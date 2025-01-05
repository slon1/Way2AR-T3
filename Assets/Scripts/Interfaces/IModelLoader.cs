using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IModelLoader 
{
	ModelView GetModel(string modelName);
	UniTask<ModelView> Load(string fileName);
	void Unload(string modelName);

	UniTask<string[]> GetModelNames();

}
