
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.U2D;
using System;
public class ItemBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image focusImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private SpriteAtlas itemAtlas;
    private Action<ItemBtn> onSelect;
    private Action<ItemBtn> onUnSelect;
    private Action<ItemBtn> onFocus;
    public string DataWithID{get; private set; }
    public string KeyString{get; private set;}
    private bool canSelect = true;

    public int Index{get; private set;}
    public void SetIndex(int index){
        Index = index;
    }
    public void Init( Action<ItemBtn> onSelect, Action<ItemBtn> onUnSelect,Action<ItemBtn> onFocus)
    {
        this.onSelect = onSelect;
        this.onUnSelect = onUnSelect;
        this.onFocus = onFocus;
    }

    public void SetData(string keyString,string id)
    {
        string imageName = "empty";
        if(keyString == "empty"){
            canSelect = false;
        }else{
            canSelect = true;
            imageName = TheGlobal.Instance.keyStrImageDatas.GetImageName(keyString);
            if(imageName == "empty"){
                Debugger.LogError(DebugCategory.UI,"Image not found: " + keyString);
            }
        }
        Sprite sprite = itemAtlas.GetSprite(imageName);
        itemImage.sprite = sprite;
        DataWithID = id;
        KeyString = keyString;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        focusImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        focusImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!canSelect){
            return;
        }
        if(selectImage.gameObject.activeSelf){
            UnSelect();
        }else{
            Select();
        }
    }

    private void Select(){
        selectImage.gameObject.SetActive(true);
        onSelect.Invoke(this);
    }

    public void UnSelect(){
        selectImage.gameObject.SetActive(false);
        onUnSelect.Invoke(this);
    }

    public void JustUnSelect(){
        selectImage.gameObject.SetActive(false);
    }
}
