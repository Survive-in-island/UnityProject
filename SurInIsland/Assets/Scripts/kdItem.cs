using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kdItem : ScriptableObject
{
    public string itemName;
    public ItemType itemType;       // enum ItemType {none, weapon, ammo, consumable }
    public Sprite itemImage;
    public GameObject itemPrefab;

}
