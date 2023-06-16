using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;

public class ScrollViewBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameObject contentToFillPrefab;

    private List<GameObject> contentList;

    VerticalLayoutGroup verticalLayoutGroup;

    private void Awake() {
        contentList = new List<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void CreateNewContent(string contentText) {
        contentList.Add(Instantiate(contentToFillPrefab, content.transform));
        contentList[contentList.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = contentText;
    }

    public int GetContentCount(string contentText) {
        int count = 0;
        
        foreach(GameObject content in contentList) {
            string text = contentList[contentList.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text;

            string compareText = text.Remove(text.LastIndexOf('X') - 1);
            if (compareText == contentText) {
                count = int.Parse(text.Substring(text.LastIndexOf('X') + 1));
            }
        }

        return count;
    }

    public void ReplaceContent(string contentToReplace, string newContent) {
        foreach (GameObject content in contentList) {
            if (content.GetComponentInChildren<TextMeshProUGUI>().text == contentToReplace) {
                content.GetComponentInChildren<TextMeshProUGUI>().text = newContent;
                break;
            }
        }
    }

    public void RemoveContent(string contentToRemove) {
        GameObject contentObjectToRemove = null;
        foreach (GameObject content in contentList) {
            if (content.GetComponentInChildren<TextMeshProUGUI>().text == contentToRemove) {
                contentObjectToRemove = content;
                break;
            }
        }

        if (contentObjectToRemove != null) {
            Destroy(contentObjectToRemove);
            contentList.Remove(contentObjectToRemove);
        }
    }

    public void ContentCountDown(string contentText) {
        foreach (GameObject content in contentList) {
            string text = contentList[contentList.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text;

            string compareText = text.Remove(text.LastIndexOf('X') - 1);
            if (compareText == contentText) {
                int count = int.Parse(text.Substring(text.LastIndexOf('X') + 1));

                if (count == 1) {
                    RemoveContent(text);
                } else {
                    contentList[contentList.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = compareText + " X " + (count - 1);
                }
                break;
            }
        }
    }
}
