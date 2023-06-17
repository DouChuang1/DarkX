using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    private Transform camTrans;
    public CharacterController ctrl;

    private Vector3 camOffset;
    public new Animator ani;
    float targetBlend;
    float currentBlend;
    public GameObject daggeratk1fx;

    public void Init()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;

        if(daggeratk1fx!=null)
        {
            fxDic.Add(daggeratk1fx.name, daggeratk1fx);
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
