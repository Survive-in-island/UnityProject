using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Kit
{
    public string kitName;
    public string kitDescription;

    public GameObject go_Kit_Prefab;
}

public class ComputerKit : MonoBehaviour
{
    [SerializeField]
    private Kit[] kits;

    [SerializeField]
    private Transform tf_ItemAppear; // 생성될 아이템 위치

    private bool isCraft = false;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickButton(int _slotNumber)
    {
        if (isCraft)
        {
            isCraft = true;

            StartCoroutine(CraftCoroutain(_slotNumber));
        }
    }


    IEnumerator CraftCoroutain(int _slotNumber)
    {
        yield return new WaitForSeconds(3f);

        Instantiate(kits[_slotNumber].go_Kit_Prefab, tf_ItemAppear.position, Quaternion.identity);
        isCraft = false;
    }

}
