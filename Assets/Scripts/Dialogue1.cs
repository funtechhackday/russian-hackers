using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue1 : MonoBehaviour
{
	[SerializeField] private GameObject mDialog;
	[SerializeField] private Text messageContainer;
	[SerializeField] private List<Dialogue> messageList;
	[SerializeField] private GameObject sybolKeyboardHelp;
	[SerializeField] private GameObject consoleHelp;
	[SerializeField] private GameObject disablerHackBtn;
	[SerializeField] private GameObject dialogBg;
	[SerializeField] private GameObject dialogBgHalf;

	private int currentMessage;
	private bool keyboardLighting;
	private bool consoleLighting;
	private bool firstEnter = false;
	
	private static bool isShow = false;

	public void OnShow()
	{
		Show();
	}
	
	public void OnHide()
	{
		Hide();
	}

	private void Show()
	{
		mDialog.SetActive(true);
	}
	
	private void Hide()
	{
		mDialog.SetActive(false);
	}

	public void OnHack()
	{
		if (!firstEnter)
		{
			if (TextField.IsFilledField())
			{
				sybolKeyboardHelp.SetActive(true);
				consoleHelp.SetActive(false);
				dialogBg.SetActive(false);
				dialogBgHalf.SetActive(true);
				Show();
				firstEnter = true;
			}
		}
	}

	public void OnNextSentences()
	{
		Debug.Log("On next Id: " + currentMessage);
		if (mDialog.activeSelf)
		{
			switch (currentMessage)
			{
				case 0:
					NextSentences(currentMessage);
					break;
				case 1:
					NextSentences(currentMessage);
					break;
				case 2:
					NextSentences(currentMessage);
					break;
				case 3:
					NextSentences(currentMessage);
					break;
				case 4:
					NextSentences(currentMessage);
					break;
				case 5:
					NextSentences(currentMessage);
					break;
				case 6:
					NextSentences(currentMessage);
					Hide();
                    sybolKeyboardHelp.SetActive(false);
					break;
				case 7:
					NextSentences(currentMessage);
					Hide();
					disablerHackBtn.SetActive(false);
					break;
				case 8:
					NextSentences(currentMessage);
					break;
				case 9:
					NextSentences(currentMessage);
					/*Hide();
					sybolKeyboardHelp.SetActive(false);*/
					break;
				case 10:
					Hide();
					sybolKeyboardHelp.SetActive(false);
					break;
			}
			/*if (currentMessage < messageList.Count)
			{*/
				currentMessage++;
				Debug.Log("After Mesage Id: " + currentMessage);
			//}
			//NextSentences(currentMessage);
			
		}
	}
	
	IEnumerator FillMessage(string message)
	{
		messageContainer.text = "";

		foreach (char mChar in message)
		{
			messageContainer.text += mChar;
			
			yield return new WaitForSeconds(0.05f);
		}
	}

	private void NextSentences(int idMess)
	{
		Debug.Log("Before Mesage Id: " + currentMessage);
		StopAllCoroutines();
		StartCoroutine(FillMessage(messageList[idMess].sentences));
	}
	
	// Use this for initialization
	void Start ()
	{
		keyboardLighting = true;
		consoleLighting = true;
		
		mDialog.SetActive(true);
		
		currentMessage = 0;
		
		OnShow();
		NextSentences(currentMessage);

		currentMessage++;
	}
	
	// Update is called once per frame
	void Update () {

		if (keyboardLighting)
		{
			if (TextField.IsFilledField())
			{
				sybolKeyboardHelp.SetActive(false);
				Show();
				keyboardLighting = false;
			}
		}

	}
}
