using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DarkTreeFPS;
namespace DarkTreeFPS
{
    public class KdSlot : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler
    {

        public Item item; // 획득한 아이템.
        public int itemCount; // 획득한 아이템의 개수.
        public Image itemImage; // 아이템의 이미지.

        [SerializeField] private bool isQuickSlot; //퀵슬롯 여부 판단.
        [SerializeField] private int quickSlotNumber; // 퀵슬롯 번호.


        //// 필요한 컴포넌트.
        //[SerializeField]
        //private Text text_Count;                // 아이템UI 띄우기 
        //[SerializeField]
        //private GameObject go_CountImage;

        //private ItemEffectDatabase theItemEffectDatabase;
        [SerializeField]
        private RectTransform baseRect; // 인벤토리 영역.
        [SerializeField]
        private RectTransform quickSlotBaseRect; // 퀵슬롯의 영역.
                                                 // private InputNumber theInputNumber;

        void Start()
        {
            //theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
            //theInputNumber = FindObjectOfType<InputNumber>();
        }

        // 이미지의 투명도 조절.
        private void SetColor(float _alpha)
        {
            Color color = itemImage.color;
            color.a = _alpha;
            itemImage.color = color;
        }

        // 아이템 획득
        public void AddItem(Item _item, int _count = 1)         // Inventory.cs에 있음  GiveItem ()
        {
            item = _item;
            itemCount = _count;
            //itemImage.sprite = item.icon;

            //if (item.type != Item.item.weapon)
            //{
            //    go_CountImage.SetActive(true);
            //    text_Count.text = itemCount.ToString();
            //}
            //else
            //{
            //    text_Count.text = "0";
            //    go_CountImage.SetActive(false);
            //}

            SetColor(1);
        }

        public int GetQuickSlotNumber()
        {
            return quickSlotNumber;
        }

        // 아이템 개수 조정.
        public void SetSlotCount(int _count)
        {
            itemCount += _count;
            //text_Count.text = itemCount.ToString();                   /////////////

            if (itemCount <= 0)
                ClearSlot();
        }


        // 슬롯 초기화.
        private void ClearSlot()
        {
            item = null;
            itemCount = 0;
            //itemImage.sprite = null;                                              //////////////////
            SetColor(0);

            //text_Count.text = "0";                                                        //////////
            //go_CountImage.SetActive(false);                                                   ///////
        }








        //// 마우스가 슬롯에 들어갈 때 발동.
        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    if (item != null)
        //        theItemEffectDatabase.ShowToolTop(item, transform.position);
        //}

        //// 슬롯에서 빠져나갈 때 발동.
        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    theItemEffectDatabase.HideToolTip();
        //}
    }
}
