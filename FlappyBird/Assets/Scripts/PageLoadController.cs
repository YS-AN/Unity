using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageLoadController : MonoBehaviour
{
	public void ChangeSceneByName(string sceneNm)
	{
		SceneManager.LoadScene(sceneNm, LoadSceneMode.Single);
	}
}
