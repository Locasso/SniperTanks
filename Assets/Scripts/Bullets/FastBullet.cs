using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBullet : Bullet
{
    protected override void TurnControl(short id)
    {
        GameManager.fastBulletCount++;
        GameManager.turnMoment = id;
        if (GameManager.fastBulletCount == 2)
        {
            GameManager.fastBulletCount = 0;
        }
    }
}
