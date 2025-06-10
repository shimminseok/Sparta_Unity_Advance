using UnityEngine;

public class StageManager : SceneOnlySingleton<StageManager>
{
    public StageTable StageTable => TableManager.Instance.GetTable<StageTable>();

    public int  CurrentStage { get; private set; } = 1010101;
    public bool IsStageStart { get; private set; }
    public bool IsWaveClear  { get; private set; }
    public bool IsWaveStart  { get; private set; }
    public int  CurrentWave  { get; private set; } = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    public void EnterStage(int stage)
    {
        CurrentStage = stage;
        StageSO stageSo = StageTable.GetDataByID(CurrentStage);
        MapGenerator.Instance.GenerateStage();
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
        Debug.Log($"Stage Clear : Best Stage{AccountManager.Instance.BestStage}, Current Stage{CurrentStage}");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}