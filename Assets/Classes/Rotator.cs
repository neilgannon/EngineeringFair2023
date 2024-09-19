using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float Amount = 10;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, Amount * Time.deltaTime);
    }
}
