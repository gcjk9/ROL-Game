using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace GameServer.Game
{
    public enum MonsterTypes
    {

    }
    class Monster
    {
        public int id;
        public bool isLive = true;
        public float HP = 100;
        public Vector3 position = new Vector3();
        public List<Vector3> positionSample = new List<Vector3>();

        public Monster(string id)
        {
            int.TryParse(id, out this.id);
        }
        public void SetHP(string HP)
        {
            float.TryParse(HP, out this.HP);
        }
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
        public void Sync(List<Player> players)
        {
            foreach(Player player in players)
            {
                if (player != null&& player.isLive)
                {
                    if(player.localMonsters[id] != null && isLive)
                    {
                        
                    }
                }
            }
        }
        public static Monster FindMonsterById(List<Monster> monsters, int id)
        {
            foreach (Monster monster in monsters)
            {
                if (id == monster.id)
                {
                    return monster;
                }
            }
            return null;
        }
    }
}
