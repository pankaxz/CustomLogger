using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlaneScript : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 40);
        transform.Rotate(Vector3.right, Time.deltaTime * Mathf.Sin(Time.time) * 10f);
        transform.Rotate(Vector3.forward, Time.deltaTime * Mathf.Sin(Time.time) * 10f);

        Debug.LogWarning($"{name} rotation : {transform.rotation}");
    }
}
