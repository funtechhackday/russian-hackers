using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public enum Type {
		Mission, Endless, Attempts,  Time
	}
	[Header("Режим игры")]
	public Type type;


	[System.Serializable]
	public class Mission
	{
		public int stopProcent;
		public bool complite = false;
	}  
	[Header("Миссии")]
	public Mission[] mission;
	
	[Header("Объекты")]
	public GameObject entry;
	public GameObject breaking;
	public RectTransform centralize;
	public RectTransform scroll;
	public Text compiling;
	public GameObject missionComplite;
	public Text textMissionComplite;

	[Header("Газета")]
	public GameObject newspapir;
	public Transform newspaperStars;
	public Text newspaperTime;
	public Text newspaperAttempts;
	public Text newspaperCode;
	public Text newspaperRestart;
	public Text newspaperValue;
	public Slider slider;

	[Header("Эффект загрузки")]
	public GameObject TextFildTrue;
	public GameObject TextFildFalse;
	public GameObject TextFild;
	public GameObject TextFildLoading;
	public Animation DebugResult;
	public Text TextDebugResult;
	public Image imageDebugResult;
	public Image imageDebugResultLoading;
	public Sprite buttonON;
	public Sprite buttonOFF;
	public GameObject mask;
	public Text TextField;
	public string[] textFieldCode;

	[Header("Attempts")]
	public int maxCheck;
	private int countBreaking;
	private bool _breaking;

	[Header("Time")]
	public float maxTime;

	[Header("Звуки")]
	public AudioClip touch;
	public AudioClip key;
	public AudioClip sliderAudio;

	public static string currentCode;
	public static int typesGameSetting;
	public static int lengthCodeGameSetting;
	private int lengthCode = 3;
	private string correctCode; // код который угадываем
	private string abc = "0123456789";
	private int countCheck;
	private int checkMission = 0;
	private int bull = 0;
	private int cow = 0;
	private float timmer;
	private int countEntery;
	private Coroutine textCompiling;
	[HideInInspector]public int AllStars;

	public void Start() {
		/* Настройки */
		if (typesGameSetting == 0) {
			type = Type.Endless;

			if(lengthCodeGameSetting != null && lengthCodeGameSetting > 3)
				lengthCode = lengthCodeGameSetting;
		} else if (typesGameSetting == 1)
			type = Type.Attempts;
		else if (typesGameSetting == 2)
			type = Type.Time;
		
		/* конец настроек */

		if(type == Type.Attempts) compiling.text = "Attempts remain: " + (maxCheck - countCheck);

		CreateCode ();
		if (type == Type.Mission)
			Check ();
	}

	void Update() {
		if(!newspapir.active) timmer += Time.deltaTime;

		if (type == Type.Time) {
			if (maxTime < 0) {
				if (!missionComplite.active) {
					int min = (int)timmer / 60;
					int sec = (int)timmer - min * 60;

					newspaperTime.text = min + " m " + sec + " s";
					newspaperAttempts.text = countCheck + " raids";
					newspaperCode.text = countBreaking + " hack-in";
					newspaperRestart.text = "restart";

					OpenNewsPapir ();
				}
			} else {
				compiling.text = "IP will be changed: " + (int)maxTime + " sec.";
				maxTime -= Time.deltaTime;
			}
		}
	}
	
	void CreateCode() {
		abc = "0123456789";
		correctCode = "";
		currentCode = "";

		for (int i = 0; i < lengthCode; i++) {
			int randomSymbol = Random.Range (0, abc.Length);

			correctCode += abc [randomSymbol];

			string copyABC = abc;
			abc = "";

			for (int j = 0; j < copyABC.Length; j++) {
				if (randomSymbol != j) {
					abc += copyABC [j];
				}
			}
			currentCode += "*";
		}
	}

	public void Check() {
		if (DefaultNamespace.TextField.IsFilledField()) {
			/* РАБОТА С АНИМАЦИЕЙ */
			mask.SetActive (true);
			DebugResult.Play ("DebugResult(loading)");
			TextDebugResult.text = "hacking";
			TextFild.SetActive (false);
			TextFildLoading.SetActive (true);
			imageDebugResult.sprite = buttonOFF;
			textCompiling = StartCoroutine (TextCompiling ());

			bull = 0;
			cow = 0;


			/* ПРОВЕРКА НА БЫКА */
			for (int i = 0; i < currentCode.Length; i++) {
				if (currentCode [i] == correctCode [i]) {
					bull++;
				}
			}

			/* ПРОВЕРКА НА КОРОВУ */
			for (int i = 0; i < currentCode.Length; i++) { // перебираем каждый символ
				if (currentCode [i] != correctCode [i]) { // если не бык
					for (int j = 0; j < correctCode.Length; j++) { // перебираем каждый символ правильного кода
						if (currentCode [i] == correctCode [j]) {
							cow++;
						}
					}
				}
			}

			if (bull == currentCode.Length) StartCoroutine (StopCheck (false));
			else StartCoroutine (StopCheck (true));
		}
	}

	IEnumerator StopCheck(bool stop) {
		if (stop) {
			yield return new WaitForSeconds (0.75f);
			DebugResult.Stop ();
			TextFildLoading.SetActive (false);
			TextFildFalse.SetActive (true);
		} else {
			yield return new WaitForSeconds (1.5f);
			TextFildLoading.SetActive (false);
			TextFildTrue.SetActive (true);
		}

		imageDebugResult.sprite = buttonON;
		TextDebugResult.text = "hack";
		imageDebugResultLoading.fillAmount = 0;
		StopCoroutine (textCompiling);

		yield return new WaitForSeconds (1f);

		mask.SetActive (false);
		TextFild.SetActive (true);
		TextFildFalse.SetActive (false);
		TextFildTrue.SetActive (false);

		countCheck++;
		if (type == Type.Mission)
			StartCoroutine (CheckCoroutine ());
		else {
			OutputResult ();
			if (type == Type.Attempts) { // если игра на колличество попыток
				if (countCheck == maxCheck) {
					int min = (int)timmer / 60;
					int sec = (int)timmer - min * 60;

					newspaperTime.text = min + " m " + sec + " s";
					newspaperAttempts.text = countCheck + " raids";
					newspaperCode.text = countBreaking + " hack-in";
					newspaperRestart.text = "restart";

					OpenNewsPapir ();
				}
			}
		} 
	}

	IEnumerator TextCompiling() {
		for (int i = 0; i < textFieldCode.Length - 5; i++) {
			string text = textFieldCode[i] + "\n" + textFieldCode[i+1] + "\n" + textFieldCode[i+2] + "\n" + textFieldCode[i+3] + "\n" + textFieldCode[i+4];
			TextField.text = text;
			Debug.Log (i);
			yield return new WaitForSeconds (0.01f);
		}

	}

	IEnumerator CheckCoroutine() {
		checkMission = 0;

		for (int i = 0; i <= 100; i++) {
			if (i != mission[checkMission].stopProcent) {
				yield return new WaitForSeconds (0.1f);
				compiling.text = "Compiling: " + i + "%";
			} else {
				if (mission [checkMission].complite)
					checkMission++;
				else {
					yield return new WaitForSeconds (1f);

					if (bull == currentCode.Length) {
						mission [checkMission].complite = true;
						checkMission++;
						CreateCode ();
						bull = 0;
						cow = 0;
					} else {
						compiling.text += " Error! Not the correct code!";
						OutputResult ();
						break;
					}
				}
			}
		}
	}

	void OutputResult() { // вывод результата
		if(_breaking) {
			for (int i = 1; i < centralize.childCount; i++) {
				Destroy(centralize.GetChild (i).gameObject);
			} 
			_breaking = false;
			countEntery = 0;

			scroll.sizeDelta = new Vector2 (scroll.sizeDelta.x, 836f);
			centralize.anchoredPosition = new Vector2 (0, 0);
		}

		GameObject entryScene = (GameObject)Instantiate(entry, centralize);

		entryScene.transform.GetChild (0).GetComponent<Text> ().text = currentCode;
		entryScene.transform.GetChild (1).GetComponent<Text> ().text = bull.ToString();
		entryScene.transform.GetChild (2).GetComponent<Text> ().text = cow.ToString();

		if (countEntery > 14) {
			scroll.sizeDelta = new Vector2 (scroll.sizeDelta.x, scroll.sizeDelta.y + 50);
			centralize.anchoredPosition = new Vector2 (0, centralize.anchoredPosition.y + 25f);
		}
		countEntery++;

		if (type != Type.Mission && type != Type.Attempts && type != Type.Time) {
			if (bull == currentCode.Length) {
				int min = (int)timmer / 60;
				int sec = (int)timmer - min * 60;
				countBreaking++;

				newspaperTime.text = min + " m " + sec + " s";
				newspaperAttempts.text = countCheck + " raids";
				newspaperCode.text = countBreaking + " hack-in";
				newspaperRestart.text = "continue";

				GameObject breakingScene = (GameObject)Instantiate(breaking, centralize);
				Stars (breakingScene);

				OpenNewsPapir ();

				countEntery++;
				if (countEntery > 14) {
					scroll.sizeDelta = new Vector2 (scroll.sizeDelta.x, scroll.sizeDelta.y + 50);
					centralize.anchoredPosition = new Vector2 (0, centralize.anchoredPosition.y + 25f);
				}
				_breaking = true;
			}
		} else if(type == Type.Attempts || type == Type.Time) {
			if(type == Type.Attempts) compiling.text = "Attempts remain: " + (maxCheck - countCheck);

			if(bull == currentCode.Length) {
				GameObject breakingScene = (GameObject)Instantiate(breaking, centralize);
				Stars (breakingScene);
				countEntery++;

				countBreaking++;
				if (type == Type.Attempts) {
					maxCheck += correctCode.Length;

					int addCode = countBreaking/3;
					if (addCode < 4) {
						// обновить
						lengthCode = 3 + addCode;
					}
					DefaultNamespace.SymbolKeyboard.FillTextField();
					DefaultNamespace.TextField.FillTextField(lengthCode);
					
				} 
				if (type == Type.Time) {
					maxTime += float.Parse (correctCode.Length.ToString ());

					int addCode = countBreaking/5;
					if (addCode < 4) {
						// обновить
						lengthCode = 3 + addCode;
					}
					DefaultNamespace.SymbolKeyboard.FillTextField();
					DefaultNamespace.TextField.FillTextField(lengthCode);
				} 


				CreateCode ();
				_breaking = true;

				if (countEntery > 14) {
					scroll.sizeDelta = new Vector2 (scroll.sizeDelta.x, scroll.sizeDelta.y + 50);
					centralize.anchoredPosition = new Vector2 (0, centralize.anchoredPosition.y + 25f);
				}

			}
		}
	}
	public void Stars(GameObject breakingScene) {
		int stars = 1;

		if (lengthCode == 3) {
			if (countEntery  > 6) {
				stars = 1;
			} else if (countEntery > 4) {
				stars = 2;
			} else if(countEntery <= 4) {
				stars = 3;
			}
		} else if (lengthCode == 4) {
			if (countEntery  > 8) {
				stars = 1;
			} else if (countEntery > 6) {
				stars = 2;
			} else if(countEntery <= 6) {
				stars = 3;
			}
		} else if (lengthCode == 5) {
			if (countEntery  > 14) {
				stars = 1;
			} else if (countEntery > 10) {
				stars = 2;
			} else if(countEntery <= 10) {
				stars = 3;
			}
		} else if (lengthCode == 6) {
			if (countEntery  > 15) {
				stars = 1;
			} else if (countEntery > 11) {
				stars = 2;
			} else if(countEntery <= 11) {
				stars = 3;
			}
		}
		for (int i = 1; i < stars + 1; i++) {
			breakingScene.transform.GetChild (i).GetChild (0).gameObject.SetActive (true);
		}
		AllStars += stars;
	}

	void OpenNewsPapir() {
		Debug.Log (AllStars / countBreaking);

		int stars = (int) AllStars / countBreaking;
		for (int i = 0; i < stars; i++) {
			newspaperStars.transform.GetChild (i).GetChild (0).gameObject.SetActive (true);
		}
		missionComplite.SetActive (true);
	}
	public void Touch() {
		AudioSource.PlayClipAtPoint (touch, Camera.main.transform.position);
	}
	public void Key() {
		AudioSource.PlayClipAtPoint (key, Camera.main.transform.position);
	}
	public void Slider() {
		AudioSource.PlayClipAtPoint (sliderAudio, Camera.main.transform.position, 0.25f);
	}
}
