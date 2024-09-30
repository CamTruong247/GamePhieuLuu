using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Image healthbar;

    private float health = 100;

    private void Update()
    {
        if (health < 100)
        {
            health += 0.5f * Time.deltaTime;
            healthbar.fillAmount = health / 100f;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            health -= 20f;
        }
    }
}
