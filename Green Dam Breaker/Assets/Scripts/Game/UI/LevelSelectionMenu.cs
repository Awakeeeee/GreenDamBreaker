using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour 
{
	public ModeContentGroup[] contents;

	private bool isScrolling = false;

	void Start()
	{
		isScrolling = false;
	}

	public void ScrollRight(int contentIndex)
	{
		if(isScrolling)
			return;

		int dir = 1;

		if(contents[contentIndex].pivot + dir > contents[contentIndex].childs.Length - 1)
			return;

		contents[contentIndex].pivot += dir;

		StartCoroutine(ScrollCo(contentIndex, dir));

	}

	public void ScrollLeft(int contentIndex)
	{
		if(isScrolling)
			return;

		int dir = -1;

		if(contents[contentIndex].pivot + dir < 0)
			return;

		contents[contentIndex].pivot += dir;

		StartCoroutine(ScrollCo(contentIndex, dir));
	}

	IEnumerator ScrollCo(int contentIndex, int dir)
	{
		isScrolling = true;

		float percent = 0f;
		ModeContentGroup mcg = contents[contentIndex];

		while(percent < 1f)
		{
			float fromX = mcg.startX - (mcg.pivot - dir) * mcg.scrollAmount;
			float toX = mcg.startX - mcg.pivot * mcg.scrollAmount;
			float newX = Mathf.Lerp(fromX, toX, percent);
			mcg.content.localPosition = new Vector3(newX, mcg.content.localPosition.y, mcg.content.localPosition.z);

			percent += Time.deltaTime * mcg.scrollSpeed;
			yield return null;
		}

		mcg.content.localPosition = new Vector3(mcg.startX - mcg.pivot * mcg.scrollAmount, mcg.content.localPosition.y, mcg.content.localPosition.z);

		isScrolling = false;
	}
}

[System.Serializable]
public class ModeContentGroup
{
	public RectTransform content;
	public float startX;	//the start Xpos
	public RectTransform[] childs;
	public float scrollAmount;	//how much Xpos is scroll from one card to the next
	public float scrollSpeed = 2f;

	public float endX {
		get { 
			return startX - scrollAmount * (childs.Length - 1);
		}
	}

	public int pivot = 0;	//which card is currently at
}