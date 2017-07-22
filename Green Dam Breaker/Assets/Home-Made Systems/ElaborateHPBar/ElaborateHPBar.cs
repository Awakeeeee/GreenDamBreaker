using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElaborateHPBar : MonoBehaviour
{
	public float shortFreezeTime = 0.1f;
	public float shrunkTime = 0.5f;

	public Image background;
	public Image foreground;
	public Image hurtground;

	[SerializeField]private float barLength;
	[SerializeField]private float maxHp;
	[SerializeField]private float currentHp;
	private float hpPivot;

	void Start()
	{
		InitHpBar(1f);
	}

	public void Setup(float _maxHp)
	{
		maxHp = _maxHp;
	}

	public void InitHpBar(float startHealthPercent)
	{
		startHealthPercent = Mathf.Clamp(startHealthPercent, 0f, 1f);
		currentHp = maxHp * startHealthPercent;

		hpPivot = startHealthPercent;
		foreground.fillAmount = hpPivot;
		hurtground.fillAmount = hpPivot;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
			UpdateHP(10f);
		}
	}

	public void UpdateHP(float reduce)
	{
		currentHp -= reduce;
		currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

		hpPivot = currentHp / maxHp;
		foreground.fillAmount = hpPivot;
		float hurtAmount = hpPivot;

		StartCoroutine(HurtBarShrunkCo(hurtAmount));
	}

	IEnumerator HurtBarShrunkCo(float toAmount)
	{
		float timer = 0.0f;
		float shrunkSpeed = (hurtground.fillAmount - toAmount) / shrunkTime;

		yield return new WaitForSeconds(shortFreezeTime);

		while(timer < shrunkTime)
		{
			hurtground.fillAmount = Mathf.MoveTowards(hurtground.fillAmount, toAmount, shrunkTime * Time.deltaTime);

			timer += Time.deltaTime;
			yield return null;
		}

		hurtground.fillAmount = toAmount;
	}
}
