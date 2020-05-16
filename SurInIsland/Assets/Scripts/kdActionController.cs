using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTreeFPS;


public class kdActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 습득 가능한 최대 거리

    private bool pickupActivated = false; // 습득 가능할 시 true
    private bool dissolveActivated = false; // 고기 해체 가능할 시 true
    private bool isDissolving = false; // 고기 해체 중에는 true

    private RaycastHit hitInfo; // 충돌체 정보 저장

    // 아이템 레이어에만 반응하도록 레이어 마스크 설정
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;

    //[SerializeField]
    //private Inventory theInventory;

    //[SerializeField]
    //private Transform tf_MeatDissolveTool;  // 고기 해체


    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.T))        // 고기 해체만
        {
            CheckAction();
            //CanPickUP();
            CanMeat();
        }
    }

    private void CanMeat()
    {
        if (dissolveActivated == true)
        {
            if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "Pig" && hitInfo.transform.GetComponent<Animal>().isDead && !isDissolving)
            {
                isDissolving = true;
                InfoDisappear();

                // 고기 해체 시작 
                StartCoroutine(MeatCoroutine());
            }
        }
    }

    IEnumerator MeatCoroutine()
    {
        // 고기 해체 애니메이션 이 밑에 추가해줄 것

        yield return new WaitForSeconds(0.2f);

        // tf_MeatDissolveTool.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        // 고기 해체 사운드 
        yield return new WaitForSeconds(1.8f);

        // 인벤토리에 넣어주기 
        //theInventory.GiveItem(hitInfo.transform.GetComponent<Animal>().GetItem());

        // 여기는 애니메이션 종료될때

        isDissolving = false;
    }

    private void CanPickUP()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUP>().item.itemName + " 획득 ");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckAction()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")     // kdItem으로 바꿀수도
            {
                //ItemInfoAppear();
            }
            else if(hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "Pig")        // 태그는 바꿔야될수도 
            {
                MeatInfoAppear();
            }
        }
        else
            InfoDisappear();
    }

    //private void ItemInfoAppear()
    //{
    //    pickupActivated = true;
    //    actionText.gameObject.SetActive(true);
    //    actionText.text = hitInfo.transform.GetComponent<ItemPickUP>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    //}

    private void MeatInfoAppear()
    {
        if (hitInfo.transform.GetComponent<Animal>().isDead)
        {
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " 해체하기 " + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        dissolveActivated = false;
        actionText.gameObject.SetActive(false);
    }


}
