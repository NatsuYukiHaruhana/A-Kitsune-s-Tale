using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    public void TakeScreenshot() {
        ScreenshotHandler.TakeScreenshot_Static();
    }
}
