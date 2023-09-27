using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSpaceUI : MonoBehaviour
{

    private void Awake() {
        Show();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Hide();
        }
    }
    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
