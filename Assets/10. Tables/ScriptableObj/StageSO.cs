using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = "ScriptableObject/StageSO", order = 0)]
public class StageSO : ScriptableObject
{
    public int ID;
    public int WaveCount;
    public RewardSO Reward;
}