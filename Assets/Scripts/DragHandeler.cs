using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject itemBeingDragged;
        private Vector3 startPosition;
        private Transform startParent;

		public Transform canvas;
		public Transform parent;
		public AudioClip key;

		void Start() {
			canvas = GameObject.Find ("Canvas").transform;
		}

        
        #region IBeginDragHandler implementation

        public void OnBeginDrag (PointerEventData eventData)
        {
            itemBeingDragged = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;

            GetComponent<CanvasGroup>().blocksRaycasts = false;

			if (!parent)
				parent = transform.parent;
			transform.parent = canvas;
        }

        #endregion

        #region IDragHandler implementation

        public void OnDrag (PointerEventData eventData)
        {
            transform.position = eventData.position;
            //transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }

        #endregion

        #region IEndDragHandler implementation

        public void OnEndDrag (PointerEventData eventData) {
			if (parent) {
				transform.parent = parent;
				transform.localPosition = new Vector3(0,0,0);
			}

            itemBeingDragged = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;


			if(transform.parent == startParent) {
                transform.position = startPosition;
                Debug.Log("Возращаем");
            }
			AudioSource.PlayClipAtPoint (key, Camera.main.transform.position);
        }

        #endregion
        
        
        
    }
}