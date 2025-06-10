using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


public class StageListSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private GameObject lockImg;
    [SerializeField] private GameObject selectedImg;

    private StageSO stageSo;

    public void SetSlot(StageSO stageData)
    {
        stageSo = stageData;
        stageName.text = stageData.StageName;
        lockImg.SetActive(stageSo.ID > AccountManager.Instance.BestStage);
    }

    public void Refresh()
    {
        lockImg.SetActive(stageSo.ID > AccountManager.Instance.BestStage);
    }

    public void DeSelect()
    {
        selectedImg.SetActive(false);
    }

    public void Select()
    {
        selectedImg.SetActive(true);
    }

    public void OnClickStageSlot()
    {
        UIStage.Instance.SelectedStage(this);
    }
}