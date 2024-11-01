using UnityEngine;
using Unity.Netcode;

public class PetFollowEnemy : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float lifetime = 15.0f; // Thời gian tồn tại của slime
    private Transform enemyTarget;
    private Quaternion originalRotate;

    void Start()
    {
        if (IsServer)
        {
            // Tìm đối tượng có tag "Enemy" khi slime được triệu hồi
            GameObject enemy = GameObject.FindWithTag("Enemy");
            if (enemy != null)
            {
                enemyTarget = enemy.transform;
                MoveTowardsEnemyServerRpc(enemyTarget.position);
            }
            else
            {
                Debug.LogWarning("No enemy with tag 'Enemy' found in the scene.");
            }

            // Bắt đầu đếm ngược thời gian để tự hủy slime
            Invoke(nameof(DestroySelf), lifetime);
        }
    }

    void Update()
    {
        if (IsServer && enemyTarget != null)
        {
            MoveTowardsEnemyServerRpc(enemyTarget.position);
        }
        transform.rotation = originalRotate;
    }

    [ServerRpc]
    private void MoveTowardsEnemyServerRpc(Vector3 targetPosition)
    {
        // Tính toán hướng di chuyển về phía enemy và cập nhật vị trí của slime
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        // Cập nhật vị trí và hướng trên client
        UpdatePositionClientRpc(newPosition, direction);
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 newPosition, Vector3 direction)
    {
        transform.position = newPosition;

        // Cập nhật hướng nhìn của slime để luôn hướng về phía enemy
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void DestroySelf()
    {
        // Tự hủy slime sau thời gian tồn tại
        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
