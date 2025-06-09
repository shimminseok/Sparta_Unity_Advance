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
        SpawnEnemy(5);
    }


    public void SpawnEnemy(int enemyCnt)
    {
        for (int i = 0; i < enemyCnt; i++)
        {
            var        randomCircle = Random.insideUnitCircle * 15f;
            var        spawnPos     = new Vector3(randomCircle.x, 1f, randomCircle.y);
            MonsterSO  monsterSo    = monsterTb.GetDataByID(Random.Range(1, monsterTb.dataDic.Count + 1));
            GameObject go           = Instantiate(monsterSo.Prefab, spawnPos, Quaternion.identity);
            var        controller   = go.GetComponent<EnemyController>();
            controller.StatManager.Initialize(monsterSo);
            Enemies.Add(controller);
        }
    }

    public void MonsterDead(EnemyController enemy)
    {
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        if (Enemies.Count == 0)
        {
            //다음 스테이지
            SpawnEnemy(5);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}