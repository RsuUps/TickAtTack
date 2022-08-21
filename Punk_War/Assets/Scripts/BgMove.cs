using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMove : MonoBehaviour
{
    public float speed;  //����̵��ӵ�
    public List<GameObject> BG;  //�̵�����

    int currentBG = 0, nextBG;  //����̵��� ���� ������

    static float sizeOfBG = 35.5f;  //��潺������Ʈ�� Xũ��
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BackgroundMove();
        BackgroundPull();
    }

    //����̵�
    void BackgroundMove()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            gameObject.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * -speed * Time.deltaTime, 0, 0));
        }
    }

    //���Ǯ��
    void BackgroundPull()
    {
        nextBG = GetQuot(gameObject.transform.localPosition.x, sizeOfBG / 2) > 0
                           ? GetQuot(gameObject.transform.localPosition.x, sizeOfBG / 2) % 3
                          : Mathf.Abs(GetQuot(gameObject.transform.localPosition.x, sizeOfBG / 2) * 2 % 3);

        if (nextBG != currentBG)
        {
            int moveBG = 3 - (currentBG + nextBG);
            if (nextBG-currentBG==1|| nextBG - currentBG == -2)
            {
                BG[moveBG].transform.localPosition = new Vector3(BG[nextBG].transform.localPosition.x + sizeOfBG/2, 0, 0);
            }
            else
            {
                BG[moveBG].transform.localPosition = new Vector3(BG[nextBG].transform.localPosition.x - sizeOfBG/2, 0, 0);
            }
            currentBG = nextBG;
        }
    }
    //�� ��������
    int GetQuot(float a, float b)
    {
        int c = 0;
        if (a >= 0)
        {
            while (a >= b)
            {
                c++;
                a -= b;
            }
        }
        else
        {
            while (a <= -b)
            {
                c--;
                a += b;
            }
        }
        return c;
    }
}
