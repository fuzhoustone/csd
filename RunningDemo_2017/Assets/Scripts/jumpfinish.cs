using UnityEngine;
using System.Collections;

public class jumpfinish : MonoBehaviour {

    // 动画播放器
    Animator m_animator;
    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void jumpFinishEvent() {
        Debug.LogWarning("jumpFinish");
        m_animator.SetBool("jump",false);
    }
	
}
