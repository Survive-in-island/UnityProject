/// DarkTreeDevelopment (2019) DarkTree FPS v1.2
/// If you have any questions feel free to write me at email --- darktreedevelopment@gmail.com ---
/// Thanks for purchasing my asset!

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;       // 

namespace DarkTreeFPS
{
    public class Inventory : MonoBehaviour
    {
        [System.Serializable]
        public class OnAddItem : UnityEvent { }

        public List<Item> characterItems = new List<Item>();
        public UIInventory inventoryUI;

        public bool debug = true;
        
        public OnAddItem onAddItem;

        //Method to add item to inventory
        public void GiveItem(Item item)
        {
            if (CheckFreeSpace() == false)
            {
                return;
            }

            characterItems.Add(item);
            inventoryUI.AddNewItem(item);
            item.gameObject.SetActive(false);

            if (debug) Debug.Log("Added item: " + item.title);

            //Events
            item.onPickupEvent.Invoke();
            onAddItem.Invoke();
        }

        //Method return true if inventory found free space
        public bool CheckFreeSpace()
        {
            if (inventoryUI.UIItems.FindLast(i => i.item == null))
            {
                if (debug)
                    Debug.Log("Free space found");
                return true;
            }
            if (debug)
                Debug.Log("No free space found");
            return false;
        }

        //Method to check if item reference exist in inventory. Used by remove method to check if we delete item that is really exist
        public Item CheckForItem(Item item)
        {
            return characterItems.Find(x => item.GetHashCode() == x.GetHashCode());
        }

        //Method to check if we have an item or items that have needed name. Will return true if find first suitable item
        public bool CheckIfItemExist(string name)
        {
            if (characterItems.Find(x => name == x.title))
                return true;
            else
                return false;
        }

        //Remove item by name. If destroy true inventory will not drop item ahead of player after remove.
        public void RemoveItem(string name, bool destroy)
        {
            var _item = characterItems.Find(x => name == x.title);

            if (_item != null)
            {
                if (_item.gameObject != null && !destroy)
                {
                    _item.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
                    _item.gameObject.SetActive(true);
                }

                if (debug)
                    Debug.Log("Remove item: " + _item.title);

                characterItems.Remove(_item);
                inventoryUI.RemoveItem(_item);
            }
            else
            {
                if (debug)
                    Debug.Log("No item found");
            }
        }

        //Remove item by item reference
        public void RemoveItem(Item item, bool destroy)
        {
            var _item = CheckForItem(item);

            if (_item != null)
            {
                if (_item.gameObject != null && !destroy)
                {
                    _item.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
                    _item.gameObject.SetActive(true);
                }

                if (debug)
                    Debug.Log("Remove item: " + _item.title);

                characterItems.Remove(_item);
                inventoryUI.RemoveItem(_item);
            }
            else
            {
                if (debug)
                    Debug.Log("No item found");
            }
        }

        public void UseItem(Item item, bool closeInventory)
        {
            item.onUseEvent.Invoke();

            // !!! Crutch for grenade object !!!
            if (item.id != 105)
                RemoveItem(item, true);

            if (closeInventory)
                InventoryManager.showInventory = false;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        ///

        //public int GetItemCount(string _itemName)
        //{
        //    int temp = SearchSlotItem(characterItems, characterItems[0].title);

        //    if (temp != 0)
        //        return temp;

        //    else
        //        return 0;
        //    //return temp != 0 ? temp : SearchSlotItem()              // 앞에 조건을 만족하면 temp를 리턴 그렇지 않으면 뒤에 리턴
        //}

        //private int SearchSlotItem(List<Item> _slots, string _itemName)
        //{
        //    for (int i = 0; i < 18; i++)
        //    {
        //        if(_itemName == _slots)
        //    }
        //}


        // 고기 해체 하기 위한 코드
        //[SerializeField]
        //private Transform tf_MeatDissolveTool;      // 고기 해체 툴

        //private bool dissolveActivated = false;  // 고기 해체 가능할 시 true
        //private bool isDissolving = false; // 고기 해체 충에는 true

        //[SerializeField]
        //private Text actionText;

        //private RaycastHit hitInfo; // 충돌체 정보 저장


        //private void MeatInfoAppear(Item item)
        //{
        //    dissolveActivated = true;
        //    item.gameObject.SetActive(true);

        //    //Debug.Log("해체하기");
        //    actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " 해체하기 " + "<color=yellow>" + "(E)" + "</color>";
        //}


        ////////////////////////////////////////////////////////////////////////////////////////////
    }
}