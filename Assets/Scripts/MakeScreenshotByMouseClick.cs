using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MakeScreenshotByMouseClick : MonoBehaviour
{
	public Camera mainCamera;

	int counter = 1;

	private void Update()
	{
		if (UnityEngine.Input.GetMouseButtonDown(1))
		{
			ScreenCapture.CaptureScreenshot("Assets/Screenshots/Sreenshot" + counter.ToString("00") + "_" + mainCamera.pixelWidth + "x" + mainCamera.pixelHeight + "_" + "_SceneID"+ SceneManager.GetActiveScene().name + ".png");
			counter++;
		}
	}
}
