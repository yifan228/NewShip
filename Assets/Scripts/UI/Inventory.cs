using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour,Ipage
{
    [SerializeField] private List<ItemBtn> itemBtns;
    [SerializeField] private Button nextPageBtn;
    [SerializeField] private Button prevPageBtn;
    [SerializeField] private TMP_Text pageText;
    private ItemBtn selectedItem;

    private int currentPage = 0;
    private int totalPage = 0;
    private List<IdAndKeyString> datas;

    private Action<ItemBtn> onSelect;
    private Action<ItemBtn> onUnSelect;
    private Action<ItemBtn> onFocus;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Debug.Log("Inventory Start");
        if(nextPageBtn != null){
            nextPageBtn.onClick.AddListener(NextPage);
        }
        if(prevPageBtn != null){
            prevPageBtn.onClick.AddListener(PrevPage);
        }
    }

    public void Init(List<IdAndKeyString> datas, Action<ItemBtn> onSelect, Action<ItemBtn> onUnSelect, Action<ItemBtn> onFocus){
        this.datas = datas;
        this.onSelect = onSelect;
        this.onUnSelect = onUnSelect;
        this.onFocus = onFocus;
        for(int i = 0; i < itemBtns.Count; i++){
            itemBtns[i].Init(OnSelect, OnUnSelect, onFocus);
        }
        totalPage = CalculatePage(datas.Count);
        currentPage = 0;
        SetPage(currentPage);
    }

    private void OnSelect(ItemBtn itemBtn){
        if(selectedItem != itemBtn){
            if(selectedItem != null){
                selectedItem.UnSelect();
            }
            selectedItem = itemBtn;
            onSelect.Invoke(itemBtn);
        }
    }

    private void OnUnSelect(ItemBtn itemBtn){
        selectedItem = null;
        onUnSelect.Invoke(itemBtn);
    }

    private int CalculatePage(int dataAmount){
        int page = 0;
        page = dataAmount/itemBtns.Count;
        if(dataAmount % itemBtns.Count != 0){
            page++;
        }
        return page;
    }
    private void SetPage(int page){
        Debug.Log($"SetPage: {page}");
        currentPage = page;
        int startIndex = currentPage * itemBtns.Count;
        int endIndex = startIndex + itemBtns.Count;
        if(endIndex > datas.Count){
            endIndex = datas.Count;
        }
        for(int i = 0; i < itemBtns.Count; i++){
            if(startIndex + i < datas.Count){
                itemBtns[i].SetData(datas[startIndex + i].KeyString,datas[startIndex + i].ID);
            }else{
                itemBtns[i].SetData("empty","empty");
            }
        }
        if(pageText != null){
            pageText.text = $"{currentPage + 1}/{totalPage}";
        }
    }

    private void NextPage(){
        if(currentPage < totalPage - 1){
            currentPage++;
            SetPage(currentPage);
        }
    }
    private void PrevPage(){
        currentPage--;
        SetPage(currentPage);
    }
    public void ChangeSelectedContent(ItemBtn itemBtn)
    {
        Debug.Log($"ChangeSelectedContent: {itemBtn.DataWithID}");
        selectedItem.SetData(itemBtn.KeyString,itemBtn.DataWithID);
    }
    public void Deselect(ItemBtn itemBtn){
        if(selectedItem != null && selectedItem == itemBtn){
            selectedItem.SetData("empty","empty");
            selectedItem = null;
        }
    }
    public void Deselect(){
        if(selectedItem != null){
            selectedItem.SetData("empty","empty");
            selectedItem = null;
        }
    }
    public void Close(){
        gameObject.SetActive(false);
    }
    public void Open(){
        gameObject.SetActive(true);
    }
}
