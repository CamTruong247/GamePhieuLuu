using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private GameObject bullet; // GameObject cho đạn
    [SerializeField] private GameObject laser; // GameObject cho kỹ năng laser
    [SerializeField] private Transform shootingPoint; // Điểm bắn
    public Animator animator; // Animator để điều khiển hoạt ảnh

    private bool isAutoAttackActive = false; // Trạng thái tấn công tự động

    void Update()
    {
        // Kiểm tra nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            AttackServerRpc();
        }

        // Kiểm tra nhấn phím R để kích hoạt tấn công tự động
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isAutoAttackActive)
            {
                isAutoAttackActive = true;
                StartCoroutine(AutoAttack());
            }
            else
            {
                isAutoAttackActive = false;
            }
        }

        // Kiểm tra nhấn phím Q để sử dụng kỹ năng
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkillServerRpc(); // Gọi hàm sử dụng kỹ năng
        }
    }

    private void FixedUpdate()
    {
        RotationWeapon(); // Xoay vũ khí theo chuột
    }

    private IEnumerator AutoAttack()
    {
        while (isAutoAttackActive)
        {
            AttackServerRpc();
            yield return new WaitForSeconds(0.3f); // Điều chỉnh tốc độ tấn công
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AttackServerRpc()
    {
        if (IsServer)
        {
            AttackClientRpc();
            var b = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
            b.GetComponent<NetworkObject>().Spawn();
        }
    }

    [ClientRpc]
    private void AttackClientRpc()
    {
        animator.SetTrigger("Attack"); // Gán trigger cho hoạt ảnh tấn công
    }

    [ServerRpc(RequireOwnership = false)]
    public void UseSkillServerRpc()
    {
        if (IsServer)
        {
           
            Vector3 laserPosition = transform.position; // Đặt laser ở vị trí của nhân vật
            var l = Instantiate(laser, laserPosition, transform.rotation);
            l.GetComponent<NetworkObject>().Spawn();

            // Gọi coroutine để hủy laser sau 0.5 giây
            StartCoroutine(DestroyLaserAfterDelay(l, 0.8f));
        }
    }

    // Coroutine để hủy laser sau một khoảng thời gian nhất định
    private IEnumerator DestroyLaserAfterDelay(GameObject laser, float delay)
    {
        yield return new WaitForSeconds(delay); // Chờ trong khoảng thời gian delay
        Destroy(laser); // Hủy laser
    }



    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false; // Vô hiệu hóa script nếu không phải chủ sở hữu
            return;
        }
    }

    private void RotationWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 look = mousePos - transform.position;
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation; // Xoay vũ khí
    }
}
