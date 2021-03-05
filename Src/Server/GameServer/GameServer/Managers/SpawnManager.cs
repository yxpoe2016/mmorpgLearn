using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Models;

namespace GameServer.Managers
{
    
    class SpawnManager
    {
        private List<Spawner> Rules = new List<Spawner>();
        private Map Map;
        internal void Init(Map map)
        {
            this.Map = map;
            if (DataManager.Instance.SpawnRules.ContainsKey(map.ID))
            {
                foreach (var define in DataManager.Instance.SpawnRules[map.ID].Values)
                {
                    this.Rules.Add(new Spawner(define,this.Map));
                }
            }
        }

        internal void Update()
        {
           if(Rules.Count == 0)
               return;
           for (int i = 0; i < this.Rules.Count; i++)
           {
               this.Rules[i].Update();
           }
        }
    }
}
