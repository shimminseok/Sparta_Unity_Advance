using System;
using UnityEngine;

public class StageManager : SceneOnlySingleton<StageManager>
{
    public StageTable StageTable => TableManager.Instance.GetTable<StageTable>();

    public int  CurrentStage { get; private set; } = 1010101;
    public bool IsStageStart { get; private set; }
    public bool IsWaveClear  { get; private set; }
    public bool IsWaveStart  { get; private set; }
    public int  CurrentWave  { get; private set; } = 1;

    public event Action OnEnterStage;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        EnterStage(CurrentStage);
    }

    public void EnterStage(int stage)
    {
        CurrentWave = 1;
        OnEnterStage?.Invoke();
        GameManager.Instance.PlayerController.Agent.Warp(Vector3.up);
        CurrentStage = stage;
        MapGenerator.Instance.GenerateStage(GetCurrentStage());
        UIManager.Instance.FadeIn(3, () =>
        {
            IsWaveStart = true;
            StartCoroutine(MapGenerator.Instance.SpawnEnemies(GetCurrentStage().MonsterCount));
        });
    }

    public StageSO GetCurrentStage()
    {
        return StageTable.GetDataByID(CurrentStage);
    }

    public void WaveClear()
    {
        IsWaveClear = true;
        IsWaveStart = false;
        CurrentWave++;
        if (CurrentWave > GetCurrentStage().WaveCount)
        {
            StageClear();
        }
        else
        {
            MapGenerator.Instance.NextWave();
        }
    }

    public void SpawnWave()
    {
        IsWaveClear = false;
        IsWaveStart = true;
    }

    private void StageClear()
    {
        AccountManager.Instance.UpdateBestStage(CurrentStage);
        EnterStage(AccountManager.Instance.BestStage);
        RewardManager.Instance.GetReward(GetCurrentStage().Reward);
    }

    public void LoadStageData(int currentStage)
    {
        CurrentStage = currentStage;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}