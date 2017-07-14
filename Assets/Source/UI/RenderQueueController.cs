using UnityEngine;
using System.Collections;

public class RenderQueueController : MonoBehaviour {

	public bool initRQ = false;
    public int incrementRQ = 0;
	public int renderQ = 2000;
	public UIWidget widget;

	protected Renderer[] renders;

	public static RenderQueueController AddIfNull(GameObject go, int renderqueue )
    {
        RenderQueueController rqm = null;
        if (go.GetComponent<RenderQueueController>() == null ){
            rqm = go.AddComponent<RenderQueueController>();
        }
        rqm.SetRenderQueue(renderqueue );
        return rqm;
    }

    void Awake()
    {
        EventNotificationCenter.GetInstance().AddListener(BrowdCastEvent.AJUST_TOP_DEPTH, UpdateRenderQueue);
    }

    void OnDestroy()
    {
        EventNotificationCenter.GetInstance().RemoveListener(BrowdCastEvent.AJUST_TOP_DEPTH, UpdateRenderQueue);
    }

	void Start()
    {
        if(initRQ)
        {
            SetRenderQueue(renderQ);
        }else
        {
            StartCoroutine(NextFrame()); 
        }
    }

    IEnumerator NextFrame()
    {
        yield return null;
        yield return 1;
        yield return 1;
        UpdateRenderQueue();
        yield return null;
    }

	public void SetRenderQueue(int renderqueue)
	{
		renderQ = renderqueue;
        renders = transform.GetComponentsInChildren<Renderer>();
        foreach(Renderer render in renders )
        {
            render.material.renderQueue = renderQ;
        }
	}

	public void ChangeRenderQueue(int renderqueue)
    {
        renders = transform.GetComponentsInChildren<Renderer>();
        foreach(Renderer render in renders ){
            render.material.renderQueue += renderqueue;
        }
    }

    public void UpdateRenderQueue()
    {
        if(widget != null && widget.drawCall.widgetCount > 0 )
        {
            SetRenderQueue(widget.drawCall.renderQueue + incrementRQ);
            Debug.Log("--- " + widget.drawCall.renderQueue);
        }
    }

}
