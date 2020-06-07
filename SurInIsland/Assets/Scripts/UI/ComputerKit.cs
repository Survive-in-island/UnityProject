using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

[System.Serializable]
public class Kit
{
    public string kitName;
    public string kitDescription;
    public string[] needItemName;
    public int[] needItemNumber;

    public GameObject go_Kit_Prefab;
}

public class ComputerKit : MonoBehaviour
{
    [SerializeField]
    private Kit[] kits;

    [SerializeField]
    private Transform tf_ItemAppear; // 생성될 아이템 위치

    private bool isCraft = false;   // 중복 실행 방지

    // 필요한 컴포넌트
    private Inventory theInven;

    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickButton(int _slotNumber)
    {
        if (!isCraft)
        {
            //if (!CheckIngredient(_slotNumber))
            //    return;

            isCraft = true;
            StartCoroutine(CraftCoroutain(_slotNumber));
        }
    }

    //private bool CheckIngredient(int _slotNumber)
    //{
    //    for (int i = 0; i < kits[_slotNumber].needItemName.Length; i++)
    //    {
    //        if (theInven.GetItemCount(kits[_slotNumber].needItemName[i] < kits[_slotNumber].needItemNumber[i]))
    //            return false;
    //    }
    //    return true;
    //}

    IEnumerator CraftCoroutain(int _slotNumber)
    {
        yield return new WaitForSeconds(3f);

        Instantiate(kits[_slotNumber].go_Kit_Prefab, tf_ItemAppear.position, Quaternion.identity);
        isCraft = false;
    }

}
