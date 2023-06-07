using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;
    [SerializeField] private GameObject testPoint;

    [SerializeField]
    private Camera cam;

    private void Awake() {
        instance = this;
    }

    private void TakeScreenshot() {
        int height = (int)(cam.scaledPixelHeight),
            width = (int)(cam.scaledPixelWidth);

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);

        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        cam.targetTexture = null;
        RenderTexture.active = null;

        byte[] byteArray = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Input.png", byteArray);
        Debug.Log("Saved Input.png!");

        Destroy(renderTexture);
        renderTexture = null;

        GameObject brush;
        while ((brush = GameObject.FindGameObjectWithTag("Brush")) != null) {
            Object.DestroyImmediate(brush);
        }

        TesseractHandler.Recognize_Static();
    }

    public static void TakeScreenshot_Static() {
        instance.TakeScreenshot();
    }
}
