using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneController : PersistentSingletonBase<SceneController>
{
	public CanvasGroup fadePanel;
	public CanvasGroup overallBackground;
	public Text overallPrompt;
	private Image overallBgImage;
	public int firstSceneToLoad = 1;

	public float fadeTime = 1.0f;
	public bool isFading = false;

	public event Action LoadSceneEvent;
	public event Action UnLoadSceneEvent;

	IEnumerator Start()
	{
		overallBgImage = overallBackground.GetComponent<Image>();

		fadePanel.alpha = 1.0f;
		fadePanel.blocksRaycasts = true;

		overallBgImage.fillAmount = 0f;
		overallBackground.alpha = 0f;

		yield return StartCoroutine(LoadAndActiveScene(firstSceneToLoad));

		yield return StartCoroutine(ScreenFade(0.0f));
	}

	//External point
	public void MyLoadScene(int sceneID, bool fadeIn = true, bool fadeOut = true, bool isGameLevel = false)
	{
		if(isFading)
			return;
		
		StartCoroutine(LoadSceneProcess(sceneID, fadeIn, fadeOut, isGameLevel));
	}
	public void LoadNextScene(bool fadeIn = true, bool fadeOut = true, bool isGameLevel = false)
	{
		if(isFading)
			return;

		int nextSceneID = 1;
		int thisSceneID = SceneManager.GetActiveScene().buildIndex;
		if(thisSceneID + 1 < SceneManager.sceneCountInBuildSettings)
		{
			nextSceneID = thisSceneID + 1;
			//sceneName = SceneManager.GetSceneByBuildIndex(thisSceneID + 1).name;	//GetSceneByBuildIndex only return valid scene if the scene is loaded
		}
		StartCoroutine(LoadSceneProcess(nextSceneID, fadeIn, fadeOut, isGameLevel));
	}

	IEnumerator LoadSceneProcess(int sceneID, bool fadeIn, bool fadeOut, bool isGameLevel)
	{
		if(fadeIn)
		{
			yield return StartCoroutine(ScreenFade(1f));	
		}

		if(UnLoadSceneEvent != null)
		{
			UnLoadSceneEvent();
		}

		yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

		yield return StartCoroutine(LoadAndActiveScene(sceneID));

		if(LoadSceneEvent != null)
		{
			LoadSceneEvent();
		}

		if(fadeOut)
		{
			fadePanel.alpha = 1f;
			yield return StartCoroutine(ScreenFade(0f));
		}

		if(isGameLevel)
		{
			yield return StartCoroutine(SceneStartFade());
		}
	}

	IEnumerator LoadAndActiveScene(int sceneID)
	{
		yield return SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);	//load scene in background but not active yet

		Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);	//find the loaded scene

		SceneManager.SetActiveScene(newScene);	//set it active
	}

	IEnumerator ScreenFade(float targetAlpha)
	{
		isFading = true;
		fadePanel.blocksRaycasts = true;

		float fadeSpeed = Mathf.Abs(fadePanel.alpha - targetAlpha) / fadeTime;

		while(!Mathf.Approximately(fadePanel.alpha, targetAlpha))
		{
			float newAlpha = Mathf.MoveTowards(fadePanel.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
			fadePanel.alpha = newAlpha;
				
			yield return null;
		}

		fadePanel.blocksRaycasts = false;
		isFading = false;
	}

	IEnumerator SceneStartFade()
	{
		float percent = 0f;

		while(percent < 1f)
		{
			float x = Mathf.Lerp(0f, 1f, percent);
			overallBgImage.fillAmount = x;
			overallBackground.alpha = x;

			percent += Time.deltaTime * 2f;
			yield return null;
		}

		overallBgImage.fillAmount = 1f;
		overallBackground.alpha = 1f;

		yield return new WaitForSeconds(1.5f);

		overallBgImage.fillAmount = 0f;
		overallBackground.alpha = 0f;
	}

	//2 options:
	//fail end: prompt fade in with screen fade balck
	//success end: prompt fade in without screen fade black
	public IEnumerator SceneEndFade(bool isFailEnd)
	{
		float percent = 0f;

		while(percent < 1f)
		{
			float x = Mathf.Lerp(0f, 1f, percent);
			overallBackground.alpha = x;
			if(isFailEnd)
			{
				fadePanel.alpha = x;
			}

			percent += Time.deltaTime;
			yield return null;
		}

		overallBackground.alpha = 1;
		if(isFailEnd)
		{
			fadePanel.alpha = 1f;
		}

		yield return new WaitForSeconds(2f);

		overallBackground.alpha = 0f;
	}

	public void SetOverallPropmpt(string s, Color textCol)
	{
		overallPrompt.color = textCol;
		overallPrompt.text = s;
	}
}
