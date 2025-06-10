using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStage : UIBase<UIStage>, IUIBase
{
    [SerializeField] private GameObject StageListSlotPrefab;
    [SerializeField] private Transform StageListSlotsParent;

    private List<StageListSlot> stageListSlots = new List<StageListSlot>();

    public StageManager StageManager => StageManager.Instance;

    private StageListSlot selectedStage;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        stageListSlots = new List<StageListSlot>(StageManager.StageTable.DataDic.Count);
        foreach (var stageTableData in StageManager.StageTable.DataDic)
        {
            GameObject    slot          = Instantiate(StageListSlotPrefab, StageListSlotsParent);
            StageListSlot stageListSlot = slot.GetComponent<StageListSlot>();
            stageListSlot.SetSlot(stageTableData.Value);
            stageListSlots.Add(stageListSlot);
        }
    }

    public void SelectedStage(StageListSlot slot)
    {
        if (selectedStage != null && this.selectedStage != slot)
        {
            selectedStage.DeSelect();
        }

        selectedStage = slot;
        selectedStage.Select();
    }

    public void OnClickEnterStageBtn()
    {
        StageManager.EnterStage(selectedStage.StageSo.ID);
    }

    public override void Open()
    {
        base.Open();
        foreach (StageListSlot stageListSlot in stageListSlots)
        {
            stageListSlot.Refresh();
        }
    }

    public override void Close()
    {
        base.Close();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}