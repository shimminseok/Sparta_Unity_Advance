using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHeader : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform targetTrans;

    [SerializeField] private Button closeButton;
    private Vector2 beginPoint;
    private Vector2 moveBegin;


    private IUIBase targetPanel;

    private void Awake()
    {
        targetTrans = transform.parent;
    }

    private void Start()
    {
        IUIBase uiBase = transform.root.GetComponent<IUIBase>();
        closeButton.onClick.AddListener(uiBase.Close);
    }

    public void OnDrag(PointerEventData eventData)
    {
        targetTrans.position = beginPoint + (eventData.position - moveBegin);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        beginPoint = targetTrans.position;
        moveBegin = eventData.position;
    }

    public void OnClickCloseBtn()
    {
        if (targetPanel == null)
            targetPanel = GetComponentInParent<IUIBase>();

        targetPanel?.Close();
    }
}