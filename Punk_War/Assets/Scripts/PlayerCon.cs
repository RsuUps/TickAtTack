using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public float hp,  //플레이어 체력
                        playerMovSpeed, // 플레이어 이동속도
                       playerJumpPower, // 플레이어 점프력
                       NDT; //피격후 무적시간

    public int playerJumpCount, // 플레이어 점프횟수
                    playerJumpCountMX; // 플레이어 최대 점프횟수
                    

    public KeyCode jump; //점프키

    Rigidbody2D rb2D; //플레이어 리지드바디
    Animator animator; //플레이어 애니메이터
    SpriteRenderer spriteRenderer; //플레이어 스프라이트
    bool isNoDamageTime = false;  //플레이어 무적 여부
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerJump();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        Vector2 moveTo;
        if (Input.GetAxis("Horizontal") == 0)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1, 1 << LayerMask.NameToLayer("Stair"));
            float angle = Vector2.Angle(hit.normal, Vector2.right);
            if (Input.GetAxis("Horizontal") > 0)
            {
                angle -= 90;
                spriteRenderer.flipX = false;
            }
            else
            {
                angle += 90;
                spriteRenderer.flipX = true;
            }
            if (hit.collider == null)
            {
                moveTo = new Vector2(Input.GetAxis("Horizontal"), 0);
            }
            else
            {
                moveTo = new Vector2(Mathf.Cos(angle * Mathf.PI / 180), Mathf.Sin(angle * Mathf.PI / 180)) * Mathf.Abs(Input.GetAxis("Horizontal"));
            }
            transform.Translate(moveTo * playerMovSpeed * Time.deltaTime);
        }
    }
    void PlayerJump()
    {
        if (Input.GetKeyDown(jump) && playerJumpCount >= 1)
        {
            animator.SetBool("isJumping", true);
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(Vector2.up * playerJumpPower, ForceMode2D.Impulse);
            playerJumpCount--;
        }
    }

    public void Dammaged(float dammage, float knockback, float posX)
    {
        if (!isNoDamageTime)
        {
            hp -= dammage;
            if (hp <= 0)
            {
                //사망모션
                Destroy(gameObject);
            }
            rb2D.AddForce(new Vector2(posX > gameObject.transform.position.x ? -1 : 1, 1) * knockback);
            isNoDamageTime = true;
            StartCoroutine(NoDamageTime());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform")
        {
            playerJumpCount = playerJumpCountMX;
            animator.SetBool("isJumping", false);
        }
    }

    IEnumerator NoDamageTime()
    {
        int countTime = 0;
        while (countTime < NDT)
        {
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            }
            yield return new WaitForSeconds(0.2f);
            countTime++;
        }
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        isNoDamageTime = false;

        yield return null;
    }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;

//public class PlayerCon : MonoBehaviour
//{
//    public float hp, mana, mov_speed, jump_pow, dash_time, dash_speed;
//    public int jump_count = 1, jump_count_mx = 1, NDT = 10, dash_cooltime = 10;
//    public KeyCode jump = KeyCode.Space;               //키세팅

//    public float dashtime, angle;
//    bool is_NoDamageTime = false, canDash = true, isDashing = false;
//    Vector2 mouse_pos;                                              //마우스 위치

//    public Camera cam;

//    Rigidbody2D rb;
//    SpriteRenderer spriteRenderer;
//    GunCon gunCon;
//    Animator animator;
//    CinemachineBrain cinemachineBrain;
//    Vector2 move_by_platform;
//    float gravity_save;


//    // Start is called before the first frame update
//    void Start()
//    {
//        rb = gameObject.GetComponent<Rigidbody2D>();
//        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
//        gunCon = gameObject.GetComponent<GunCon>();
//        animator = gameObject.GetComponent<Animator>();
//        cinemachineBrain = cam.GetComponent<CinemachineBrain>();
//        cinemachineBrain.OutputCamera.GetComponent<CinemachineVirtualCamera>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        PlayerFire();
//        PlayerSwap();
//        PlayerJump();
//        PlayerDash();
//    }


//    private void FixedUpdate()
//    {
//        PlayerMove();
//    }


//    //플레이어 이동
//    void PlayerMove()
//    {
//        if (isDashing)
//        {
//            if (dashtime > 0)
//            {
//                transform.Translate(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dash_speed * Time.deltaTime);
//                dashtime -= 1f * Time.deltaTime;
//            }
//            else
//            {
//                isDashing = false;
//                animator.SetBool("is_dashing", false);
//                rb.gravityScale = gravity_save;
//            }

//        }
//        else
//        {
//            if (Input.GetAxis("Horizontal") == 0)
//            {
//                animator.SetBool("is_walking", false);
//            }
//            else
//            {
//                animator.SetBool("is_walking", true);
//                if (Input.GetAxis("Horizontal") > 0)
//                {
//                    spriteRenderer.flipX = false;
//                }
//                else
//                {
//                    spriteRenderer.flipX = true;
//                }
//                transform.Translate(new Vector2(mov_speed * Input.GetAxis("Horizontal") * Time.deltaTime, 0));
//            }
//        }
//    }
//    //플레이어 점프
//    void PlayerJump()
//    {
//        if (Input.GetKeyDown(jump) && jump_count >= 1)
//        {
//            animator.SetBool("is_jumping", true);
//            rb.velocity = Vector2.zero;

//            rb.AddForce((spriteRenderer.flipY ? Vector2.down : Vector2.up) * jump_pow, ForceMode2D.Impulse);
//            jump_count--;
//        }
//    }
//    //플레이어 대쉬
//    void PlayerDash()
//    {
//        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
//        {
//            if (!animator.GetBool("is_jumping"))
//            {
//                jump_count--;
//            }
//            dashtime = dash_time;
//            angle = Mathf.Atan2(mouse_pos.y - transform.position.y, mouse_pos.x - transform.position.x);
//            isDashing = true;
//            animator.SetBool("is_dashing", true);
//            rb.velocity = Vector2.zero;
//            gravity_save = rb.gravityScale;
//            rb.gravityScale = 0;
//            canDash = false;
//            StartCoroutine(DashCoolTime());
//        }
//    }

//    //플레이어 무기 발사 -> <GunCon>으로 이동
//    void PlayerFire()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            gunCon.GunFire();
//        }
//        if (Input.GetMouseButton(1))
//        {
//            gunCon.GunFire();
//        }
//    }
//    //플레이어 무기 변경 -> <GunCon>으로 이동
//    void PlayerSwap()
//    {
//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            gunCon.GunSwap();
//        }
//    }


//    public void Dammaged(float dammage, float knockback, float posX)
//    {
//        if (!is_NoDamageTime)
//        {
//            hp -= dammage;
//            if (hp <= 0)
//            {
//                //사망모션
//                Destroy(gameObject);
//            }
//            rb.AddForce(new Vector2(posX > gameObject.transform.position.x ? -1 : 1, 1) * knockback);
//            is_NoDamageTime = true;
//            StartCoroutine(NoDamageTime());
//        }
//    }

//    public void ChangeGravity()
//    {
//        gunCon.gun_posY = -gunCon.gun_posY;
//        if (isDashing)
//        {
//            gravity_save = -gravity_save;
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.collider.tag == "platform")
//        {
//            jump_count = jump_count_mx;
//            animator.SetBool("is_jumping", false);
//        }
//    }

//    IEnumerator NoDamageTime()
//    {
//        int countTime = 0;
//        while (countTime < NDT)
//        {
//            if (countTime % 2 == 0)
//            {
//                spriteRenderer.color = new Color32(255, 255, 255, 90);
//            }
//            else
//            {
//                spriteRenderer.color = new Color32(255, 255, 255, 180);
//            }
//            yield return new WaitForSeconds(0.2f);
//            countTime++;
//        }
//        spriteRenderer.color = new Color32(255, 255, 255, 255);
//        is_NoDamageTime = false;

//        yield return null;
//    }

//    IEnumerator DashCoolTime()
//    {
//        int countTime = 0;
//        while (countTime < dash_cooltime)
//        {
//            yield return new WaitForSeconds(0.2f);
//            countTime++;
//        }
//        canDash = true;

//        yield return null;
//    }
//}
