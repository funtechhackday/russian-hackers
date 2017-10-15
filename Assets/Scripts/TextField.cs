using System.Collections.Generic;
using DefaultNamespace.UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TextField : MonoBehaviour, IHasChanged
    {
        [SerializeField] private Transform slots;
        [SerializeField] private Text fieldText;
        [SerializeField] private int countSlots;
        [SerializeField] private GameObject slotPrefab;
        
        private static GameObject mSlotPrefab;
        private static Transform mTransform;
        private static int mCountSlots;
        
        public static List<GameObject> textFieldSlots = new List<GameObject>();

		public void Start ()
		{
			if(GameManager.lengthCodeGameSetting != null && GameManager.lengthCodeGameSetting > 3)
				countSlots = GameManager.lengthCodeGameSetting;

		    mSlotPrefab = slotPrefab;
		    mTransform = transform;
		    mCountSlots = countSlots;

			FillTextField(mCountSlots);

		}

        // Заполнено ли поле
        // True полностью заполнено
        // False есть хоть одно не заполненное поле
        public static bool IsFilledField()
        {

            for (int i = 0; i < textFieldSlots.Count; i++)
            {
                if (textFieldSlots[i].transform.childCount == 0)
                {
                    return false;
                }
            }
            
            return true;
        }

		public static void FillTextField(int count)
        {
			mCountSlots = count;

            if (mTransform.childCount == 0)
            {
                Init();
            }
            else
            {
                foreach (Transform slot in mTransform)
                {
                    Destroy(slot.gameObject);
                }

                textFieldSlots.RemoveAll(item => item);

                Init();
            }
        }

        private static void Init()
        {
            for (int i = 0; i < mCountSlots; i++) {
				if (textFieldSlots.Count > i && !textFieldSlots [i])
					textFieldSlots [i] = (GameObject)Instantiate (mSlotPrefab, mTransform);
				else 
					textFieldSlots.Add((GameObject)Instantiate(mSlotPrefab, mTransform));
            }
			/*Debug.Log (textFieldSlots.Count);
            foreach (Transform slot in mTransform)
            {
				
                textFieldSlots.Add(slot.gameObject);

            }
			Debug.Log (textFieldSlots.Count);*/
        }
        
        public static int GetFreeSlot()
        {
            //Debug.Log("Count " + textFieldSlots.Count);
            
            for (int i = 0; i < textFieldSlots.Count; i++)
            {
                if (textFieldSlots[i].transform.childCount == 0)
                {
                    return i;
                }
            }

            return -1;
        }
	
        #region IHasChanged implementation
        public void HasChanged()
        {
            //Debug.Log("OnChange");
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //builder.Append (" - ");
            foreach (Transform slotTransform in slots){
                GameObject item = slotTransform.GetComponent<Slot>().item;
                if (item){
                    builder.Append (item.GetComponent<Symbol>().id);
                    //builder.Append (" - ");
                }
            }
            GameManager.currentCode = builder.ToString();
            //fieldText.text = builder.ToString();
        }
        #endregion
    }
    
    namespace UnityEngine.EventSystems {
        public interface IHasChanged : IEventSystemHandler {
            void HasChanged();
        }
    }
    
}