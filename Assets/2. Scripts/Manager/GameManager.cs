using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController PlayerController { get; private set; }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetPlayerController(PlayerController playerController)
    {
        PlayerController = playerController;
    }

    public void OnSceneChanged()
    {
        PlayerController = null;
    }
}