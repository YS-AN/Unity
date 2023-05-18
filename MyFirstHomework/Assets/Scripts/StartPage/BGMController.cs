using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
	private AudioSource audioSource;

	[SerializeField]
	private Slider soundSlider;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		soundSlider.value = audioSource.volume;

		DontDestroyOnLoad(this.gameObject);
	}

	public void ChangeVolume()
	{
		audioSource.volume = soundSlider.value;
	}
}
