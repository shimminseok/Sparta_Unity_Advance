using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : SceneOnlySingleton<MapGenerator>
{
    private readonly Dictionary<Direction, Vector3> dirVectors = new Dictionary<Direction, Vector3>()
    {
        { Direction.Forward, Vector3.forward },
        { Direction.Backward, Vector3.back },
        { Direction.Left, Vector3.left },
        { Direction.Right, Vector3.right }
    };

    [Header("Spawn Area")]
    [SerializeField] private GameObject planePrefab;

    [SerializeField] private float minDistance;
    [SerializeField] private int maxAttempts;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> obstaclePrefabs;

    [Header("Spawn Count")]
    [SerializeField] private int obstacleCount;

    [Header("Chunk Spacing")]
    [SerializeField] private NavMeshSurface mapChunkParent;

    [SerializeField] private float chunkSpacing;

    private List<Vector3> usedPositions = new();
    private Transform currentChunkPlane;
    private Direction currentDir = Direction.Forward;
    private Vector3 lastChunkPosition;

    private StageManager StageManager => StageManager.Instance;
    private StageSO currentStage;

    private Queue<GameObject> spawnedMaps = new Queue<GameObject>();
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    private void Start()
    {
        StageManager.Instance.OnEnterStage += ResetMap;
    }

    public void ResetMap()
    {
        while (spawnedMaps.Count > 0)
        {
            ObjectPoolManager.Instance.ReturnObject(spawnedMaps.Dequeue());
        }

        lastChunkPosition = Vector3.zero;
        currentDir = Direction.Forward;
        spawnedObstacles.ForEach(Destroy);
        spawnedMaps.Clear();
    }

    public void GenerateStage(StageSO stage)
    {
        usedPositions.Clear();
        GameObject map = ObjectPoolManager.Instance.GetObject("Map");
        spawnedMaps.Enqueue(map);
        currentChunkPlane = map.transform;
        currentChunkPlane.SetParent(mapChunkParent.transform);
        currentChunkPlane.position = lastChunkPosition;
        currentChunkPlane.localScale = Vector3.one * 5f;
        currentStage = stage;
        SpawnObjects(obstaclePrefabs, obstacleCount);
        if (spawnedMaps.Count > 3)
        {
            ObjectPoolManager.Instance.ReturnObject(spawnedMaps.Dequeue());
        }

        mapChunkParent.BuildNavMesh();
    }

    public void NextWave()
    {
        currentDir = GetNextDirection();
        Vector3 offset       = dirVectors[currentDir] * chunkSpacing;
        Vector3 nextChunkPos = lastChunkPosition + offset;

        lastChunkPosition = nextChunkPos;
        transform.position = nextChunkPos;


        //여길 바꿔줘야함.
        NavMeshAgent agent = GameManager.Instance.PlayerController.Agent;
        if (agent != null)
        {
            Vector3 targetPos = nextChunkPos - dirVectors[currentDir] * 5f;
            agent.SetDestination(targetPos);
        }

        GenerateStage(currentStage);
        StartCoroutine(SpawnEnemies(currentStage.MonsterCount));
    }

    private void SpawnObjects(List<GameObject> prefabList, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetValidPosition();
            if (spawnPos == Vector3.positiveInfinity)
                continue;

            GameObject prefab = prefabList[Random.Range(0, prefabList.Count)];
            spawnedObstacles.Add(Instantiate(prefab, spawnPos, Quaternion.identity, transform));
            usedPositions.Add(spawnPos);
        }
    }

    public IEnumerator SpawnEnemies(int count)
    {
        yield return new WaitUntil(() => StageManager.Instance.IsWaveStart);
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetValidPosition();
            if (spawnPos == Vector3.positiveInfinity)
                continue;

            MonsterSO monsterSo = currentStage.Monsters[Random.Range(0, currentStage.Monsters.Count)];
            EnemyManager.Instance.SpawnEnemy(monsterSo, spawnPos);

            usedPositions.Add(spawnPos);
        }
    }


    private Vector3 GetValidPosition()
    {
        Vector3 center = currentChunkPlane.position;
        Vector3 size   = currentChunkPlane.localScale * 10f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 candidate = center + new Vector3(Random.Range(-size.x / 2f, size.x / 2f), 1f, Random.Range(-size.z / 2f, size.z / 2f));
            bool    tooClose  = false;
            foreach (var pos in usedPositions)
            {
                if (Vector3.Distance(pos, candidate) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return candidate;
        }

        return Vector3.positiveInfinity;
    }

    private Direction GetNextDirection()
    {
        Direction       opposite   = GetOpposite(currentDir);
        List<Direction> candidates = new() { Direction.Forward, Direction.Left, Direction.Right, Direction.Backward };
        candidates.Remove(opposite); // 지나온 방향은 제외

        return candidates[Random.Range(0, candidates.Count)];
    }


    private Direction GetOpposite(Direction dir)
    {
        return dir switch
        {
            Direction.Forward  => Direction.Backward,
            Direction.Backward => Direction.Forward,
            Direction.Left     => Direction.Right,
            Direction.Right    => Direction.Left,
            _                  => Direction.Backward
        };
    }
}