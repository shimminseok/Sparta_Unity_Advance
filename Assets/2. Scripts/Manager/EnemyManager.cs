using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SceneOnlySingleton<EnemyManager>
{
    public List<EnemyController> Enemies { get; private set; } = new List<EnemyController>();

    private MonsterTable monsterTb;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        monsterTb = TableManager.Instance.GetTable<MonsterTable>();
    }


    public void SpawnEnemy(MonsterSO monsterSo, Vector3 spawnPos)
    {
        GameObject go         = ObjectPoolManager.Instance.GetObject(monsterSo.Prefab.name);
        var        controller = go.GetComponent<EnemyController>();
        controller.SpawnMonster(monsterSo, spawnPos);
        Enemies.Add(controller);
    }

    public void MonsterDead(EnemyController enemy)
    {
        Enemies.Remove(enemy);
        ObjectPoolManager.Instance.ReturnObject(enemy.gameObject);
        if (Enemies.Count == 0)
        {
            StageManager.Instance.WaveClear();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}