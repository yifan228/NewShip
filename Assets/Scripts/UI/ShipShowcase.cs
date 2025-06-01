using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ShipShowcase : MonoBehaviour, Ipage    
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private ItemBtn Ship;
    [SerializeField] private Inventory Shipinventory;
    [SerializeField] private Inventory Weaponinventory;
    [SerializeField] private Inventory WeaponShowcase;
    [SerializeField] private List<string> weaponSpriteNames;
    [SerializeField] private List<string> shipIds;

    private Ipage currentOpenInventory;
    private List<SelectedItem> selectedItems = new List<SelectedItem>();

    private void Start(){
        closeBtn.onClick.AddListener(Close);
        WeaponShowcase.Init(weaponSpriteNames, OnWeaponShowcaseSelect, OnWeaponShowcaseDeselect, OnItemFocas);
        Ship.Init( OnShipSelect, OnShipDeselect, OnItemFocas);
        Ship.SetData(shipIds[0]);
    }   

    public void Close(){
        gameObject.SetActive(false);
        Shipinventory.Close();
        Weaponinventory.Close();
        WeaponShowcase.Close();
    }
    public void Open(){
        gameObject.SetActive(true);
        WeaponShowcase.Open();
    }

    private void OnWeaponShowcaseSelect(ItemBtn itemBtn){
        if(currentOpenInventory != null){
            currentOpenInventory.Close();
        }
        currentOpenInventory = Weaponinventory;

        selectedItems.Where(x=>x.type == SelectedItemType.Ship).ToList().ForEach(x=>x.itemBtn.UnSelect());
        selectedItems.Where(x=>x.type == SelectedItemType.ShipInventory).ToList().ForEach(x=>x.itemBtn.UnSelect());

        Weaponinventory.Init(weaponSpriteNames, 
        OnWeaponInventorySelect,
        OnWeaponInventoryDeselect,
        OnItemFocas);
        Weaponinventory.Open();
        selectedItems.Add(new SelectedItem{type = SelectedItemType.WeaponShowcase, itemBtn = itemBtn});
    }
    private void OnWeaponShowcaseDeselect(ItemBtn itemBtn){
        Weaponinventory.Close();
        WeaponShowcase.Deselect(itemBtn);
        currentOpenInventory = null;
        selectedItems.Remove(selectedItems.Find(x=>x.type == SelectedItemType.WeaponShowcase && x.itemBtn == itemBtn));
    }

    private void OnWeaponInventorySelect(ItemBtn itemBtn){
        WeaponShowcase.ChangeSelectedContent(itemBtn);
    }
    private void OnWeaponInventoryDeselect(ItemBtn itemBtn){
        Weaponinventory.Deselect(itemBtn);
        selectedItems.Remove(selectedItems.Find(x=>x.type == SelectedItemType.WeaponInventory && x.itemBtn == itemBtn));
    }
    private void OnItemFocas(ItemBtn itemBtn){
        
    }

    private void OnShipSelect(ItemBtn itemBtn){
        if(currentOpenInventory != null){
            currentOpenInventory.Close();
        }
        currentOpenInventory = Shipinventory;

        selectedItems.Where(x=>x.type == SelectedItemType.WeaponShowcase).ToList().ForEach(x=>x.itemBtn.UnSelect());
        selectedItems.Where(x=>x.type == SelectedItemType.WeaponInventory).ToList().ForEach(x=>x.itemBtn.UnSelect());

        Shipinventory.Init(shipIds, OnShipInventorySelect, OnShipInventoryDeselect, OnItemFocas);
        Shipinventory.Open();
        selectedItems.Add(new SelectedItem{type = SelectedItemType.Ship, itemBtn = itemBtn});
    }
    private void OnShipDeselect(ItemBtn itemBtn){
        Shipinventory.Close();
        Ship.JustUnSelect();
        currentOpenInventory = null;
        selectedItems.Remove(selectedItems.Find(x=>x.type == SelectedItemType.Ship && x.itemBtn == itemBtn));
    }
    private void OnShipInventorySelect(ItemBtn itemBtn){
        Ship.SetData(itemBtn.DataWithID);
        selectedItems.Add(new SelectedItem{type = SelectedItemType.ShipInventory, itemBtn = itemBtn});
    }
    private void OnShipInventoryDeselect(ItemBtn itemBtn){
        Shipinventory.Deselect(itemBtn);
        selectedItems.Remove(selectedItems.Find(x=>x.type == SelectedItemType.ShipInventory && x.itemBtn == itemBtn));
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
}
