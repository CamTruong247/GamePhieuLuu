using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private Image healthbar;

    private float health = 100;

    //public NetworkVariable<ulong> ID { get; private set; }
    //public NetworkVariable<FixedString128Bytes> PlayerName;

    //private void Awake()
    //{
    //    ID = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //    PlayerName = new("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //    RegisterEvent();
    //}

    //private void RegisterEvent()
    //{
    //    ID.OnValueChanged += (oldID, newID) =>
    //    {
    //        oldID = newID;
    //    };
    //    PlayerName.OnValueChanged += (oldName, newName) =>
    //    {
    //        name = newName.Value;
    //    };
    //}

    //public override void OnNetworkSpawn()
    //{
    //    if (IsOwner)
    //    {
    //        ID.Value = OwnerClientId;
    //        PlayerName.Value = PlayerPrefs.GetString("Player_Name" + ID);
    //        return;
    //    }
    //    name = PlayerName.Value.ToString();
    //}

    private void Update()
    {
        //if (health < 100)
        //{
        //    health += 0.5f * Time.deltaTime;
        //    healthbar.fillAmount = health / 100f;
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    health -= 20f;
        //}
    }
}
