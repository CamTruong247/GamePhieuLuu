using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CheckPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject attackpoint;
    [SerializeField] private GameObject menu;
    public float radius;
    public LayerMask layer;
    public LayerMask layercheck;
    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackpoint.transform.position, radius, Vector2.zero, 0f, layer);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            // Kiểm tra xem đối tượng bị trúng có phải là Enemy hay không
            if (System.Array.Exists(enemies, enemy => enemy == hitObject))
            {
                if (IsServer)
                {
                    hitObject.GetComponent<NetworkObject>().Despawn();
                }
            }
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        RaycastHit2D[] hits1 = Physics2D.CircleCastAll(attackpoint.transform.position, radius, Vector2.zero, 0f, layercheck);
        foreach (RaycastHit2D hit in hits1)
        {
            GameObject hitObject = hit.collider.gameObject;
            // Kiểm tra xem đối tượng bị trúng có phải là player hay không
            if (System.Array.Exists(players, player => player == hitObject))
            {
                menu.SetActive(false);
            }
        }
    }
}
