using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	public GameManager gm;

	public Animation menu;
	public Animation output;
	public Animation newsPaper;

	private bool a_newsPaper;
	private int values;

	void Start() {
		if (gm.type == GameManager.Type.Endless) {
			gm.slider.gameObject.SetActive (true);
			gm.newspaperValue.gameObject.SetActive (true);
		}

		values = GameManager.lengthCodeGameSetting;
		gm.slider.value = values;
	}

	private bool activeMenu = true;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if(!menu.IsPlaying ("gameMenu(diactive)") && !menu.IsPlaying ("gameMenu(active)"))
				OnActive (activeMenu);
		}
	}

	public void OnActive(bool on) {
		if (on) {
			menu.gameObject.SetActive (true);
			Invoke ("timeControl", 0.7f);
		} else {
			Time.timeScale = 1;
			menu.Play ("gameMenu(diactive)");
			Invoke ("enableMenu", 0.7f);
		}
		activeMenu = !activeMenu;
	}
	void enableMenu() {
		menu.gameObject.SetActive (false);
	}
	void timeControl() {
		Time.timeScale = 0;
	}

	public void Restart() {
		Time.timeScale = 1;
		output.Play ("game(diactive)");
		Invoke ("RestartTime", 0.5f);
	}
	void RestartTime() {
		Application.LoadLevel (Application.loadedLevel);
	}


	public void LoadLevel() {
		Time.timeScale = 1;
		output.Play ("game(diactive)");
		Invoke ("LoadLevelTime", 0.5f);
	}
	void LoadLevelTime() {
		Application.LoadLevel (0);
	}

	public void NewsPaper() {
		if (gm.type == GameManager.Type.Endless) {
			//if (!a_newsPaper) {
				//newsPaper.Play ("NewsPaper (active)");
				//a_newsPaper = true;
			//} else {
				//newsPaper.CrossFade ("NewsPaper (diactive)");
				Invoke ("NewsPaperTime", 0.7f);
				//a_newsPaper = false;
			//}
		} else
			RestartTime ();
	}

	void NewsPaperTime() {
		newsPaper.Play ("NewsPaper (diactive2)");

		if (values < 3)
			values = 3;

		GameManager.lengthCodeGameSetting = values;

		gm.Start ();
		DefaultNamespace.SymbolKeyboard.FillTextField();
		DefaultNamespace.TextField.FillTextField(values);
		Invoke ("NewsPaperActiveTime", 0.5f);
	}
	void NewsPaperActiveTime() {
		newsPaper.gameObject.SetActive (false);
	}

	public void newpaperSlider(float value) {
		/*string text = "";
		if(value == 3) text = "easy";
		else if(value == 4) text = "medium";
		else if(value == 5) text = "hard";
		else if(value == 6) text = "bdsm";

		gm.newspaperValue.text = "complexity: " + text;*/

		gm.newspaperValue.text = "complexity: " + value;
		values = (int)value;
	}
}
