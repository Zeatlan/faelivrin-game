using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.SO
{
    [CreateAssetMenu(fileName = "BattleMap", menuName = "Game/Battle/Map")]
    public class BattleMapSO : ScriptableObject
    {
        public List<Vector3Int> playerPossiblePos;
        public List<Vector3Int> ennemiesSpawnPos;
        public List<CharacterInfo> ennemies;
    }
}
