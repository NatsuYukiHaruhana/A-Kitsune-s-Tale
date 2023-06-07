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
}
