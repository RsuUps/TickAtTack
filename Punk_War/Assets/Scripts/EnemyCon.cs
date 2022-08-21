using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCon : MonoBehaviour
{
    public float enemyHP = 10,  //�� ������Ʈ ü��
                        attack,   //�� ������Ʈ ���ݷ�
                        enemyScaleX = 1,   //�� ������Ʈ Xũ��
                        enemySpeed,   //�� ������Ʈ �̵��ӵ�
                        knockbackRange;  //�� ������Ʈ ���ݽ� �˹�Ÿ�

    public GameObject eyesPrefap;  //�� ���� ������Ʈ

    int moveStatus = 0;   //�̵�����
    bool isTracing = false;   //��������

    Rigidbody2D rb2D;  //�� ������Ʈ ������ٵ�
    Animator animator;  //�� ������Ʈ �ִϸ�����

    SpriteRenderer eyeSpriteRenderer;  //������ ��������Ʈ
    PolygonCollider2D sightCollider;  //�þ� �ݶ��̴�
    CircleCollider2D chaseCollider;  //�������� �ݶ��̴�
    GameObject traceTarget;  //�������

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
