using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private List<IUIBase> openedUI = new List<IUIBase>();

    public bool IsOpenUI { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void CheckOpenPopup(IUIBase panel)
    {
        if (openedUI.Contains(panel))
        {
            panel.Close();
        }
        else
        {
            panel.Open();
        }
    }

    public void OpenPanel(IUIBase panel)
    {
        openedUI.Add(panel);
        IsOpenUI = true;
    }

    public void ClosePanel(IUIBase panel)
    {
        if (!openedUI.Contains(panel))
            return;
        openedUI.Remove(panel);
        IsOpenUI = false;
    }

    public void AllClosePanel()
    {
        for (int i = openedUI.Count - 1; i >= 0; i--)
        {
            openedUI[i].Close();
        }
    }
}