using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class ItemService:Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
        }

        public void Init()
        {

        }

        private void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemBuy:character:{0}:Shop{1}ShopItem{2}", character.Id, message.shopId,
                message.shopItemId);
            var result = ShopManager.Instance.BuyItem(sender, message.shopId, message.shopItemId);
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = result;
            sender.SendResponse();
        }
    }
}
