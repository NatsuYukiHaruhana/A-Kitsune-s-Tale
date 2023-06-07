using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TesseractHandler : MonoBehaviour
{
    public static TesseractHandler instance;
    [SerializeField] private Text displayText;
    [SerializeField] private RawImage outputImage;
    private Texture2D _texture;

    private TesseractDriver _tesseractDriver;
    private string recognizedText = "";
    private string errorMsg = "";
    private bool isDone = false;

    private void Start() {
        instance = this;

        _tesseractDriver = new TesseractDriver();
    }

    private void Recognize(Texture2D outputTexture) {
        _texture = outputTexture;
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
    }

    private void OnSetupCompleteRecognize() {
        recognizedText = _tesseractDriver.Recognize(_texture);
        errorMsg = "Error message: " + _tesseractDriver.GetErrorMessage();
        if (displayText != null) {
            displayText.text = recognizedText + errorMsg;
        }
        if (outputImage != null) { 
            SetImageDisplay();
        }
        Debug.Log("Recognized: " + recognizedText);
        isDone = true;
    }

    public static void Recognize_Static() {
        if (!File.Exists(Application.persistentDataPath + "/Input.png")) {
            return;
        }
        byte[] fileData = File.ReadAllBytes(Application.persistentDataPath + "/Input.png");
        
        Texture2D imageToRecognize = new Texture2D(0, 0);
        imageToRecognize.LoadImage(fileData);

        Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        texture.SetPixels32(imageToRecognize.GetPixels32());
        texture.Apply();

        instance.Recognize(texture);
    }

    public static string GetRecognizedText() {
        return instance.recognizedText;
    }

    public static string GetErrorMsg() {
        return instance.errorMsg;
    }

    public static bool GetIsDone() {
        return instance.isDone;
    }

    public static void ResetIsDone() {
        instance.isDone = false;
    }

    private void SetImageDisplay() {
        RectTransform rectTransform = outputImage.GetComponent<RectTransform>();

        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            rectTransform.rect.width *
            _tesseractDriver.GetHighlightedTexture().height /
            _tesseractDriver.GetHighlightedTexture().width);

        outputImage.texture = _tesseractDriver.GetHighlightedTexture();
    }
}
