using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCon : MonoBehaviour
{
    public float gunRadious, // Gun 회전 반지름
                        gunPosY,   //Gun 회전 중심 Y축
                        gunPosZ,  //Gun Z위치
                        bulletSpeed;  //Bullet 속도

    public GameObject gun,  //Gun 오브젝트
                                   bullet,  //Bullet 프리팹
                                   muzzle;   //총알생성위치

    Camera mainCam;
    Vector2 mousePos;   //카메라 상 마우스 위치
    float mouseAngle;      //마우스가 플레이어기준 X축과 이루는 각

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFire();
        GunPosition();
    }

    void PlayerFire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GunFire(); //좌클단발
        }
        if (Input.GetMouseButton(1))
        {
            GunFire(); //우클연사
        }
    }

    void GunPosition()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseAngle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);

        gun.transform.position = new Vector3(Mathf.Cos(mouseAngle) * gunRadious + transform.position.x,
                                                                    Mathf.Sin(mouseAngle) * gunRadious + transform.position.y + gunPosY,
                                                                    gunPosZ);
        gun.transform.rotation = Quaternion.AngleAxis(mouseAngle * Mathf.Rad2Deg, Vector3.forward);
    }


    public void GunFire()
    {
        GameObject bul = Instantiate<GameObject>(bullet, muzzle.transform.position, Quaternion.identity);
        bul.transform.rotation = gun.transform.rotation;
        bul.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(mouseAngle), Mathf.Sin(mouseAngle)) * bulletSpeed;
    }
}

//gun_scale = gun.transform.localScale;
//if (mouse_pos.x > gameObject.transform.position.x)
//{
//    gun.transform.localScale = new Vector3(gun_scale.x, 1 * gun_scaleY, gun_scale.z);
//}
//else
//{
//    gun.transform.localScale = new Vector3(gun_scale.x, -1 * gun_scaleY, gun_scale.z);
//}
