using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Managers
{
    class Spawner
    {
        private SpawnRuleDefine define;
        private Map map;
        private float spawnTime = 0;
        private float unspawnTime = 0;
        private bool spawned = false;
        private SpawnPointDefine spawnPoint = null;
        public Spawner(SpawnRuleDefine define, Map map)
        {
            this.define = define;
            this.map = map;
            if (DataManager.Instance.SpawnPoints.ContainsKey(this.map.ID))
            {
                if (DataManager.Instance.SpawnPoints[this.map.ID].ContainsKey(this.define.SpawnPoint))
                {
                    spawnPoint = DataManager.Instance.SpawnPoints[this.map.ID][this.define.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoint[{1}] not existed",this.define.ID,this.define.SpawnPoint);
                }
            }
        }

        internal void Update()
        {
            if (this.CanSpawn())
            {
                this.Spawn();
            }
        }

        private void Spawn()
        {
            this.spawned = true;
            Log.InfoFormat("Map{0} Spawn[{1}]Mon{2},Lv{3} At Point{4}",this.define.MapID,this.define.ID,this.define.SpawnMonID,this.define.SpawnLevel, this.spawnPoint.Position);
            this.map.MonsterManager.Create(this.define.SpawnMonID, this.define.SpawnLevel, this.spawnPoint.Position,
                this.spawnPoint.Direction);
        }

        private bool CanSpawn()
        {
            if (this.spawned)
                return false;
            if (this.unspawnTime + this.define.SpawnPeriod > Time.time)
                return false;
            return true;
        }
    }
}
