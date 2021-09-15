using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using stoneState;

public interface IbaseANI  
{
    void initData(GameObject paraObj);

    bool isInPlayEntry(roleState stateName); 

    void PlayState(roleState stateName);


   // bool attackStateEnd();


    void dieStateEnd();

    roleState getRoleNowState();

    roleState getHopeState(float h, float tmpv, bool isfire, bool isKeyJump);
    //bool updataRoleControl(float h, float tmpv, bool isfire, bool isJump = false);

   // void setToAttack(Vector3 rolePos);

    // void setStopAttack(); 

    // void setToStand(); 

    //  bool IsInAttackState(); 

    // bool IsDie(); 
}
