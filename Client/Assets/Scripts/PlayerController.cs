using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    public Vector3 camOffset;
    float targetBlend;
    float currentBlend;
    public GameObject daggeratk1fx;
    public GameObject daggeratk2fx;
    public GameObject daggeratk3fx;
    public GameObject daggeratk4fx;
    public GameObject daggeratk5fx;

    public GameObject daggerskill1fx;
    public GameObject daggerskill2fx;
    public GameObject daggerskill3fx;

    public override void Init()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;

        if(daggeratk1fx!=null)
        {
            fxDic.Add(daggeratk1fx.name, daggeratk1fx);
        }
        if (daggeratk2fx != null)
        {
            fxDic.Add(daggeratk2fx.name, daggeratk2fx);
        }
        if (daggeratk3fx != null)
        {
            fxDic.Add(daggeratk3fx.name, daggeratk3fx);
        }
        if (daggeratk4fx != null)
        {
            fxDic.Add(daggeratk4fx.name, daggeratk4fx);
        }
        if (daggeratk5fx != null)
        {
            fxDic.Add(daggeratk5fx.name, daggeratk5fx);
        }

        if (daggerskill1fx != null)
        {
            fxDic.Add(daggerskill1fx.name, daggerskill1fx);
        }
        if (daggerskill2fx != null)
        {
            fxDic.Add(daggerskill2fx.name, daggerskill2fx);
        }
        if (daggerskill3fx != null)
        {
            fxDic.Add(daggerskill3fx.name, daggerskill3fx);
        }
    }

    private void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //Vector2 _dir = new Vector2(h, v).normalized;
        //if (_dir != Vector2.zero)
        //{
        //    Dir = _dir;
        ///    SetBlend(Const.BlendWalk);
        //}
        //else
        //{
        ///    Dir = Vector2.zero;
        //    SetBlend(Const.BlendIdle);
       // }
        if(currentBlend!=targetBlend)
        {
            UpdateMixBlend();
        }
        
        if (isMove)
        {
            SetDir();
            SetMove();
            SetCam();
        }
        if(skillMove)
        {
            SetSkillMove();
            SetCam();
        }
    }

    private void SetSkillMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Const.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if(camTrans!=null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1))+camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if(Mathf.Abs(currentBlend-targetBlend)<Const.AccelerSpeed*Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if(currentBlend>targetBlend)
        {
            currentBlend -= Const.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Const.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend", currentBlend);
    }

    public override void SetFX(string name, float destroyTime)
    {
        GameObject go;
        if(fxDic.TryGetValue(name,out go))
        {
            go.SetActive(true);
            TimerSvc.Instance.AddTimeTask((int tid) =>
            {
                go.SetActive(false);
            }, destroyTime);
        }
    }
}
