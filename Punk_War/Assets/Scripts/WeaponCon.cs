using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCon : MonoBehaviour
{
    public float gunRadious, // Gun ȸ�� ������
                        gunPosY,   //Gun ȸ�� �߽� Y��
                        gunPosZ,  //Gun Z��ġ
                        bulletSpeed;  //Bullet �ӵ�

    public GameObject gun,  //Gun ������Ʈ
                                   bullet,  //Bullet ������
                                   muzzle;   //�Ѿ˻�����ġ

    Camera mainCam;
    Vector2 mousePos;   //ī�޶� �� ���콺 ��ġ
    float mouseAngle;      //���콺�� �÷��̾���� X��� �̷�� ��

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
            GunFire(); //��Ŭ�ܹ�
        }
        if (Input.GetMouseButton(1))
        {
            GunFire(); //��Ŭ����
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
