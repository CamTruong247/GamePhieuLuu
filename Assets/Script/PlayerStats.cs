using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private Image healthbar;
    [SerializeField] private GameObject healingEffect;
    private float health = 100f;
    private float maxHealth = 100f;
    public SkillData healSkillData;

    private void Awake()
    {
        if (IsOwner)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Nếu kỹ năng hồi máu tự động đã mở khóa, bắt đầu hồi máu mỗi 10 giây
        if (healSkillData.isSkillUnlocked)
        {
            StartCoroutine(AutoHeal());
        }
    }

    private IEnumerator AutoHeal()
    {
        while (true)
        {
            // Đợi 10 giây
            yield return new WaitForSeconds(10f);

            // Hồi phục 50% máu (50% của maxHealth)
            Heal(20f);
        }
    }

    private void Heal(float healAmount)
    {
        float newHealth = Mathf.Min(health + healAmount, maxHealth);

        // Nếu lượng máu thay đổi, cập nhật lại
        if (newHealth != health)
        {
            // Cập nhật lượng máu mới vào server
            UpdateHealthServerRpc(-(newHealth - health));  // Truyền giá trị âm để tăng máu
            health = newHealth;  // Cập nhật giá trị health trên client
            SpawnHealingEffect();
        }
    }
    private GameObject healingEffectInstance;
    private void SpawnHealingEffect()
    {
        // Kiểm tra nếu prefab healingEffectPrefab không phải là null
        if (healingEffect != null)
        {
            // Instantiate prefab healingEfct tại vị trí của người chơi
            healingEffectInstance = Instantiate(healingEffect, transform.position, Quaternion.identity);
        }
        StartCoroutine(Deplay());
    }
    private IEnumerator Deplay()
    {
      
        yield return new WaitForSeconds(0.5f);

        if (healingEffectInstance != null)
        {
            Debug.Log("Destroying healing effect instance.");
            Destroy(healingEffectInstance); // Hủy instance của healing effect
        }

    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        HealthBarServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void HealthBarServerRpc()
    {
        HealthBarClientRpc();
    }

    [ClientRpc]
    private void HealthBarClientRpc()
    {
        healthbar.fillAmount = health / maxHealth;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateHealthServerRpc(float health)
    {
        // Chỉ giảm máu (khi bị tấn công hoặc nhận thiệt hại)
        UpdateHealthClientRpc(health);
    }

    [ClientRpc]
    public void UpdateHealthClientRpc(float health)
    {
        // Giảm máu dựa trên giá trị từ server
        this.health -= health;
        Debug.Log("Updated Health: " + this.health); // Kiểm tra giá trị máu khi cập nhật

        // Kiểm tra nếu máu <= 0 thì hủy nhân vật
        if (this.health <= 0)
        {
            Debug.Log("Player health reached 0, destroying the object.");
            Destroy(gameObject);
        }
    }
}