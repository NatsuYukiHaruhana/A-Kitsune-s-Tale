using UnityEngine;
using UnityEngine.UI;

public class TesseractDemo : MonoBehaviour
{
    [SerializeField] private Text displayText;
    [SerializeField] private Texture2D imageToRecognize;
    [SerializeField] private RawImage outputImage;
    private Texture2D _texture;

    private TesseractDriver _tesseractDriver;
    private string text = "";

    private void Start() {
        Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        texture.SetPixels32(imageToRecognize.GetPixels32());
        texture.Apply();

        _tesseractDriver = new TesseractDriver();
        Recognize(texture);
    }

    private void Recognize(Texture2D outputTexture) {
        _texture = outputTexture;
        ClearTextDisplay();
        AddToTextDisplay(_tesseractDriver.CheckTessVersion());
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
    }

    private void OnSetupCompleteRecognize() {
        AddToTextDisplay(_tesseractDriver.Recognize(_texture));
        AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
        SetImageDisplay();
    }

    private void ClearTextDisplay() {
        text = "";
    }

    private void AddToTextDisplay(string newText, bool isError = false) {
        if (string.IsNullOrWhiteSpace(newText)) { 
            return;
        }

        text += (string.IsNullOrWhiteSpace(displayText.text) ? "" : "\n") + newText;

        if (isError) {
            Debug.LogError(text);
        }
        else {
            Debug.Log(text);
        }
    }

    private void Update() {
        displayText.text = text;
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
