using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private ParticleSystem clickParticles;
    [SerializeField] private GameObject clickParticlesObj;
    [SerializeField] private CameraShake cameraShake;
    private PlayerController playerController;
    private Camera mainCamera;


    public PlayerController PlayerController
    {
        get
        {
            if (playerController == null)
            {
                playerController = FindObjectOfType<PlayerController>();
            }

            return playerController;
        }
    }

    public SaveFile SaveFile { get; private set; }
    private string path;

    protected override void Awake()
    {
        base.Awake();
        path = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    private void Start()
    {
        LoadGame();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayClick();
        }
    }


    public void SetPlayerController(PlayerController player)
    {
        playerController = player;
    }

    private void PlayClick()
    {
        clickParticles.Stop();
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = 5f;
        clickParticlesObj.transform.position = mainCamera.ScreenToWorldPoint(screenPosition);
        clickParticles.Play();
        AudioManager.Instance.PlaySFX(SFXSoundType.Click);
    }

    public void MainCameraShake()
    {
        cameraShake.ShakeCamera(1, 1, 1);
    }

    public void OnSceneChanged()
    {
        playerController = null;
    }

    public void SaveGame()
    {
        if (SaveFile == null)
        {
            SaveFile = new SaveFile(true);
        }
        else
        {
            SaveFile.Gold = AccountManager.Instance.Gold;
            SaveFile.BestStage = AccountManager.Instance.BestStage;
            SaveFile.CurrentStage = StageManager.Instance.CurrentStage;

            SaveFile.InventoryItems.Clear();
            foreach (InventoryItem inventoryItem in InventoryManager.Instance.Inventory)
            {
                if (inventoryItem?.ItemSo == null)
                    continue;
                SaveFile.InventoryItems.Add(new SaveInventoryItem(inventoryItem));
            }
        }

        var sJson = JsonConvert.SerializeObject(SaveFile, Formatting.Indented);
        File.WriteAllText(path, sJson);
    }

    public void LoadGame()
    {
        if (File.Exists(path))
        {
            string decrypted = File.ReadAllText(path);
            SaveFile = JsonConvert.DeserializeObject<SaveFile>(decrypted);

            AccountManager.Instance.AddGold(SaveFile.Gold);
            AccountManager.Instance.UpdateBestStage(SaveFile.BestStage);
            StageManager.Instance.LoadStageData(SaveFile.CurrentStage);
            var ItemTable = TableManager.Instance.GetTable<ItemTable>();
            foreach (SaveInventoryItem item in SaveFile.InventoryItems)
            {
                if (item == null)
                    continue;

                ItemSO itemSo = ItemTable.GetDataByID(item.Id);

                var inven = new InventoryItem();
                if (itemSo is ConsumableItemSO consumable)
                {
                    inven = new InventoryItem(consumable, item.Quantity);
                }
                else if (itemSo is EquipmentItemSO equipmentItem)
                {
                    inven = new EquipmentItem(equipmentItem);
                }

                InventoryManager.Instance.AddItem(inven, inven.Quantity);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}

[Serializable]
public class SaveFile
{
    public List<SaveInventoryItem> InventoryItems = new List<SaveInventoryItem>();
    public Dictionary<EquipmentType, EquipmentItem> EquipmentItems;

    public double Gold;

    public int BestStage;
    public int CurrentStage;

    public SaveFile()
    {
    }

    public SaveFile(bool isSave)
    {
        Gold = AccountManager.Instance.Gold;
        BestStage = AccountManager.Instance.BestStage;
        CurrentStage = StageManager.Instance.CurrentStage;

        foreach (InventoryItem inventoryItem in InventoryManager.Instance.Inventory)
        {
            if (inventoryItem == null)
                return;
            InventoryItems.Add(new SaveInventoryItem(inventoryItem));
        }
    }
}