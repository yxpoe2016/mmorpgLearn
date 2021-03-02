using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Managers
{
    class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHandle();

        public event OnEquipChangeHandle OnEquipChanged;
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];
        private byte[] Data;

        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            this.ParseEquipData(data);
        }

        public bool Contains(int equipId)
        {
            for (int i = 0; i < this.Equips.Length; i++)
            {
                if (this.Equips[i] != null && this.Equips[i].Id == equipId)
                    return true;
            }

            return false;
        }

        public Item GetEquip(EquipSlot slot)
        {
            return this.Equips[(int) slot];
        }

        unsafe void ParseEquipData(byte[] data)
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < this.Equips.Length; i++)
                {
                    int itemId = *(int*) (pt + i * sizeof(int));
                    if (itemId > 0)
                        Equips[i] = ItemManager.Instance.Items[itemId];
                    else
                        Equips[i] = null;
                }
            }
        }

        unsafe public byte[] GetEquipData()
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemid = (int*) (pt + i * sizeof(int));
                    if (Equips[i] == null)
                        *itemid = 0;
                    else
                        *itemid = Equips[i].Id;
                }
            }

            return this.Data;
        }

        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }

        public void OnEquipItem(Item pendingEquip)
        {
            if(this.Equips[(int)pendingEquip.EquipInfo.slot]!=null&&this.Equips[(int)pendingEquip.EquipInfo.slot].Id ==pendingEquip.Id)
                return;
            this.Equips[(int) pendingEquip.EquipInfo.slot] = ItemManager.Instance.Items[pendingEquip.Id];
            if (OnEquipChanged != null)
                OnEquipChanged();
        }

        internal void OnUnEquipItem(EquipSlot slot)
        {
            if (this.Equips[(int) slot] != null)
            {
                this.Equips[(int) slot] = null;
                if (OnEquipChanged != null)
                    OnEquipChanged();
            }
        }
    }
}
