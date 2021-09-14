using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DamageCal
{
    public class RoleDamageCal
    {
        private static RoleDamageCal _instance = null;

        public static RoleDamageCal instance {
            get {
                if (_instance == null) {
                    _instance = new RoleDamageCal();
                }

                return _instance;
            }
        }
        //计算伤害
        public int DamageCal(roleProperty AttPro, roleProperty DefPro)
        {
            int damage = 1;
            if (AttPro.attack > 0)
                damage = AttPro.attack;

            return damage;
        }

    }
}
