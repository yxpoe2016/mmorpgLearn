using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{

    public Text title;
    public Text money;
    public GameObject shopItem;
    private ShopDefine shop;
    public Transform[] itemRoot;
    private UIShopItem selectItem;

    void Start()
    {
        StartCoroutine(InitItems());
    }

    IEnumerator InitItems()
    {
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status > 0)
            {
                GameObject go = Instantiate(shopItem, itemRoot[0]);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(kv.Key, kv.Value, this);
            }
        }

        yield return null;
    }

    public void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    public void SelectShopItem(UIShopItem item)
    {
        Debug.Log(item.title.text);
        if (selectItem != null)
            selectItem.Selected = false;
        selectItem = item;

        OnClickBuy();
    }

    public void OnClickBuy()
    {
        if (this.selectItem == null)
        {
            MessageBox.Show("请选择需要购买的道具", "提示购买");
            return;
        }

        if (!ShopManager.Instance.BuyItem(this.shop.ID, this.selectItem.ShopItemID))
        {

        }
    }
}
