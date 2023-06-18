using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;
    [SerializeField] private GameObject testPoint;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Text text;

    [SerializeField]
    private bool trainingMode;

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

    private void TakeScreenshot_ModelBuilding() {
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

        int i = 1;
        bool saved = false;
        while (!saved) {
            Utils.saveToFile = text.text + i + ".png";
            if (File.Exists(Application.persistentDataPath + "/test_data/" + Utils.saveToFile)) {
                i++;
            } else {
                saved = true;
            }
        }

        byte[] byteArray = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/test_data/" + Utils.saveToFile, byteArray);
        Debug.Log("Saved" + Utils.saveToFile);

        Destroy(renderTexture);
        renderTexture = null;

        GameObject brush;
        while ((brush = GameObject.FindGameObjectWithTag("Brush")) != null) {
            Object.DestroyImmediate(brush);
        }

        //TesseractHandler.Recognize_Static();
    }

    private void DeleteProgress() {
        GameObject brush;
        while ((brush = GameObject.FindGameObjectWithTag("Brush")) != null) {
            Object.DestroyImmediate(brush);
        }
    }

    private void Undo() {
        GameObject[] brush = GameObject.FindGameObjectsWithTag("Brush");
        if (brush != null) {
            Object.DestroyImmediate(brush[brush.Length - 1]);
        }
    }

    public static void TakeScreenshot_Static() {
        if (instance.trainingMode) {
            instance.TakeScreenshot_ModelBuilding();
        } else {
            instance.TakeScreenshot();
        }
    }

    public static void DeleteProgress_Static() {
        instance.DeleteProgress();
    }

    public static void Undo_Static() {
        instance.Undo();
    }

    public bool GetTrainingMode() {
        return trainingMode;
    }
}
