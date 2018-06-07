using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyClass
{
	public static string myString = "This is my string from my class in my assembly";

	public int LogMyString ()
	{
		//Debug.Log (myString);
		//print("--LogMyString--");
		return 2 + 2;
	}
}