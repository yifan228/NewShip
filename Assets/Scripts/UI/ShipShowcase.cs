using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ShipShowcase : MonoBehaviour, Ipage    
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private Inventory Shipinventory;
    [SerializeField] private Inventory Weaponinventory;

    [Tooltip("for test")]
    [SerializeField] private PlayerData testPlayerData;
    [SerializeField] private List<GameObject> shipPages;
    private ShipPage currentShipPage;
    private ShipData currentShip;

    private Ipage currentOpenInventory;

    private void Start()
    {
        closeBtn.onClick.AddListener(Close);

        BuildShipPage(testPlayerData.Ships[0]);
    }

    private void BuildShipPage(ShipData ship)
    {
        currentShip = ship;
        string shipKeySting = ship.KeStr;

        List<ShipWeaponData> weaponData = new List<ShipWeaponData>();
        foreach (var weapon in ship.EquippedWeaponID)
        {
            weaponData.Add(testPlayerData.Weapons.Find(x => x.ID == weapon.ID));
        }
        Excel_ShipData target = TheGlobal.Instance.ShipExcelDatabase.ShipDatas.Find(x => x.KeyString == shipKeySting);
        currentShipPage = shipPages.Find(x => x.name == target.UiPrefab).GetComponent<ShipPage>();
        currentShipPage.Init(ship,testPlayerData.Weapons, OpenShipInventory, OpenWeaponInventory, OnItemFocas, OnShipDeselect, OnWeaponInventoryDeselect);
        currentShipPage.Open();
    }

    public void Close(){
        gameObject.SetActive(false);
        Shipinventory.Close();
        Weaponinventory.Close();
    }
    public void Open(){
        gameObject.SetActive(true);
    }

    private void OpenWeaponInventory(){
        if(currentOpenInventory != null){
            currentOpenInventory.Close();
        }
        currentOpenInventory = Weaponinventory;
        Weaponinventory.Init(testPlayerData.Weapons.Select(x=>new IdAndKeyString{ID = x.ID,KeyString = x.KeStr}).ToList(), OnWeaponInventorySelect, OnWeaponInventoryDeselect, OnItemFocas);
        Weaponinventory.Open();
    }
    private void OnWeaponInventorySelect(ItemBtn itemBtn){
        currentShipPage.ChangeSelectedWeaponContent(itemBtn.KeyString,itemBtn.DataWithID);
    }
    private void OnWeaponInventoryDeselect(ItemBtn itemBtn){
        Weaponinventory.Deselect(itemBtn);
        currentShipPage.ChangeSelectedWeaponContent("empty","empty");
    }
    private void OnItemFocas(ItemBtn itemBtn){
        
    }

    private void OpenShipInventory(){
        if(currentOpenInventory != null){
            currentOpenInventory.Close();
        }
        currentOpenInventory = Shipinventory;

        Shipinventory.Init(testPlayerData.Ships.Select(x=>new IdAndKeyString{ID = x.ID,KeyString = x.KeStr}).ToList(), OnShipInventorySelect, OnShipInventoryDeselect, OnItemFocas);
        Shipinventory.Open();
    }
    private void OnShipDeselect(ItemBtn itemBtn){
        Shipinventory.Close();
    }
    private void OnShipInventorySelect(ItemBtn itemBtn){
        ChangeShipPage(itemBtn.DataWithID);
    }
    private void OnShipInventoryDeselect(ItemBtn itemBtn){
        Shipinventory.Deselect(itemBtn);
    }

    private enum SelectedItemType{
        WeaponShowcase,
        WeaponInventory,
        Ship,
        ShipInventory
    }
    private class SelectedItem{
        public SelectedItemType type;
        public ItemBtn itemBtn;
    }

    private void ChangeShipPage(string id){
        ShipData shipData = testPlayerData.Ships.Find(x=>x.ID == id);
        if(shipData == null){
            Debugger.LogError(DebugCategory.UI,"ShipData not found");
            return;
        }
        if(currentShipPage != null){
            currentShipPage.Close();
        }
        BuildShipPage(shipData);
    }
}
