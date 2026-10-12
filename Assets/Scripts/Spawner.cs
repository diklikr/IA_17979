using UnityEngine;

public class Spawner : MonoBehaviour
{

public GameObject prefab;           // what to spawn
    public Transform spawnPosition;     // where to spawn it


public bool autoSpawn = true;
public float spawnInterval = 10f;
public int maxAlive = 3;            // 0 = unlimited

public bool useSpawnRotation = true;
public Transform parent;

float timer;
int aliveCount;

void Update()
{
    if (!autoSpawn) return;

    timer += Time.deltaTime;
    if (timer >= spawnInterval)
    {
        timer = 0f;
        Spawn();
    }
}

public GameObject Spawn()
{
if (prefab == null)
{
Debug.LogWarning("Spawner: no prefab assigned.", this);
return null;
}

if (maxAlive > 0 && aliveCount >= maxAlive) return null;

Transform point = spawnPosition != null ? spawnPosition : transform;
Quaternion rot = useSpawnRotation ? point.rotation : Quaternion.identity;

GameObject obj = Instantiate(prefab, point.position, rot, parent);

aliveCount++;
obj.AddComponent<SpawnedTracker>().Init(this);
return obj;
}

void OnSpawnedDestroyed() => aliveCount--;

class SpawnedTracker : MonoBehaviour
{
    Spawner owner;
    public void Init(Spawner s) => owner = s;
    void OnDestroy()
    {
        if (owner != null) owner.OnSpawnedDestroyed();
    }
}

}