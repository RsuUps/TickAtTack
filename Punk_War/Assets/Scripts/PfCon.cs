using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PfCon : MonoBehaviour
{
    public float moveSpeed = 0;
    public bool isReturn = false;

    public List<GameObject> moveWith;
    Vector3 moveVec = new Vector3(0, 0, 0);
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isMoving", true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (animator.GetBool("isMoving"))
        {
            gameObject.transform.Translate(moveVec * moveSpeed * Time.deltaTime);
            for (int i = 0; i < moveWith.Count; i++)
            {
                moveWith[i].transform.Translate(moveVec * moveSpeed * Time.deltaTime);
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag != "Untagged" && !moveWith.Contains(collision.collider.gameObject))
        {
            Debug.Log(collision.collider.tag + "+");
            moveWith.Add(collision.collider.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag != "Untagged" && moveWith.Contains(collision.collider.gameObject))
        {
            Debug.Log(collision.collider.tag + "-");
            moveWith.Remove(collision.collider.gameObject);
        }
    }

    public void ReturnPf(Vector3 vec)
    {
        Vector3 saveVec = moveVec;
        moveVec = vec;
        animator.SetBool("isMovingReverse", !animator.GetBool("isMovingReverse"));
    }

    public Vector3 ChangeMoveVector(Vector3 vec)
    {
        Vector3 saveVec = moveVec;
        moveVec = vec;
        if (isReturn)
        {
            return -saveVec;
        }
        else
            return vec;
    }

    public Vector3 ChangeMoveVector(Vector3 vec, int stopTime)
    {
        Vector3 saveVec = moveVec;
        moveVec = vec;
        StartCoroutine(StopSec(stopTime));
        if (isReturn)
        {
            return -saveVec;
        }
        else
            return vec;
    }
    public Vector3 ChangeMoveVector(Vector3 vec, float speed)
    {
        Vector3 saveVec = moveVec;
        moveVec = vec;
        moveSpeed = speed;
        if (isReturn)
        {
            return -saveVec;
        }
        else
            return vec;
    }



    IEnumerator StopSec(int stopTime)
    {
        animator.SetBool("isMoving", false);
        int time = stopTime;
        while (time > 0)
        {
            yield return new WaitForSeconds(0.2f);
            time--;
        }
        animator.SetBool("isMoving", true);
    }
}
