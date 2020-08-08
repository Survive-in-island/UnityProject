using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;
using UnityEngine.UI;

[System.Serializable]
public class Craft
{
    public string craftName;            // 이름
    public Sprite craftImage;           // 이미지
    public string craftDesc;            // 설명


    public string[] craftNeedItem;      // 필요한 아이템
    public int[] craftNeedItemCount;    // 필요한 아이템 개수 
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

    private int tabNumber = 0;
    private int page = 1;
    private int selectedSlotNumber;
    private Craft[] craft_SelectedTab;

    [SerializeField]
    private Craft[] craft_fire; // 모닥불용 탭
    [SerializeField]
    private Craft[] craft_build; // 건축용 탭

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

    // 필요한 UI slot 요소
    [SerializeField]
    private GameObject[] go_Slots;
    [SerializeField]
    private Image[] image_Slot;
    [SerializeField]
    private Text[] text_SlotName;
    [SerializeField]
    private Text[] text_SlotDesc;
    [SerializeField]
    private Text[] text_SlotNeedItem;

    // 필요한 컴포넌트
    private Inventory theInventory;

    private Transform buildPosition;

    FPSController controller;



    void Start()
    {
        theInventory = FindObjectOfType<Inventory>();

        controller = FindObjectOfType<FPSController>();

        tabNumber = 0;
        page = 1;
        TabSlotSetting(craft_fire);

    }

    public void TabSetting(int _tabNumber)
    {
        tabNumber = _tabNumber;
        page = 1;

        switch (tabNumber)
        {
            case 0:
                TabSlotSetting(craft_fire);// 불 세팅
                break;
            case 1:
                TabSlotSetting(craft_build);// 건축세팅
                break;
        }
    }

    private void ClearSlot()
    {
        for (int i = 0; i < go_Slots.Length; i++)
        {
            image_Slot[i].sprite = null;
            text_SlotDesc[i].text = "";
            text_SlotName[i].text = "";
            text_SlotNeedItem[i].text = "";
            go_Slots[i].SetActive(false);
        }
    }

    public void RightPageSetting()
    {
        if(page < (craft_SelectedTab.Length / go_Slots.Length) + 1)
            page++;
        else
            page = 1;

        TabSlotSetting(craft_SelectedTab);
    }

    public void LeftPageSetting()
    {
        if (page != 1)
            page--;
        else
            page = (craft_SelectedTab.Length / go_Slots.Length) + 1;

        TabSlotSetting(craft_SelectedTab);
    }

    private void TabSlotSetting(Craft[] _craft_tab)
    {
        ClearSlot();
        craft_SelectedTab = _craft_tab;

        int startSlotNumber = (page - 1) * go_Slots.Length; // 4의 배수.

        for (int i = startSlotNumber; i < craft_SelectedTab.Length; i++)
        {
            if (i == page * go_Slots.Length)
                break;

            go_Slots[i - startSlotNumber].SetActive(true);

            image_Slot[i - startSlotNumber].sprite = craft_SelectedTab[i].craftImage;
            text_SlotName[i - startSlotNumber].text = craft_SelectedTab[i].craftName;
            text_SlotDesc[i - startSlotNumber].text = craft_SelectedTab[i].craftDesc;

            for (int x = 0; x < craft_SelectedTab[i].craftNeedItem.Length; x++)
            {
                text_SlotNeedItem[i - startSlotNumber].text += craft_SelectedTab[i].craftNeedItem[x];
                text_SlotNeedItem[i - startSlotNumber].text += " x " + craft_SelectedTab[i].craftNeedItemCount[x] + "\n";
            }
        }
    }

    public void SlotClick(int _slotNumber)
    {
        selectedSlotNumber = _slotNumber + (page - 1) * go_Slots.Length;

        if (!CheckIngredient())
            return;

        //go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + (tf_Player.up) * 4 - tf_Player.forward - (tf_Player.right * 2), Quaternion.identity);
        go_Preview = Instantiate(craft_SelectedTab[selectedSlotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_SelectedTab[selectedSlotNumber].go_Prefab;
        buildPosition = go_Preview.transform;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    private bool CheckIngredient()
    {
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            if(theInventory.GetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i]) < craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i])
                return false;
        }

        return true;
    }

    private void UseIngredient()
    {
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            theInventory.SetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i], craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P) && !isPreviewActivated)            // 바꿔야 될 수도
        {
            controller.lockCursor = false;
            Window();
        }

        if (isPreviewActivated)
            PreviewPositionUpdate();

        if (Input.GetKeyDown(KeyCode.Mouse0))       // if(Input.GetKeyDown("Fire1"))
            Build();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            UseIngredient();
            Instantiate(go_Prefab, buildPosition.transform.position, go_Preview.transform.rotation); // 가운데 인자 hitInfo.point
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Prefab = null;
            go_Prefab = null;
            controller.lockCursor = true;
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

        controller.lockCursor = true;
    }

    private void PreviewPositionUpdate()
    {
        //if(Physics.Raycast(tf_Player.position + (tf_Player.up) * 4 - tf_Player.forward - (tf_Player.right * 3), tf_Player.forward, out hitInfo, range, layerMask))
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask)){
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;

                if (Input.GetKeyDown(KeyCode.T))
                    go_Preview.transform.Rotate(0, -90f, 0f);
                if (Input.GetKeyDown(KeyCode.Y))
                    go_Preview.transform.Rotate(0, 90f, 0f);

                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
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
        controller.lockCursor = false;

    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
        controller.lockCursor = true;
    }
}
