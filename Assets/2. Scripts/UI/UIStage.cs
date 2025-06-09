using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStage : UIBase<UIStage>, IUIBase
{
    [SerializeField] private GameObject StageListSlotPrefab;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void Open()
    {
        base.Open();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}