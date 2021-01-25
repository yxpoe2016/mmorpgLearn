﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class CharacterManager:Singleton<CharacterManager>
    {
        public Dictionary<int,Character> Characters = new Dictionary<int, Character>();
        public CharacterManager()
        {

        }

        public void Dispose()
        {

        }

        public void Init()
        {

        }

        public void Clear()
        {

        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player,cha);
            this.Characters[cha.ID] = character;
            return character;
        }
    }
}
