using UnityEngine;
using System.Collections;
using UnityEditor;

public static class EditorExtensionMethods
{
	public static void AddElementToArray(this SerializedProperty arrayProp, Object element)
	{
		if (!arrayProp.isArray)
		{
			Debug.LogError("Trying to add element to a non-array SerializedProperty, stop executing.");
			return;
		}

		arrayProp.serializedObject.Update();

		///firstly insert an empty element to array, because there is a built-in method to do this
		///then set the empty element to the one we want
		arrayProp.InsertArrayElementAtIndex(arrayProp.arraySize);
		arrayProp.GetArrayElementAtIndex(arrayProp.arraySize - 1).objectReferenceValue = element;

		arrayProp.serializedObject.ApplyModifiedProperties();
	}

	/// <summary>
	/// My version of SerializedProperty.DeleteArrayElementAtIndex().
	/// </summary>
	public static void RemoveElementAtIndex(this SerializedProperty arrayProp, int index)
	{
		if (!arrayProp.isArray)
		{
			Debug.LogError("Trying to remove element from a non-array SerializedProperty, stop executing.");
			return;
		}

		if (index < 0f || index > arrayProp.arraySize - 1)
		{
			Debug.LogError("RemoveElementAtIndex : Pass in index is not valid!");
			return;
		}

		arrayProp.serializedObject.Update();

		arrayProp.DeleteArrayElementAtIndex(index);

		arrayProp.serializedObject.ApplyModifiedProperties();
	}

	public static void RemoveElementFromArray<T>(this SerializedProperty arrayProp, T element)
		where T : Object
	{
		if (!arrayProp.isArray)
		{
			Debug.LogError("Trying to remove eleenmt from a non-array SerializedProperty, stop executing.");
			return;
		}

		if (element == null)
		{
			Debug.LogError("RemoveElementFromArray : deleting a null element is not supported!");
			return;
		}

		arrayProp.serializedObject.Update();

		for (int i = 0; i < arrayProp.arraySize; i++)
		{
			SerializedProperty arrayElement = arrayProp.GetArrayElementAtIndex(i);
			if (arrayElement.objectReferenceValue == element)
			{
				arrayProp.RemoveElementAtIndex(i);
				return;
			}
		}

		Debug.LogError("RemoveElementFromArray: the element trying to remove is not in the SerializedProperty array.");
		arrayProp.serializedObject.ApplyModifiedProperties();
	}

	public static void ClearArray(this SerializedProperty arrayProp)
	{
		if (!arrayProp.isArray)
		{
			Debug.LogError("Trying to clear a non-array SerializedProperty, stop executing.");
			return;
		}

		arrayProp.serializedObject.Update();

		for (int i = 0; i < arrayProp.arraySize; i++)
		{
			arrayProp.RemoveElementAtIndex(i);
		}

		arrayProp.serializedObject.ApplyModifiedProperties();
	}
}
