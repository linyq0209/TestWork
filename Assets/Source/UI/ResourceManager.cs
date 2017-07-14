using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : PSingleton<ResourceManager>
{
    public const string PROP_PATH_NAME = "Prefab/";
    public const string ANIMATION_CLIP_NAME = "Animation/";
   
    // cache:
    protected Dictionary<string, GameObject> abObject = new Dictionary<string, GameObject>(); 

    public AsyncOperation LoadSceneAsync(string name, bool isFromAB)
    {
       
        AsyncOperation isDone = Application.LoadLevelAsync(name );
        return isDone;
    }


    public void LoadScene(string name)
    {
        Application.LoadLevel(name);
    }


     public T GetAsset<T>(string name) where T : UnityEngine.Object
    {
        T ret = Resources.Load(name) as T;
        return ret;
    }

    public IEnumerator GetAssetAsyn<T>(string name, Action<T> callback) where T: UnityEngine.Object
    {
       
        var request = Resources.LoadAsync(name, typeof(T));
        while(!request.isDone)
        {
            yield return null;
        }
        if(callback != null)
        {
            callback(request.asset as T);
        }
        yield return null;
    }

    public UIAtlas GetAtals(string name)
    {
        var obj = GetPrefab(name);
        if(obj == null)
        {
            PDebug.Break ();
            PDebug.Log ("<size=20> : error: " + name + "</size>");
        }
        return obj.GetComponent<UIAtlas>();
    }

    public UIFont GetFont(string name)
    {
        var obj = GetPrefab(name);
		
        return obj.GetComponent<UIFont>();
    }


    public GameObject GetPrefab(string name)
    {
        name = PROP_PATH_NAME + name;
        return GetAsset<GameObject>(name);
    }

    public IEnumerator GetPrefabAsyn(string name, Action<GameObject> callback)
    {
        name = PROP_PATH_NAME + name;
       yield return GetAssetAsyn<GameObject>(name, callback);
    }


    public IEnumerator GetAudioAsyn(string name, Action<AudioClip> callback)
    {
        name = "Audio/" + name;
        yield return GetAssetAsyn<AudioClip>(name, callback);
    }


    // public GameObject GetABObject(string filename)
    // {
    //     GameObject obj;
    //     if(abObject.TryGetValue(filename, out obj))
    //     {
    //         return obj;
    //     }
    //     string assetName;
    //     string abName;
    //     GetAssetName(filename, out assetName, out abName);
    //     AssetBundle bundle = FileUtility.GetAssetBundle(abName);
    //     obj = bundle.LoadAsset<GameObject>(assetName);
    //     abObject[filename] = obj;
    //     bundle.Unload(false);
    //    return obj;
    // }


    public static bool GetAssetName(string name, out string assetName , out string abName)
    {
        int index = name.LastIndexOf('@');
        assetName = name;
        bool ret = index != -1 && index != 0 && index != (name.Length-1);
        if(ret)
        {
            //assetName = name.Substring(index);    
            abName = name.Substring(0, index);
        }else
        {
            abName = name;
        }
        Debug.Log(assetName + "--- " + abName);
        return ret;
    } 


}
