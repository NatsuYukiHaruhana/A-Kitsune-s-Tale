using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableObject : MonoBehaviour
{
    [SerializeField] private GameObject brushPrefab;

    private Brush activeBrush = null;

    private void Start() {
        float width = .1f;
        brushPrefab.GetComponent<LineRenderer>().startWidth = brushPrefab.GetComponent<LineRenderer>().endWidth = width;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Utils.GetMouseWorldPosition();
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.zero);
            
            if (hit.collider == null) {
                return;
            }

            GameObject newBrush = Instantiate(brushPrefab, this.transform);

            activeBrush = newBrush.GetComponent<Brush>();
        }

        if (Input.GetMouseButtonUp(0)) {
            if (activeBrush == null) {
                return;
            }

            Vector3 mousePos = Utils.GetMouseWorldPosition();
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.zero);

            if (hit.collider == null) {
                Destroy(activeBrush.gameObject);
            }

            activeBrush = null;
        }

        if (Input.GetMouseButton(0) && activeBrush != null) {
            Vector3 mousePos = Utils.GetMouseWorldPosition();

            activeBrush.UpdateLine(mousePos);
        }
    }
}
