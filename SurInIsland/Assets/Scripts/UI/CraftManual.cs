using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManual : MonoBehaviour
{
    // 상태변수 
    private bool isActivated = false;

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))            // 바꿔야 될 수도
        {
            Window();
        }
    }

    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
}
