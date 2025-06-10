using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour, IPoolObject
{
    [SerializeField] private int poolSize;
    [SerializeField] private string poolID;

    private bool triggered = false;
    public GameObject GameObject => gameObject;
    public string     PoolID     => poolID;
    public int        PoolSize   => poolSize;

    public void InitFromPool()
    {
        transform.position = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (triggered || !other.gameObject.CompareTag("Player"))
            return;

        Debug.Log("Wave Start");
        triggered = true;
        StageManager.Instance.SpawnWave();
    }
}