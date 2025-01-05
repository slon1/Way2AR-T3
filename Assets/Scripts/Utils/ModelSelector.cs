using UnityEngine;

public class ModelSelector : MonoBehaviour, ISelectable
{
    [SerializeField]
    private GameObject prefab;
	//private Animator anim;
    void Start()
    {
        prefab=Instantiate(prefab);
        prefab.SetActive(false);
		//anim=prefab.GetComponent<Animator>();
    }
	public void EnableVFX(ModelView targetModel) {

		prefab.transform.SetParent(targetModel.transform, true);
		prefab.transform.localPosition = Vector3.zero;		
		Vector3 offset = Vector3.up * targetModel.Height*1.33f;
		prefab.transform.localPosition = offset;		
		prefab.SetActive(true);
		//anim.enabled = true;
	}
	public void DisableVFX() {
		prefab.transform.SetParent(null);        
		prefab.SetActive(false);
		//anim.enabled = false;
	}
	private void OnDestroy() {
		//anim = null;
		Destroy(prefab);
	}
}
