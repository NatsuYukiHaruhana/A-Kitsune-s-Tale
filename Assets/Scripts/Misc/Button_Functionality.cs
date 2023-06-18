using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Functionality : MonoBehaviour
{
    [SerializeField]
    private float radius;

    public bool IsButtonPressed() {
#if UNITY_EDITOR || UNITY_WIN
        if (Input.GetMouseButtonDown(0)) {
            if (Utils.CheckPointIsWithinRadius(transform.position, Utils.GetMouseWorldPosition(), radius)) {
                return true;
            }
        }
#elif UNITY_ANDROID
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);
            if (Utils.CheckPointIsWithinRadius(transform.position, Utils.GetTouchWorldPosition(touch), radius)) {
                return true;
            }
        }
#else
        if (Input.GetMouseButtonDown(0)) {
            if (Utils.CheckPointIsWithinRadius(transform.position, Utils.GetMouseWorldPosition(), radius)) {
                return true;
            }
        }
#endif
        return false;
    }
}
