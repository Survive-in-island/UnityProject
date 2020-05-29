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
    public bool isActivated = false;
    public bool isPreviewActivated = false;

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire; // 모닥불용 탭

    private GameObject go_Preview; // 미리보기 프리팹을 담을 변수 
    private GameObject go_Prefab; // 실제 생성될 프리팹을 담을 변수

    [SerializeField]
    private Transform tf_Player;

    // 레이캐스트 필요 변수 선언
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float range;

    private Transform buildPosition;

    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + (tf_Player.up) * 4 - tf_Player.forward - (tf_Player.right * 2), Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        buildPosition = go_Preview.transform;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPreviewActivated)            // 바꿔야 될 수도
        {
            Window();
        }

        if (isPreviewActivated)
            PreviewPositionUpdate();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Build();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, buildPosition.transform.position, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Prefab = null;
            go_Prefab = null;

        }
    }

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;

        go_BaseUI.SetActive(false);
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tf_Player.position + (tf_Player.up) * 4 - tf_Player.forward - (tf_Player.right * 2), tf_Player.forward, out hitInfo, range, layerMask)){
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
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
