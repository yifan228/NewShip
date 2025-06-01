
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
    private bool canSelect = true;

    public void Init( Action<ItemBtn> onSelect, Action<ItemBtn> onUnSelect,Action<ItemBtn> onFocus)
    {
        this.onSelect = onSelect;
        this.onUnSelect = onUnSelect;
        this.onFocus = onFocus;
    }

    public void SetData(string spriteName)
    {
        if(spriteName == "empty"){
            canSelect = false;
        }else{
            canSelect = true;
        }
        Sprite sprite = itemAtlas.GetSprite(spriteName);
        if(sprite == null){
            Debug.LogError("Sprite not found: " + spriteName);
        }
        itemImage.sprite = sprite;
        DataWithID = spriteName;
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
