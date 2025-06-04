using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipPage : MonoBehaviour,Ipage
{
	[SerializeField] private ItemBtn shipBtn;
	[SerializeField] private List<ItemBtn> weaponBtns;

	private Action OpenShipInventory;
	private Action OpenWeaponInventory;
	private Action<ItemBtn> Onfocus;
	private Action<ItemBtn> OnDeselectShip;
	private Action<ItemBtn> OnDeselectWeapon;
	private ShipData ship;
	
	public ItemBtn currentSelectedBtn{get; private set;}
	public void Close(){
        gameObject.SetActive(false);
		shipBtn.JustUnSelect();
		if(currentSelectedBtn != null){
			currentSelectedBtn.JustUnSelect();
			currentSelectedBtn = null;
		}
    }
	public void Open(){
		gameObject.SetActive(true);
	}
	public void Init(ShipData ship,List<ShipWeaponData> weaponDatabase,Action openShipInventory, Action openWeaponInventory,Action<ItemBtn>Onfocus,Action<ItemBtn>OnDeselectShip,Action<ItemBtn>OnDeselectWeapon){
		this.ship = ship;
		this.OpenShipInventory = openShipInventory;
		this.OpenWeaponInventory = openWeaponInventory;
		this.Onfocus = Onfocus;
		this.OnDeselectShip = OnDeselectShip;
		this.OnDeselectWeapon = OnDeselectWeapon;

		shipBtn.Init( OnShipBtnClick, OnShipBtnUnClick, OnShipBtnFocus);

		if(ship.EquippedWeaponID.Count != weaponBtns.Count){
			Debugger.LogError(DebugCategory.UI,"weaponData.Count != weaponBtns.Count");
			return;
		}

		for(int i = 0; i < ship.EquippedWeaponID.Count; i++){
			int pos = ship.EquippedWeaponID[i].Position;
			string id = ship.EquippedWeaponID[i].ID;

			ItemBtn btn = weaponBtns[pos];
			btn.Init(OnWeaponBtnClick, OnWeaponBtnUnClick, OnWeaponBtnFocus);
			ShipWeaponData data = weaponDatabase.Find(x=>x.ID == id);
			btn.SetData(data.KeStr,data.ID);
		}
	}

	private void OnShipBtnClick(ItemBtn itemBtn){
		if(currentSelectedBtn == shipBtn){
			return;
		}
		if(currentSelectedBtn != null){
			currentSelectedBtn.JustUnSelect();
		}
		currentSelectedBtn = itemBtn;
		OpenShipInventory?.Invoke();
	}
	private void OnShipBtnUnClick(ItemBtn itemBtn){
		currentSelectedBtn = null;
		OnDeselectShip?.Invoke(itemBtn);
	}
	private void OnShipBtnFocus(ItemBtn itemBtn){
		Onfocus?.Invoke(itemBtn);
	}

	private void OnWeaponBtnClick(ItemBtn itemBtn){
		if(currentSelectedBtn == itemBtn){
			return;
		}
		if(currentSelectedBtn != null){
			currentSelectedBtn.JustUnSelect();
		}
		currentSelectedBtn = itemBtn;
		OpenWeaponInventory?.Invoke();	
	}
	private void OnWeaponBtnUnClick(ItemBtn itemBtn){
		if(currentSelectedBtn == itemBtn){
			currentSelectedBtn.JustUnSelect();
			currentSelectedBtn = null;
		}
		OnDeselectWeapon?.Invoke(itemBtn);
	}
	private void OnWeaponBtnFocus(ItemBtn itemBtn){
		Onfocus?.Invoke(itemBtn);
	}
	public void ChangeSelectedWeaponContent(string keyString,string id){
		if(id == "empty"){
			return;
		}

		if(currentSelectedBtn.DataWithID == id){
			currentSelectedBtn.SetData("empty","empty");
			// update ship data
			int thePos = weaponBtns.IndexOf(currentSelectedBtn);
			ship.EquippedWeaponID.Remove(ship.EquippedWeaponID.Find(x=>x.ID == id && x.Position == thePos));
			return;
		}

		// update ship data
		ship.EquippedWeaponID.Remove(ship.EquippedWeaponID.Find(x=>x.ID == currentSelectedBtn.DataWithID&&
		x.Position == weaponBtns.IndexOf(currentSelectedBtn)));
		int pos = weaponBtns.IndexOf(currentSelectedBtn);
		ship.EquippedWeaponID.Add(new PositionAndIDMap{ID = id,Position = pos});
		currentSelectedBtn.SetData(keyString,id);
	}
}


// [System.Serializable]
// public class ShipExcelDataBase{
// 	public List<ShipExcelData> shipExcelDataList;
// }

// [System.Serializable]
// public class ShipExcelData{
// 	public string KeyString;
// 	public string Attributes;
// 	public string WeaponHole;
// 	public string UiPrefab;
// }

// [System.Serializable]
// public class WeaponExcelDataBase{
// 	public List<WeaponExcelData> weaponExcelDataList;
// }

// [System.Serializable]
// public class WeaponExcelData{
// 	public string KeyString;
// 	public string Attributes;
// 	public string WeaponHole;
// 	public string UiPrefab;
// }

