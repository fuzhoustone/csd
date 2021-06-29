using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleDamageCal 
{
    //计算伤害
    public int DamageCal(roleProperty AttPro, roleProperty DefPro) {
        int damage = 1;
        if (AttPro.attack > 0)
            damage = AttPro.attack;

        return damage;
    }

}
