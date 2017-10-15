using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SymbolKeyboard : MonoBehaviour
    {

        [SerializeField] private List<GameObject> symbols;
        //[SerializeField] private int countSymbols;
        [SerializeField] private GameObject slotPrefab;
        
        private static int mCountSymbols;
        private static Transform mTransform;
        private static GameObject mSlotPrefab;
        private static List<GameObject> mSymbols = new List<GameObject>();
        
        public static List<GameObject> keyboardSlots = new List<GameObject>();

        private void Start()
        {

            mSlotPrefab = slotPrefab;
            mTransform = transform;
            mSymbols = symbols;

            FillTextField();

        }
        
        public static void FillTextField()
        {
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
                
                keyboardSlots.RemoveAll(item => item);
                Debug.Log("Keyboard slots count: " + keyboardSlots.Count);
                Init();
            }
        }

        private static void Init() {
            for (int i = 0; i < mSymbols.Count; i++)
            {
                Instantiate(mSlotPrefab, mTransform);
            }

			int J_copy = 0;
			for (int j = mTransform.childCount - mSymbols.Count; j < mTransform.childCount ; j++)  {

				if (mTransform.GetChild(j).transform.childCount == 0) {
					GameObject objectScene = (GameObject)Instantiate (mSymbols [J_copy], mTransform.GetChild (j).transform);

					if (keyboardSlots.Count > j && !keyboardSlots [j])
						keyboardSlots [j] = objectScene.transform.parent.gameObject;
					else keyboardSlots.Add(objectScene.transform.parent.gameObject);
                }
				J_copy++;
            }
        }
    }
}