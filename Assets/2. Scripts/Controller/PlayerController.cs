using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
public class PlayerController : SceneOnlySingleton<PlayerController>
{
    public StatManager StatManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StatManager = GetComponent<StatManager>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}