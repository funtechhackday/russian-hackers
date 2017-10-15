using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
	public class ChoiceTypeMenu : MonoBehaviour
    {
        public class Param
        {
            public int type;
            public int complexity;
        }
			
        [SerializeField] private ComplexitySlider slider;

		[SerializeField] private Animation mainMenu;
		[SerializeField] private Animation typeGame;
		[SerializeField] private AudioClip click;
		[SerializeField] private AudioClip sliderTouch;

        //Game types
        private const int INFINITY_TYPE = 0;
        private const int TIME_TYPE = 2;
        private const int ATTEMPTS_TYPE = 1;
        
        //Complexity types
        private const int THREE_TYPE = 3;

		private Param mParam;
        private bool isTime = false;
        private bool isAttempts = false;
		private bool a_Infinity;

		void LoadTutorial() {
			Application.LoadLevel (2);
		}

		public void OnMenu(bool on) { // меню
			if (PlayerPrefs.GetInt ("first") == 0) {
				PlayerPrefs.SetInt ("first", 1);
				mainMenu.Play ("menu(diactive)");
				Invoke ("LoadTutorial", 0.5f);
			} else {
				if (on) {
					mainMenu.Play ("menu(diactive)");
					typeGame.Play ("mode(active)");
				} else {
					mainMenu.Play ("menu(active2)");
					typeGame.Play ("mode(diactive)");
				}
			}


		}

		public ChoiceTypeMenu() {
            mParam = new Param();
        }

        public void OnInfinity() { // бесконечный уровень
			mParam.type = INFINITY_TYPE;
			mParam.complexity = slider.value;

			if (!a_Infinity) {
				typeGame.Play ("mode_infinity(active)");
				a_Infinity = true;
			} else {
				LoadLevel ();
			}
        }
        
        public void OnTime() {
			mParam.type = TIME_TYPE;
			mParam.complexity = 3;

			if (a_Infinity) {
				typeGame.Play ("mode_infinity(diactive)");
				a_Infinity = false;

				Invoke ("LoadLevel", 0.5f);
			} else {
				LoadLevel ();
			}
        }
        
        public void OnAttempts() {
			mParam.type = ATTEMPTS_TYPE;
			mParam.complexity = 3;

			if (a_Infinity) {
				typeGame.Play ("mode_infinity(diactive)");
				a_Infinity = false;

				Invoke ("LoadLevel", 0.5f);
			} else {
				LoadLevel ();
			}
        }
		public void LoadLevel() {
			typeGame.Play ("mode(diactive)");
			GameManager.typesGameSetting = mParam.type;
			GameManager.lengthCodeGameSetting = mParam.complexity;
			Invoke ("LoadLevelTime", 0.5f);
		}
		public void LoadLevelTime() {
			Application.LoadLevel (1);
		}

		public void Click() {
			AudioSource.PlayClipAtPoint (click, Camera.main.transform.position);
		}
		public void ClickSlider() {
			AudioSource.PlayClipAtPoint (sliderTouch, Camera.main.transform.position, 0.255f);
		}
    }
}