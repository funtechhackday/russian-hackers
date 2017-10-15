using DefaultNamespace.UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class Slot : MonoBehaviour, IDropHandler 
    {
        public GameObject item {
            get {
                if(transform.childCount > 0){
                    return transform.GetChild(0).gameObject;
                }
                return null;
            }
        }

        #region IDropHandler implementation
        public void OnDrop (PointerEventData eventData)
        {
			Debug.Log ("OnDrop");
            if(!item){
                //DragHandeler.itemBeingDragged.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
                
                DragHandeler.itemBeingDragged.transform.SetParent(transform);
				DragHandeler.itemBeingDragged.transform.GetComponent<DragHandeler> ().parent = transform;
                
                item.transform.localPosition = new Vector3(0, 0, 0);

                ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x,y) => x.HasChanged());
            }
        }
        #endregion

    }
}