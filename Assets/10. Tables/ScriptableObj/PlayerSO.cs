using System.Collections.Generic;
using UnityEngine;

namespace _10._Tables.ScriptableObj
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/Player", order = 0)]
    public class PlayerSO : ScriptableObject
    {
        public int ID;
        public List<StatData> PlayerStats;
    }
}