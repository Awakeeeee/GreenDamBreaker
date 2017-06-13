using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearArea : MonoBehaviour 
{
	public float confirmTime;
	[SerializeField]private float searchTimer;
	public enum SearchState
	{
		Searching,
		Found
	}
	public SearchState searchState;

	void Update()
	{
		if(Input.GetButton("Search"))
		{
			SearchClearArea();	
		}else if(Input.GetButtonUp("Search"))
		{
			ResetSearch();
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.GetComponent<FPSCharacterController>())
			return;
		
		Debug.Log("Area is not clear!");
		ResetSearch();
	}

	void SearchClearArea()
	{
		if(searchTimer < confirmTime && searchState == SearchState.Searching)
		{
			searchTimer += Time.deltaTime;

			if(searchTimer >= confirmTime && searchState != SearchState.Found)
			{
				Debug.Log("clear area found");
				searchState = SearchState.Found;

				//Send message to notify player clear area is confirmed
				SendMessageUpwards("OnFindClearArea");
			}
		}
	}

	void ResetSearch()
	{
		searchTimer = 0.0f;
		searchState = SearchState.Searching;
	}
}
