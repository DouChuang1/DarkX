using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : MonoBehaviour {

	protected ResSvr ResSvr;
	protected AudioSvc AudioSvc;

	public virtual void InitSys()
	{
		ResSvr = ResSvr.instance;
		AudioSvc = AudioSvc.Instance;
	}
}
