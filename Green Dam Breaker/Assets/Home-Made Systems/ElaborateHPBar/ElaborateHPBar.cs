using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElaborateHPBar : MonoBehaviour
{
	[Header("Setting")]
	public float shortFreezeTime = 0.1f;
	public float shrunkSpeed = 0.5f;		//a multipiler
	public bool showText;

	[Header("UI Components")]
	public Image background;
	public Image foreground;
	public Image hurtground;
	public CanvasGroup textGroup;
	public Text currentHPText;
	public Text fullHPText;

	[Header("Data Monitor")]
	[SerializeField]private float barLength;
	[SerializeField]private float maxHp;
	[SerializeField]private float currentHp;
	private float hpPivot;	//the current percent of life, floating btw 0 ~ 1

	//flags
	private bool isShrunking = false;

	void Start()
	{
		//ResetBar(100, 1f);
	}

	public void ResetBar(float _maxHp, float startHealthPercent)
	{
		maxHp = _maxHp;

		startHealthPercent = Mathf.Clamp(startHealthPercent, 0f, 1f);
		currentHp = maxHp * startHealthPercent;

		hpPivot = startHealthPercent;
		foreground.fillAmount = hpPivot;
		hurtground.fillAmount = hpPivot;

		isShrunking = false;

		if(showText)
		{
			InitText(currentHp, maxHp);
		}else{
			HideText();
		}

		StopAllCoroutines();
	}

	void Update()
	{
		//TEST
//		if(Input.GetKeyDown(KeyCode.J))
//		{
//			UpdateHP(10f);
//		}
	}

	public void UpdateHP(float reduce)
	{			
		currentHp -= reduce;
		currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
		hpPivot = currentHp / maxHp;
		foreground.fillAmount = hpPivot;		//fore ground bar shrunk immediately

		if(showText)
		{
			UpdateText(currentHp);
		}

		StartCoroutine(HurtBarShrunkCo());		//hurt ground bar shrunk gradually and follows fore ground bar
	}

	IEnumerator HurtBarShrunkCo()
	{
		if(isShrunking)
			yield break;

		isShrunking = true;
		yield return new WaitForSeconds(shortFreezeTime);

		while(!Mathf.Approximately(foreground.fillAmount, hurtground.fillAmount))
		{
			hurtground.fillAmount = Mathf.MoveTowards(hurtground.fillAmount, foreground.fillAmount, shrunkSpeed * Time.deltaTime);		//let hurt bar follow fore bar
			yield return null;
		}

		hurtground.fillAmount = foreground.fillAmount;
		isShrunking = false;
	}

	void InitText(float _currentHP, float _maxHP)
	{
		textGroup.alpha = 1.0f;
		UpdateText(_currentHP);
		fullHPText.text = " / " + _maxHP.ToString();
	}
	void UpdateText(float _currentHP)
	{
		currentHPText.text = _currentHP.ToString();
	}
	public void HideText()
	{
		if(textGroup == null)
			return;
		
		textGroup.alpha = 0.0f;
		UpdateText(0.0f);
		fullHPText.text = 0.0f.ToString();
	}
}
