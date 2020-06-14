using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTreeFPS
{
    public class WeaponSlotInventory : MonoBehaviour
    {
        public Text weaponName;
        public Text ammoCount;
        
        public int slotIndex;

        public Button dropButton;

        private Image image;

        private WeaponManager weaponManager;

        public MissionUI mission;

        void Start()
        {
            image = GetComponent<Image>();
            weaponManager = FindObjectOfType<WeaponManager>();

            //mission = GetComponent<MissionUI>();
            mission = FindObjectOfType<MissionUI>();
        }
        
        void Update()
        {
            if(weaponManager.slots[slotIndex].storedWeapon != null)
            {
                image.sprite = weaponManager.slots[slotIndex].storedWeapon.weaponSetting.weaponIcon;
                ammoCount.text = weaponManager.slots[slotIndex].storedWeapon.currentAmmo.ToString();
                weaponName.text = weaponManager.slots[slotIndex].storedWeapon.weaponName;

                image.color = Color.white;

                dropButton.gameObject.SetActive(true);

                if (weaponName.text == "SCAR")
                    mission.step1_Survive = true;

                // mission.Test();
            }
            else
            {
                image.sprite = null;
                ammoCount.text = "";
                weaponName.text = "";

                image.color = Color.clear;

                dropButton.gameObject.SetActive(false);
            }
        }
    }
}