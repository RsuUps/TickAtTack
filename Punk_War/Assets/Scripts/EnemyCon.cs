using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCon : MonoBehaviour
{
    public float enemyHP = 10,  //적 오브젝트 체력
                        attack,   //적 오브젝트 공격력
                        enemyScaleX = 1,   //적 오브젝트 X크기
                        enemySpeed,   //적 오브젝트 이동속도
                        knockbackRange;  //적 오브젝트 공격시 넉백거리

    public GameObject eyesPrefap;  //눈 색깔 오브젝트

    int moveStatus = 0;   //이동상태
    bool isTracing = false;   //추적상태

    Rigidbody2D rb2D;  //적 오브젝트 리지드바디
    Animator animator;  //적 오브젝트 애니메이터

    SpriteRenderer eyeSpriteRenderer;  //눈색깔 스프라이트
    PolygonCollider2D sightCollider;  //시야 콜라이더
    CircleCollider2D chaseCollider;  //추적범위 콜라이더
    GameObject traceTarget;  //추적대상

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(ChangeMovement());
        eyeSpriteRenderer = Instantiate(eyesPrefap, gameObject.transform).GetComponent<SpriteRenderer>();
        sightCollider = GetComponent<PolygonCollider2D>();
        chaseCollider = GetComponent<CircleCollider2D>();

        chaseCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        Vector3 moveVel = Vector3.zero;

        if (isTracing)
        {
            if (gameObject.transform.position.x > traceTarget.transform.position.x)
            {
                moveVel = Vector3.left;
                transform.localScale = new Vector3(-enemyScaleX, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                moveVel = Vector3.right;
                transform.localScale = new Vector3(enemyScaleX, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            if (moveStatus == 1)
            {
                moveVel = Vector3.right;
                transform.localScale = new Vector3(enemyScaleX, transform.localScale.y, transform.localScale.z);
            }
            else if (moveStatus == 2)
            {
                moveVel = Vector3.left;
                transform.localScale = new Vector3(-enemyScaleX, transform.localScale.y, transform.localScale.z);
            }
        }
        transform.position += moveVel * Time.deltaTime * enemySpeed;
    }

    public void ObjectTracing(GameObject target)
    {
        eyeSpriteRenderer.color = new Color32(255, 0, 0, 255);
        traceTarget = target;
        StopCoroutine(ChangeMovement());
        animator.SetBool("isWalking", true);
        isTracing = true;
        chaseCollider.enabled = true;
    }

    public void Dammaged(float dammage, float knockback, float posX)
    {
        enemyHP -= dammage;
        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
        rb2D.AddForce(new Vector2(posX > gameObject.transform.position.x ? -1 : 1, 1) * knockback);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<PlayerCon>().Dammaged(attack, knockbackRange, gameObject.transform.position.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTracing&&collision.tag == "Player")
        {
            sightCollider.enabled = false;
            ObjectTracing(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isTracing && collision.tag == "Player")
        {
            eyeSpriteRenderer.color = new Color32(0, 128, 255, 255);
            animator.SetBool("isWalking", false);
            isTracing = false;
            StartCoroutine(ChangeMovement());
            chaseCollider.enabled = false;
            sightCollider.enabled = true;
        }
    }

    IEnumerator ChangeMovement()
    {
        moveStatus = Random.Range(0, 3);
        if (moveStatus == 0)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
        yield return new WaitForSeconds(3f);

        if (!isTracing)
        {
            StartCoroutine(ChangeMovement());
        }
    }
}
