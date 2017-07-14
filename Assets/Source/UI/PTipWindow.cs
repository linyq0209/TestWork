using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class PTipWindow : MonoBehaviour 
{
	private const string WEAK_TIP_NAME = "WeakTipPanel";
	public UILabel labelSprite;
    public GameObject effectObj;
	protected UIPanel panel;
	protected bool isShowImmediately_ = false;
	protected Action<GameObject> callback_;
	protected Sequence action_;
	public float mUpTime = 2f;
	public float mFadeTime = 0.3f; 

    public static GameObject Create(String content, bool isShowImmediately = true,bool isShowEffect = true)
    {
        return Create(content, null,isShowImmediately ,isShowEffect);
    }


    public static GameObject Create(String content, Action<GameObject>callback,bool isShowEffect = true)
    {
        return Create(content, callback, true ,isShowEffect);
    }

    public static GameObject Create(
    	string content,  
    	Action<GameObject> callback, 
    	bool isShowImmediately,
        bool isShowEffect = true)
    {
        GameObject gameObj = ResourceManager.GetInstance().GetPrefab(WEAK_TIP_NAME);
        gameObj = GameObject.Instantiate(gameObj);
        PTipWindow controller = gameObj.GetComponent<PTipWindow>();
        controller.Init(content, callback, isShowImmediately, isShowEffect);
        return gameObj;
    }

    void Awake()
    {
    	Vector3 vector3 = transform.localEulerAngles;
        vector3.x = 0;
        transform.localEulerAngles = vector3;
        PUIManager.GetInstance().CreateTopLayer(transform);
        Vector3 localPos = transform.localPosition;
        localPos.z = -5000;
        transform.localPosition = localPos;
    }

    private void Init(String content, Action<GameObject> callback, bool isShowImmediately,bool isShowEffect = true)
    {
        isShowImmediately_ = isShowImmediately;
        effectObj.SetActive(isShowEffect);
        callback_ = callback;
        labelSprite.text = content;
        panel = gameObject.GetComponent<UIPanel>();
        panel.alpha = 0;
        
        if (!isShowImmediately_)
        {
            PTipManager.GetInstance().OnTip(gameObject, PlayAnimation);
        }
        else
        {
            StartCoroutine(PlayAnimation());
        }
        
    }

    protected void StartTip( )
    {
        
    }

    protected IEnumerator PlayAnimation()
    {
        yield return 2;
        Vector3 vector3 = transform.localEulerAngles;
        vector3.x = 0;
        transform.localEulerAngles = vector3;

        panel.alpha = 1;
        Vector3 originPos = transform.localPosition;

        Sequence playAction = DOTween.Sequence();
        playAction.OnComplete(End);
        var moveTo = transform.DOLocalMoveY(originPos.y + 175, mUpTime);
        playAction.Append(moveTo);
        var fade = DOTween.To(()=>panel.alpha ,alpha=> panel.alpha=alpha,  0, mFadeTime);
			playAction.Append(fade);
      

        playAction.Play();
        action_ = playAction;

        yield return new WaitForSeconds(0.5f );
        if (callback_ != null)
        {
            callback_(gameObject);
        }
    }

    public void End()
    {
        transform.parent = null;
        GameObject.Destroy(gameObject);
        action_ = null;
    }

    public void OnDestroy()
    {
        //to protect when the scene switched
        if (action_ != null)
        {
            action_.Kill();
            action_ = null;
        }

    }

    [ContextMenu("test")]
    public void Test()
    {
    	 Init("恭喜发财！", null, false );
    }
}
