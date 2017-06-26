using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class EditorWithSubEditors<TSubEditor, TSubObject> : Editor
	where TSubEditor : Editor
	where TSubObject : Object
{
	public TSubEditor[] subEditorList;

	public void SetupSubEditorList(TSubObject[] objectList)
	{
		//handle null object list
		if(objectList == null)
		{
			Debug.LogWarning("Incoming sub-objcet list is null.");
			ClearSubEditorList();
			return;
		}

		//handle null element in object list
		for(int i = 0; i < objectList.Length; i++)
		{
			if(objectList[i] == null)
			{
				Debug.LogWarning("Incoming sub-object list has null element.");
				ClearSubEditorList();
				return;
			}
		}

		//current sub-editor is correct
		if(subEditorList.Length == objectList.Length)
			return;

		//create sub-editors for sub-objects
		ClearSubEditorList();
		subEditorList = new TSubEditor[objectList.Length];
		for(int i = 0; i < subEditorList.Length; i++)
		{
			subEditorList[i] = CreateEditor(objectList[i]) as TSubEditor;
			InitEachSubEditor(subEditorList[i]);
		}
	}

	void ClearSubEditorList()
	{
		if(subEditorList == null)
			return;

		for(int i = 0; i < subEditorList.Length; i++)
		{
			DestroyImmediate(subEditorList[i]);
		}

		subEditorList = null;
	}

	protected abstract void InitEachSubEditor(TSubEditor editor);
}
