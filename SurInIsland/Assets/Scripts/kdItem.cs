using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class kdItem : ScriptableObject
{
    public string itemName;
    public ItemType itemType;       // enum ItemType {none, weapon, ammo, consumable }
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string weaponType;

}
