using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    [SerializeField]
    private ScreenshotHandler screenshotHandler;

    private void Update() {
        if (screenshotHandler.GetTrainingMode()) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                ScreenshotHandler.TakeScreenshot_Static();
            } else if (Input.GetKeyDown(KeyCode.X)) {
                ScreenshotHandler.DeleteProgress_Static();
            } else if (Input.GetKeyDown(KeyCode.C)) {
                ScreenshotHandler.Undo_Static();
            }
        }
    }

    public static void TakeScreenshot() {
        ScreenshotHandler.TakeScreenshot_Static();
    }

    public static void Clear() {
        ScreenshotHandler.DeleteProgress_Static();
    }

    public static void Undo() {
        ScreenshotHandler.Undo_Static();
    }
}
