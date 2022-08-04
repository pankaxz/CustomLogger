using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PNKZ;

public class TestScript : MonoBehaviour
{

    void Update()
    {
        transform.localScale = new Vector3(
            Mathf.Sin(Time.time + Random.Range(0.5f, 10) * Time.deltaTime),
            Mathf.Sin(Time.time + Random.Range(0.5f, 10) * Time.deltaTime),
            Mathf.Sin(Time.time + Random.Range(0.5f, 10) * Time.deltaTime)
            );


        //GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        Debug.Log($"{this.name} scale : {transform.localScale}"); 
        Debug.Log($"{this.name} positon : {transform.position}");
    }
}
