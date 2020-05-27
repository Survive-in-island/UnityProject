using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;            // 이름
    public GameObject go_Prefab;        // 실제 설치될 프리팹
    public GameObject go_PreviewPrefab; // 미리보기 프리팹
}
public class CraftManual : MonoBehaviour
{
    // 상태변수 
    private bool isActivated = false;

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire; // 모닥불용 탭

    private GameObject go_Preview; // 미리보기 프리팹을 담을 변수 

    private Transform tf_Player;

    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
    }

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
