using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = "ScriptableObject/StageSO", order = 0)]
public class StageSO : ScriptableObject
{
    public int ID;
    public string StageName;
    public int WaveCount;
    public int MonsterCount;
    public RewardSO Reward;
    public List<MonsterSO> Monsters;
}