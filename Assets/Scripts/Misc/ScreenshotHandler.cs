using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;
    [SerializeField] private GameObject testPoint;

    private Camera cam;
    private bool takeScreenshot;

    private void Awake() {
        instance = this;
        cam = gameObject.GetComponent<Camera>();
        takeScreenshot = false;
    }

    private void OnPostRender() {
        if (takeScreenshot) {
            takeScreenshot = false;
            RenderTexture renderTexture = cam.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Input.png", byteArray);
            Debug.Log("Saved Input.png!");

            RenderTexture.ReleaseTemporary(renderTexture);
            cam.targetTexture = null;

            GameObject brush;
            while ((brush = GameObject.FindGameObjectWithTag("Brush")) != null) {
                Object.DestroyImmediate(brush);
            }

            TesseractHandler.Recognize_Static();
        }
    }

    private void TakeScreenshot() {
        int height = (int)(cam.scaledPixelHeight),
            width = (int)(cam.scaledPixelWidth);

        cam.targetTexture = RenderTexture.GetTemporary(width, height, 0);
        takeScreenshot = true;
    }

    public static void TakeScreenshot_Static() {
        instance.TakeScreenshot();
    }
}
