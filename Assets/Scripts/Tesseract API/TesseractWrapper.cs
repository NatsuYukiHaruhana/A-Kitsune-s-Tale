using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TesseractWrapper
{
#if UNITY_EDITOR
    private const string TesseractDLLName = "tesseract";
    private const string LeptonicalDLLName = "tesseract";
#elif UNITY_ANDROID
    private const string TesseractDLLName = "libtesseract.so";
    private const string LeptonicalDLLName = "liblept.so";
#else
    private const string TesseractDLLName = "tesseract";
    private const string LeptonicalDLLName = "tesseract";
#endif

    [DllImport(TesseractDLLName)]
    private static extern IntPtr TessVersion();

    [DllImport(TesseractDLLName)]
    private static extern IntPtr TessBaseAPICreate();

    [DllImport(TesseractDLLName)]
    private static extern int TessBaseAPIInit3(IntPtr handle, string dataPath, string language);

    [DllImport(TesseractDLLName)]
    private static extern void TessBaseAPIDelete(IntPtr handle);

    [DllImport(TesseractDLLName)]
    private static extern void TessBaseAPIEnd(IntPtr handle);

    [DllImport(TesseractDLLName)]
    private static extern void TessBaseAPISetImage(IntPtr handle, IntPtr imagedata, int width, int height, int bytes_per_pixel, int bytes_per_line);

    [DllImport(TesseractDLLName)]
    private static extern void TessBaseAPISetImage2(IntPtr handle, IntPtr pix);

    [DllImport(TesseractDLLName)]
    private static extern int TessBaseAPIRecognize(IntPtr handle, IntPtr monitor);

    [DllImport(TesseractDLLName)]
    private static extern IntPtr TessBaseAPIGetUTF8Text(IntPtr handle);

    [DllImport(TesseractDLLName)]
    private static extern void TessDeleteText(IntPtr text);

    [DllImport(TesseractDLLName)]
    private static extern void TessBaseAPIClear(IntPtr handle);

    [DllImport(TesseractDLLName)]
    private static extern IntPtr TessBaseAPIGetWords(IntPtr handle, IntPtr pixa);

    IntPtr tessHandle;
    private string _errMsg;

    private Texture2D _highlightedTexture;

    public TesseractWrapper() {
        tessHandle = IntPtr.Zero;
    }

    public bool Init(string lang, string dataPath) {
        if (!tessHandle.Equals(IntPtr.Zero)) {
            Close();
        }

        try {
            tessHandle = TessBaseAPICreate();
            if (tessHandle.Equals(IntPtr.Zero)) {
                _errMsg = "TessAPICreate failed";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dataPath)) {
                _errMsg = "Invalid DataPath";
                return false;
            }

            int init = TessBaseAPIInit3(tessHandle, dataPath, lang);
            if (init != 0) {
                Close();
                _errMsg = "TessAPIInit failed. Output: " + init;
                return false;
            }

            return true;
        } catch (Exception e) {
            _errMsg = e + " - " + e.Message;
            return false;
        }
    }

    public void Close() {
        if (tessHandle.Equals(IntPtr.Zero)) {
            return;
        }

        TessBaseAPIEnd(tessHandle);
        TessBaseAPIDelete(tessHandle);
        tessHandle = IntPtr.Zero;
    }

    public string Recognize(Texture2D texture) {
        if (tessHandle.Equals(IntPtr.Zero)) {
            return null;
        }

        _highlightedTexture = texture;

        int width = texture.width,
            height = texture.height;
        Color32[] colors = texture.GetPixels32();
        int count = width * height,
            bytesPerPixel = 4;
        byte[] data = new byte[count * bytesPerPixel];

        int bytePtr = 0;

        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                int colorIdx = y * width + x;
                data[bytePtr++] = colors[colorIdx].r;
                data[bytePtr++] = colors[colorIdx].g;
                data[bytePtr++] = colors[colorIdx].b;
                data[bytePtr++] = colors[colorIdx].a;
            }
        }

        IntPtr imagePtr = Marshal.AllocHGlobal(count * bytesPerPixel);
        Marshal.Copy(data, 0, imagePtr, count * bytesPerPixel);

        TessBaseAPISetImage(tessHandle, imagePtr, width, height, bytesPerPixel, width * bytesPerPixel);

        if (TessBaseAPIRecognize(tessHandle, IntPtr.Zero) != 0) {
            Marshal.FreeHGlobal(imagePtr);
            return null;
        }

        int pointerSize = Marshal.SizeOf(typeof(IntPtr));
        IntPtr intPtr = TessBaseAPIGetWords(tessHandle, IntPtr.Zero);
        Boxa boxa = Marshal.PtrToStructure<Boxa>(intPtr);
        Box[] boxes = new Box[boxa.n];

        for (int i = 0; i < boxes.Length; i++) {
            IntPtr boxPtr = Marshal.ReadIntPtr(boxa.box, i * pointerSize);
            boxes[i] = Marshal.PtrToStructure<Box>(boxPtr);
            Box box = boxes[i];
            DrawLines(texture, new Rect(box.x, texture.height - box.y - box.h, box.w, box.h), Color.green);
        }

        IntPtr str_ptr = TessBaseAPIGetUTF8Text(tessHandle);
        Marshal.FreeHGlobal(imagePtr);
        if (str_ptr.Equals(IntPtr.Zero)) {
            return null;
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        string recognizedText = Marshal.PtrToStringAnsi(str_ptr);
#else
        string recognizedText = Marshal.PtrToStringAuto(str_ptr);
#endif
        TessBaseAPIClear(tessHandle);
        TessDeleteText(str_ptr);

        return recognizedText;
    }

    private void DrawLines(Texture2D texture, Rect boundingRect, Color color, int thickness = 3) {
        int x1 = (int)boundingRect.x;
        int x2 = (int)(boundingRect.x + boundingRect.width);
        int y1 = (int)boundingRect.y;
        int y2 = (int)(boundingRect.y + boundingRect.height);

        for (int x = x1; x <= x2; x++) {
            for (int i = 0; i < thickness; i++) {
                texture.SetPixel(x, y1 + i, color);
                texture.SetPixel(x, y2 - i, color);
            }
        }

        for (int y = y1; y <= y2; y++) {
            for (int i = 0; i < thickness; i++) {
                texture.SetPixel(x1 + i, y, color);
                texture.SetPixel(x2 - i, y, color);
            }
        }

        texture.Apply();
    }

    public string Version() {
        IntPtr strPtr = TessVersion();
        string tessVersion = Marshal.PtrToStringAnsi(strPtr);
        return tessVersion;
    }

    public string GetErrMsg() {
        return _errMsg;
    }

    public Texture2D GetHighlightedTexture() {
        return _highlightedTexture;
    }
}
