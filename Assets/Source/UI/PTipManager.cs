using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PTipManager : PMonoSingleton<PTipManager> 
{
	public delegate IEnumerator CoDelegate();
	protected class TipWrap
	{
		public GameObject go;
		public CoDelegate del;

		public TipWrap(GameObject go, CoDelegate del)
		{
			this.go = go;
			this.del = del;
		}
	}

	protected Queue<TipWrap> mCurObjQueue = new Queue<TipWrap>();

	public float tipInterval = 0.5f;

	protected override void Init()
	{	
		StartCoroutine(TipImpl());
	}

	public void OnTip(GameObject tipObj, CoDelegate startDel )
	{
		tipObj.SetActive(false);
		mCurObjQueue.Enqueue(new TipWrap(tipObj, startDel) );
	}

	protected IEnumerator TipImpl()
	{
		while(true)
		{
			if (mCurObjQueue.Count>0)
			{
				TipWrap tipWrap = mCurObjQueue.Dequeue();
                tipWrap.go.SetActive(true );
                yield return StartCoroutine(tipWrap.del() );
			}else
			{
				yield return 1;
			}
			
		}
	}

	public void StopTip()
	{
		mCurObjQueue.Clear();
	}

}