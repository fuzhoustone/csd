using UnityEngine;
using System.Collections;

public class attackCollider2 : MonoBehaviour {

    // Use this for initialization

    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("attack collider2");
    }
}
