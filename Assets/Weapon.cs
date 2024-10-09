using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootingPoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
        }
        RotationWeapon();
    }
     
    private void RotationWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 look = mousePos - transform.position;
        float angle = Mathf.Atan2(look.y, look.x)*Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0,0,angle);
        transform.rotation = rotation;

        /*if(transform.eulerAngles.z > 90 && transform.eulerAngles.z <270)
        {
            transform.localScale = new Vector3(1, -1, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 0);
        }*/
    }
}
