using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public List<GameObject> clock;
    public List<float> range;

    public float tick;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tik());
    }

    IEnumerator Tik()
    {
        for (int h = 0; h < 12; h++)
        {
            clock[0].transform.localPosition = new Vector3(Mathf.Cos((90 - h * 30) * Mathf.PI / 180),
                                                                                              Mathf.Sin((90 - h * 30) * Mathf.PI / 180),
                                                                                              0) * range[0];
            clock[0].transform.localRotation = Quaternion.Euler(0, 0, 90 - h * 30);
            for (int m = 0; m < 60; m++)
            {
                clock[1].transform.localPosition = new Vector3(Mathf.Cos((90 - m * 6) * Mathf.PI / 180),
                                                                                              Mathf.Sin((90 - m * 6) * Mathf.PI / 180),
                                                                                              0) * range[1];
                clock[1].transform.localRotation = Quaternion.Euler(0, 0, 90 - m * 6);
                for (int s = 0; s < 60; s++)
                {
                    clock[2].transform.localPosition = new Vector3(Mathf.Cos((90 - s * 6) * Mathf.PI / 180),
                                                                                              Mathf.Sin((90 - s * 6) * Mathf.PI / 180),
                                                                                              0) * range[2];
                    clock[2].transform.localRotation = Quaternion.Euler(0, 0, 90 - s * 6);
                    yield return new WaitForSeconds(tick);
                }
            }
        }
        StartCoroutine(Tik());
    }
}
