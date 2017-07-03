using System.Collections;
using System.Collections.Generic;

public static class Utility
{
	public static T[] FisherYatesShuffle<T>(T[] array, int seed)
	{
		//create different random by pass-in seed
		System.Random ran = new System.Random(seed);

		//shuffle
		for(int i = 0; i < array.Length; i++)
		{
			int selection = ran.Next(i, array.Length);
			T temp = array[selection];
			array[selection] = array[i];
			array[i] = temp;
		}

		return array;
	}
}
