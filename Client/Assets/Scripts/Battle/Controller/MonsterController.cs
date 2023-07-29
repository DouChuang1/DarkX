using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MonsterController : Controller
{
    private void Update()
    {
        if(isMove)
        {
            SetDir();
            SetMove();
        }
    }


    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Const.MonsterMoveSpeed);
        ctrl.Move(Vector3.down * Time.deltaTime * Const.MonsterMoveSpeed);
    }
}
