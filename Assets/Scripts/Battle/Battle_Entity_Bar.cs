using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Entity_Bar : MonoBehaviour
{
    [SerializeField]
    private GameObject movableBar;
    private const float moveSpeed = .001f;
    private float percentage = 1f;
    private float targetPercentage = 1f;

    private void FixedUpdate() {
        if (Utils.ApproximatelyEqual(targetPercentage, percentage, 0.01f) == false) {
            float deltaMoveSpeed = moveSpeed + (Mathf.Abs(targetPercentage - percentage) * .01f);
            if (targetPercentage > percentage) {
                movableBar.transform.localPosition += new Vector3(deltaMoveSpeed, 0f, 0f);
                percentage += deltaMoveSpeed;
            } else {
                movableBar.transform.localPosition -= new Vector3(deltaMoveSpeed, 0f, 0f);
                percentage -= deltaMoveSpeed;
            }
        } else {
            movableBar.transform.localPosition = new Vector3(targetPercentage - 1f, 0f, 0f);
            percentage = targetPercentage;
            gameObject.SetActive(false);
        }
    }

    public void SetPercentage(float newPercentage) {
        percentage = newPercentage;
    }

    public void SetTargetPercentage(float newPercentage) {
        targetPercentage = newPercentage;
    }

    public void SetColor(Color newColor) {
        movableBar.GetComponent<SpriteRenderer>().color = newColor;
    }
}
