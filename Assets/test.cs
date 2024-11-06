using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsServer)
            {
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                NetworkManager.Singleton.SceneManager.LoadScene("Map"+ nextSceneIndex, LoadSceneMode.Single);

            }

        }

    }

}
