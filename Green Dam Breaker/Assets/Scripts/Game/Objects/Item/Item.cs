using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The item that is already picked up
[CreateAssetMenu(fileName = "New Item", menuName = "New Item")]
public class Item : ScriptableObject
{
	public int itemID;
	public string itemName;
	[Multiline]
	public string infomation;
	public Sprite sp;
	public bool isKeyItem;
}
