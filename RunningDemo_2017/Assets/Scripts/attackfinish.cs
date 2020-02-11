using UnityEngine;
using System.Collections;

public class attackfinish : MonoBehaviour {

    Animator m_animator;
    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void attack1finish()
    {
        Debug.LogWarning("attack1finish");
        m_animator.SetBool("Attack", false);
    }
}
