using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    public static void TakeScreenshot() {
        ScreenshotHandler.TakeScreenshot_Static();
    }
}
