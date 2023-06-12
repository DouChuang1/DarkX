using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : MonoBehaviour {

	protected ResSvr ResSvr;
	protected AudioSvc AudioSvc;
    protected NetSvc NetSvc;
    protected TimerSvc TimerSvc;
	public virtual void InitSys()
	{
		ResSvr = ResSvr.instance;
		AudioSvc = AudioSvc.Instance;
        NetSvc = NetSvc.Instance;
        TimerSvc = TimerSvc.Instance;
	}
}
