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


   // void setToAttack(Vector3 rolePos);

   // void setStopAttack(); 

   // void setToStand(); 

  //  bool IsInAttackState(); 

   // bool IsDie(); 
}
