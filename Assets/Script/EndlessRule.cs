using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : NetworkBehaviour
{
    public GameObject slimePrefab;
    public GameObject werewolfPrefab;
    public GameObject slimeKingPrefab;
    public GameObject golemBossPrefab;
    public float waveDuration = 30f; // Thời gian mỗi lượt (30 giây)
    public int monstersPerWave = 10; // Số quái vật mỗi lượt
    private int currentWave = 0; // Đếm số lượt hiện tại

    private bool isWaveInProgress = false;

    private List<GameObject> activeEnemies = new List<GameObject>(); // Danh sách các quái vật đang sống

    private void Start()
    {
        if (IsServer)
        {
            StartCoroutine(SpawnWave()); // Server bắt đầu spawn
        }
    }

    private IEnumerator SpawnWave()
    {
        while (true)
        {
            if (!isWaveInProgress)
            {
                isWaveInProgress = true;
                currentWave++;

                // Gọi RPC để spawn quái vật trên server và client
                SpawnMonstersServerRpc(currentWave);

                // Đợi trong 30 giây cho mỗi lượt
                yield return new WaitForSeconds(waveDuration);

                
                isWaveInProgress = false;

            }

            yield return null;
        }
    }

    // ServerRPC để spawn quái vật trên server
    [ServerRpc(RequireOwnership = false)]
    private void SpawnMonstersServerRpc(int wave)
    {
        SpawnMonstersClientRpc(wave);
    }

    // ClientRpc để spawn quái vật trên tất cả client
    [ClientRpc]
    private void SpawnMonstersClientRpc(int wave)
    {
        SpawnMonsters(wave);
    }

    private void SpawnMonsters(int wave)
    {
        Debug.Log("Monsters Per Wave: " + monstersPerWave);
        if (monstersPerWave == 0)
        {
            Debug.LogError("Number of monsters per wave is zero!");
            return;
        }

        for (int i = 0; i < monstersPerWave; i++)
        {
            // Sinh vị trí ngẫu nhiên trong phạm vi
            Vector3 randomPosition = GetRandomSpawnPosition();

            // Chọn quái vật theo độ khó
            GameObject monsterToSpawn = GetMonsterByDifficulty(wave);

            if (monsterToSpawn != null)
            {
                // Instantiate quái vật tại vị trí ngẫu nhiên
                GameObject monster = Instantiate(monsterToSpawn, randomPosition, Quaternion.identity);

                // Spawn quái vật trên mạng (NetworkObject)
                NetworkObject networkObject = monster.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    networkObject.Spawn();

                    // Thêm quái vật vào danh sách activeEnemies
                    activeEnemies.Add(monster);
                }
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Đặt phạm vi spawn (bạn có thể thay đổi giá trị theo yêu cầu)
        float xRange = 10f; // Phạm vi theo trục X
        float yRange = 10f; // Phạm vi theo trục Y

        // Tạo tọa độ ngẫu nhiên trong phạm vi
        float randomX = Random.Range(-xRange, xRange);
        float randomY = Random.Range(-yRange, yRange);

        // Trả về vị trí ngẫu nhiên
        return new Vector3(randomX, randomY, 0f); // Nếu bạn dùng 2D, z sẽ là 0
    }

    private GameObject GetMonsterByDifficulty(int wave)
    {
        // Cài đặt quái vật theo độ khó
        if (wave <= 3)
        {
            return slimePrefab;
        }
        else if (wave <= 6)
        {
            return werewolfPrefab;
        }
        else if (wave <= 9)
        {
            return slimeKingPrefab;
        }
        else
        {
            return golemBossPrefab;
        }
    }

    // Gọi phương thức này khi quái vật bị tiêu diệt
    public void RemoveMonsterFromActiveList(GameObject monster)
    {
        if (activeEnemies.Contains(monster))
        {
            activeEnemies.Remove(monster);
        }
    }
}
