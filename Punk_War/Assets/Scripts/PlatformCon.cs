using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    destroy,
    roop,
    turn
}

public class PlatformCon : MonoBehaviour
{
    public Vector3[] vectors;
    public PlatformType type;
    public float speed;
    public int rebirthTime;

    public int state = 0;
    bool isRoop = false;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (type == PlatformType.destroy)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlatformMoving();
    }

    void PlatformMoving()
    {
        if (type!=PlatformType.destroy||!animator.GetBool("isDestroy"))
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, vectors[state + 1], speed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TurningPoint" && collision.name == "TP"+(state+1).ToString())
        {
            if (isRoop)
            {
                if (state == 0)
                {
                    isRoop = false;
                    state++;
                }
                else
                state--;
                
            }
            state++;
            if (state+1 == vectors.Length)
            {
                switch (type)
                {
                    case PlatformType.destroy:
                        StartCoroutine(RebirthPlatform());
                        state = 0;
                        animator.SetBool("isDestroy",true);
                        break;

                    case PlatformType.roop:
                        isRoop = true;
                        state--;
                        break;
                    case PlatformType.turn:
                        state = 0;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    IEnumerator RebirthPlatform()
    {
        int a = rebirthTime;
        while (a>0)
        {
            a--;
            Debug.Log(a);
            yield return new WaitForSeconds(0.2f);
        }
        
        transform.position = vectors[0];
        animator.SetBool("isDestroy", false);
    }
}
