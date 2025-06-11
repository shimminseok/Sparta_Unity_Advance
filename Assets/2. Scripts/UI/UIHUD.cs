using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image mpBar;
    [SerializeField] private Image expBar;


    [SerializeField] private TextMeshProUGUI currentGoldTxt;
    [SerializeField] private TextMeshProUGUI currentStageTxt;
    [SerializeField] private TextMeshProUGUI currentWaveProcessTxt;
    private StatManager playerStatManager;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerStatManager = gameManager.PlayerController.StatManager;
        playerStatManager.Stats[StatType.CurHp].OnValueChanged += UpdateHpUIWrapper;
        playerStatManager.Stats[StatType.CurMp].OnValueChanged += UpdateMpUIWrapper;

        AccountManager.Instance.OnGoldChanged += UpdateGoldUI;

        StageManager.Instance.OnEnterStage += UpdateStageUIWrapper;
        StageManager.Instance.OnWaveClear += UpdateWaveUIWrapper;
        UpdateGoldUI(AccountManager.Instance.Gold);
        UpdateStageUIWrapper();
        UpdateWaveUIWrapper();
    }

    private void UpdateHpUI(float cur, float max)
    {
        hpBar.fillAmount = cur / max;
    }

    private void UpdateHpUIWrapper(float cur)
    {
        UpdateHpUI(cur, playerStatManager.GetValue(StatType.MaxHp));
    }

    private void UpdateMpUI(float cur, float max)
    {
        mpBar.fillAmount = cur / max;
    }

    private void UpdateMpUIWrapper(float cur)
    {
        UpdateMpUI(cur, playerStatManager.GetValue(StatType.MaxMp));
    }

    private void UpdateGoldUI(double cur)
    {
        currentGoldTxt.text = Utility.ToCurrencyString(cur);
    }

    private void UpdateWaveUIWrapper()
    {
        UpdateWaveProcessUI(StageManager.Instance.CurrentWave, StageManager.Instance.GetCurrentStage().WaveCount);
    }

    private void UpdateWaveProcessUI(float cur, float last)
    {
        currentWaveProcessTxt.text = $"{cur} / {last}";
    }

    private void UpdateStageUIWrapper()
    {
        var stage = StageManager.Instance.GetCurrentStage();
        UpdateStageUI(stage.StageName);
        UpdateWaveUIWrapper();
    }

    private void UpdateStageUI(string stageName)
    {
        currentStageTxt.text = stageName;
    }

    private void OnDisable()
    {
        playerStatManager.Stats[StatType.CurHp].OnValueChanged -= UpdateHpUIWrapper;
        playerStatManager.Stats[StatType.CurMp].OnValueChanged -= UpdateMpUIWrapper;
    }
}