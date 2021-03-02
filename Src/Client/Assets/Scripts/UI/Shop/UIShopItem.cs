using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour,ISelectHandler
{
    private UIShop shop;

    public int ShopItemID;

    private ShopItemDefine ShopItem;

    private ItemDefine item;

    public Text title;

    public Text count;

    public Text price;
    public Text limitClass;
    public Image icon;

    private bool selected;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
    }



    // Use this for initialization
	void Start () {
		
	}

    public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {
        this.shop = owner;
        this.ShopItemID = id;
        this.ShopItem = shopItem;
        this.item = DataManager.Instance.Items[this.ShopItem.ItemID];

        this.title.text = this.item.Name;
        this.count.text = "x"+this.ShopItem.Count.ToString();
        this.price.text = this.ShopItem.Price.ToString();
        // this.limitClass.text = this.item.LimitClass.ToString();
        this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Icon);

    }

    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);
    }
}
