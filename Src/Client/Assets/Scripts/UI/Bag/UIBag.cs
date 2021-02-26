using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindow
{
    public Text Money;
    public RectTransform[] Pages;

    public GameObject BagItem;

    private List<Image> slots;

	// Use this for initialization
	void Start () {
        if (slots == null)
        {
            slots = new List<Image>();
            for (int i = 0; i < this.Pages.Length; i++)
            {
                slots.AddRange(this.Pages[i].GetComponentsInChildren<Image>(true));
            }
        }

        StartCoroutine(InitBag());
    }

    IEnumerator InitBag()
    {
        for (int i = 0; i < BagManager.Instance.Items.Length; i++)
        {
            var item = BagManager.Instance.Items[i];
            if (item.ItemId > 0)
            {
                GameObject go = Instantiate(BagItem, slots[i].transform);
                var ui = go.GetComponent<UIIconItem>();
                var def = ItemManager.Instance.Items[item.ItemId].Define;
                ui.SetMainIcon(def.Icon,item.Count.ToString());
            }
        }

        for (int i = BagManager.Instance.Items.Length; i < slots.Count; i++)
        {
            slots[i].color = Color.gray;
        }

        yield return null;
    }

    public void SetTitle(string title)
    {
        this.Money.text = User.Instance.CurrentCharacter.Id.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnReset()
    {
        BagManager.Instance.Reset();
    }
}
