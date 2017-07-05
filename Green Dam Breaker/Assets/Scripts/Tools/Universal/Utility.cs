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

	//try implement Mathf.PingPong
	//t is any increasing number, length is the range (0, range)
	//return a number from 0 to length then length to 0 back and forth
	public static float MyMathPingPoing(float t, float length)
	{
		//given t is a increase number
		//when t is between 2length~3length, T is 0~length
		//when t is between 3length~4length, T is between length~2length, in this case, transfer length~2length to length~0
		float L = length * 2;
		float T = t % L;

		if(T >= 0 && T <= length)
		{
			return T;
		}else{
			return L - T;
		}
	}
}
