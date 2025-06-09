using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private PlayerController playerController;

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

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetPlayerController(PlayerController player)
    {
        this.playerController = player;
    }

    public void OnSceneChanged()
    {
        playerController = null;
    }
}