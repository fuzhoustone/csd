using UnityEngine;
using System.Collections;

public class Player2Control : MonoBehaviour {

    private Animation animation;
    void Start()
    {

        animation = GetComponent<Animation>();
    }

    void Update()
    {
        bool isRun = false;
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            isRun = true;
            
        }
        else //if (Input.GetAxisRaw("Vertical") == 0)
        {
            isRun = false;
            
        }

        bool isJump = false;
        if (Input.GetButton("Jump"))  {
            isJump = true;
            
        }

        if (isJump)
        {
            animation.CrossFade("jump");

        }
        else if (isRun)
        {
            animation.CrossFade("run");
        }
        else {
            animation.CrossFade("Stand");
        }

        

    }
}
