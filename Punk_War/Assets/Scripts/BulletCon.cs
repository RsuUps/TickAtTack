using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCon : MonoBehaviour
{
    public float bulletDamage, knockbackRange, destroyTime;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void Awake()
    {
        StartCoroutine(BulletDestroy());
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            collision.collider.GetComponent<EnemyCon>().Dammaged(bulletDamage, knockbackRange, gameObject.transform.position.x);
        }
        if (collision.collider.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator BulletDestroy()
    {
        int countTime = 0;
        while (countTime < destroyTime)
        {
            yield return new WaitForSeconds(1f);
            countTime++;
        }
        Destroy(gameObject);

        yield return null;
    }
}
