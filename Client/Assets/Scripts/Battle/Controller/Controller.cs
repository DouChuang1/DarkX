using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Controller :MonoBehaviour
{
    public Animator ani;
    private Vector2 dir = Vector2.zero;
    protected bool isMove = false;

    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

    public Vector2 Dir
    {
        get
        {
            return dir;
        }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    protected bool skillMove = false;
    protected float skillMoveSpeed = 0;

    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Blend", blend);
    }

    public virtual void SetAction(int act)
    {
        ani.SetInteger("Action", act);
    }

    public virtual void SetFX(string name, float destroyTime)
    {

    }

    public void SetSkillMoveState(bool move,float skillSpeed=0)
    {
        skillMove = move;
        skillMoveSpeed = skillSpeed;
    }

}
