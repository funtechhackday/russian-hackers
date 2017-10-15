using DefaultNamespace;
using DefaultNamespace.UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

public class Symbol : MonoBehaviour
{

	public AudioClip clip;
	public int id = 0;

	public void OnPressed()
	{
		
		if (transform.parent.transform.parent.name.Equals("TextField"))
		{
			transform.SetParent(SymbolKeyboard.keyboardSlots[id].transform);
			transform.localPosition = new Vector3(0, 0, 0);
			
			ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x,y) => x.HasChanged());
		}
		else
		{
			var freeSlot = TextField.GetFreeSlot();
			Debug.Log("Free slot " + freeSlot);
			if (freeSlot != -1)
			{
				transform.SetParent(TextField.textFieldSlots[freeSlot].transform);
				transform.localPosition = new Vector3(0, 0, 0);
				
				ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x,y) => x.HasChanged());
			}
		}
		AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position);
	}

}
