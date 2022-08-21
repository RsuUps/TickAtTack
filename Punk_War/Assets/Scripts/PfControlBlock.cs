using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PfControlBlock : MonoBehaviour
{
    public bool isDestroy = false, isTurningPoint = false;
    public Vector3 moveVec = new Vector3(0, 0, 0), reGenPoint = new Vector3(0,0,0);
    public float moveSpeed = -1, regenTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MovePlatform")
        {
            if (isDestroy)
            {
                DestPf(collision.gameObject);
            }
            else if (isTurningPoint)
            {
                collision.GetComponent<PfCon>().ReturnPf(moveVec);
            }
            else
            {
                if (moveSpeed >= 0)
                {
                    moveVec = collision.GetComponent<PfCon>().ChangeMoveVector(moveVec, moveSpeed);
                }
                else
                {
                    moveVec = collision.GetComponent<PfCon>().ChangeMoveVector(moveVec);
                }
            }
        }
    }

    void DestPf(GameObject Pf)
    {
        Pf.SetActive(false);
        Pf.transform.position = reGenPoint;
        StartCoroutine(RegenPf(Pf));
    }

    IEnumerator RegenPf(GameObject Pf)
    {
        float time = regenTime;
        while (time > 0)
        {
            yield return new WaitForSeconds(0.5f);
            time--;
        }
        Pf.SetActive(true);
    }
}
