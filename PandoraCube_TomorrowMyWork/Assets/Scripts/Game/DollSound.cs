using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DollSound : MonoBehaviour
{
	private void OnMouseDown()
	{
		SoundManager.Instance.playGameSFX(5);
	}
}
