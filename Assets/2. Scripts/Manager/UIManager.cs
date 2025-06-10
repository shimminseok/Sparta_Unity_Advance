using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : SceneOnlySingleton<UIManager>
{
    [SerializeField] private Image fadeImage;
    private readonly List<IUIBase> openedUi = new List<IUIBase>();

    public bool IsOpenUI { get; private set; }
    private Tween fadeTween;

    protected override void Awake()
    {
        base.Awake();
    }

    public void CheckOpenPopup(IUIBase panel)
    {
        if (openedUi.Contains(panel))
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
        openedUi.Add(panel);
        IsOpenUI = true;
    }

    public void ClosePanel(IUIBase panel)
    {
        if (!openedUi.Contains(panel))
            return;
        openedUi.Remove(panel);
        IsOpenUI = false;
    }

    public void AllClosePanel()
    {
        for (int i = openedUi.Count - 1; i >= 0; i--)
        {
            openedUi[i].Close();
        }
    }

    public void FadeIn(float duration = 1f, UnityAction callback = null)
    {
        StartFade(1f, 0f, duration, callback);
    }

    public void FadeOut(float duration = 1f, UnityAction callback = null)
    {
        StartFade(0f, 1f, duration, callback);
    }

    private void StartFade(float from, float to, float duration, UnityAction callback = null)
    {
        fadeTween?.Kill();
        fadeImage.raycastTarget = true;
        Color color = fadeImage.color;
        color.a = from;
        fadeImage.color = color;

        fadeTween = fadeImage.DOFade(to, duration).SetEase(Ease.InCubic).SetUpdate(true).OnComplete(() =>
        {
            fadeImage.raycastTarget = false;
            callback?.Invoke();
        });
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}