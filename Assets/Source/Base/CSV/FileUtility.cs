using System.IO;
using UnityEngine;
using System.Text;
using System;
using System.Collections;

public class FileUtility 
{
	protected const string CONIFG_PATH_FORMAT = "Config/{0}";
	protected static string sStreamRootPath = null;
	protected static string sStreamAssetBundlePath = null;
	protected static string sStreamABPlatformPath = null;
	protected static string sWriteRootPath = null;

	public static string GetWriteRootPath()
	{
		if(string.IsNullOrEmpty(sWriteRootPath))
		{
			#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
				sWriteRootPath = Application.dataPath + "/..";
			#else
				sWriteRootPath = Application.persistentDataPath;
			#endif
		}
		return sWriteRootPath;
	}

	public static string GetStreamRootPath()
	{
		if(string.IsNullOrEmpty(sStreamRootPath))
		{
			sStreamRootPath = Application.streamingAssetsPath;
		}
		return sStreamRootPath;
	}

	public static string GetStreamAssetBundlePath()
	{
		if(string.IsNullOrEmpty(sStreamAssetBundlePath))
		{
			sStreamAssetBundlePath = Path.Combine(GetStreamRootPath(), "AssetBundle");
		}
		return sStreamAssetBundlePath;
	}

	public static string GetStreamABPlatformPath()
	{
		if(string.IsNullOrEmpty(sStreamABPlatformPath))
		{
			string platform = "";
			if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				platform = "IOS";
			}else if(Application.platform == RuntimePlatform.Android)
			{
				platform = "Android";
			}
			sStreamABPlatformPath = GetStreamAssetBundlePath();
			if(!string.IsNullOrEmpty(platform))
			{
				sStreamABPlatformPath = Path.Combine(sStreamABPlatformPath, platform);
			}
			 
		}
		return sStreamABPlatformPath;
	}

	public static string GetWriteablePath(string fileName)
	{
		return Path.Combine(GetWriteRootPath(), fileName);
	}

	public static string LoadWriteableFile(string fileName)
	{
		string path =  GetWriteablePath(fileName);
		try
		{
			if(File.Exists(path))
			{	
				return File.ReadAllText(path);
			}
		}
		catch (System.Exception ex)
		{
			
			Debug.LogError("load writeable file error: "+ path + ex.ToString());
		}
		
		return "";
	}

	public static void SaveData(string content, string fileName)
	{
		string path =  GetWriteablePath(fileName);
		try
		{
			string dir = Path.GetDirectoryName(path);
        	if (!Directory.Exists(dir))
			{
            	Directory.CreateDirectory(dir);
			}
			File.WriteAllText(path, content);
		}
		catch (System.Exception)
		{
			
			Debug.LogError("write writeable file error: "+ path);
		}
	}

    /// <summary>
    /// 获取文件路径
    /// </summary>	
    public static string GetWritableWWWPath(string fileName)
    {
        string writePath = "";
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        	writePath = "file:///" + Application.dataPath + "/../" + fileName;
		#else
        	writePath = "file://" + Application.persistentDataPath + "/" + fileName;
		#endif
        return writePath;
    }

    public static string GetStreamingWWWAssetPath(string sourcefilename)
    {
		string sourcePath = Path.Combine(Application.streamingAssetsPath, sourcefilename);
		#if UNITY_ANDROID || UNITY_WEBPLAYER
		#else
			sourcePath = "file:///" + sourcePath;
		#endif
        return sourcePath;
    }

    public static string GetStreamingAssetPath(string sourcefilename)
    {
        return Path.Combine(GetStreamRootPath(), sourcefilename);
    }


	public static byte[] GetRelativeFileBytes(string relativePath)
	{
		PDebug.Log("Load from Resouces! " + relativePath);
		TextAsset data = Resources.Load(relativePath) as TextAsset;
		byte[] bytes = null;
		if(data != null )
		{
			bytes = data.bytes;
		}
		return bytes;
	}

	public static string GetRelativeFileText(string relativePath)
	{
		TextAsset data = Resources.Load(relativePath) as TextAsset;
		string text = null;
		if(data != null )
		{
			text = data.text;
		}
		return text;
	}

	public static CsvFile LoadCsvFile(string configName, bool trimColum = true, Encoding encoding = null )
	{
		
		Stream stream = GetConfigStream(configName, ".csv");
		if(stream != null)
		{
			CsvFile file = new CsvFile();
			file.Populate(stream, encoding == null ?  Encoding.UTF8 : encoding, true, trimColum);
			return file;
		}else
		{
			Debug.LogError("load config failure: " + configName);
		}
		return null;

	}

	public static string GetConfigText(string configName, string suffix)
	{
		string path = string.Format(CONIFG_PATH_FORMAT, configName);
		string tmp = GetWritableWWWPath(path) + suffix;
		string text = null;
		if(File.Exists(tmp))
		{
			return File.ReadAllText(tmp);
		}
		tmp = GetStreamingAssetPath(path) + suffix;
		text = GetStreamFileText(tmp);
		if(string.IsNullOrEmpty(text))
		{	
			text = GetRelativeFileText(path);
		}
		return text;
	}

	public static byte[] GetConfigByte(string configName, string suffix)
	{
		string path = string.Format(CONIFG_PATH_FORMAT, configName);
		string tmp = GetWritableWWWPath(path) + suffix;
		byte[] bytes = null;
		if(File.Exists(tmp))
		{
			return File.ReadAllBytes(tmp);
		}
		tmp = GetStreamingAssetPath(path) + suffix;
		bytes = GetStreamFileBytes(tmp);
		if(bytes == null)
		{
			bytes = GetRelativeFileBytes(path);
		}
		return bytes;
	}

	public static Stream GetConfigStream(string configName, string suffix)
	{
		byte[] tmp = GetConfigByte(configName, suffix);
		if(tmp != null)
		{
			return new MemoryStream(tmp);
		}
		return null;
	}
	

	public static byte[] GetFileBytes(string fullPath)
	{
		if(!File.Exists(fullPath))
		{
			return null;
		}
		return GetStreamFileBytes(fullPath);
	}

	public static string GetFileText(string fullPath)
	{
		if(!File.Exists(fullPath))
		{
			return null;
		}
		return GetStreamFileText(fullPath);
	}

	public static Stream GetFileStream(string fullPath)
	{
		if(!File.Exists(fullPath))
		{
			return null;
		}
		return GetStreamFileStream(fullPath);
	}

	public static byte[] GetStreamFileBytes(string fullPath)
	{
		if(!File.Exists(fullPath))
		{
			return null;
		}
		try
		{
			Debug.Log("Read From Stream path");
			if(fullPath.Contains("://"))
			{
				WWW www  = new WWW(fullPath);
				while(!www.isDone){};
				return www.bytes;
			}
			return File.ReadAllBytes(fullPath);
		}
		catch (System.Exception e)
		{
			string str = string.Format("FileUtil.GetStreamingFileBytes: Read File {0:s} Error, {1:s}", fullPath, e.Message);
            Debug.LogError(str);
			return null;
		}
	}

	public static string GetStreamFileText(string fullPath)
	{
		try
		{
			if(fullPath.Contains("://"))
			{
				WWW www  = new WWW(fullPath);
				while(!www.isDone){};
				return www.text;
			}
			return File.ReadAllText(fullPath);
		}
		catch (System.Exception e)
		{
			string str = string.Format("FileUtil.GetStreamFileText: Read File {0:s} Error, {1:s}", fullPath, e.Message);
            Debug.LogError(str);
			return null;
		}
	}


	public static Stream GetStreamFileStream(string fullPath)
	{
		try
		{
			if(fullPath.Contains("://"))
			{
				WWW www  = new WWW(fullPath);
				while(!www.isDone){};
				return new MemoryStream(www.bytes); 
			}
			return File.OpenRead(fullPath);
		}
		catch (System.Exception e)
		{
			string str = string.Format("FileUtil.GetStreamFileText: Read File {0:s} Error, {1:s}", fullPath, e.Message);
            Debug.LogError(str);
			return null;
		}
	}
	

	public static AssetBundle GetAssetBundle(string fileName)
	{
		
		var path = Path.Combine(GetStreamABPlatformPath(), fileName); //(GetStreamAssetBundlePath(),fileName);
		// if(!File.Exists(path))
		// {
		// 	return null;
		// }
		var ab = AssetBundle.LoadFromFile(path);
		return ab;
	}

	public static IEnumerator GetAssetBundleAsy(string fileName, Action<AssetBundle> callback)
	{
		var path = Path.Combine(GetStreamABPlatformPath(), fileName);//(GetStreamAssetBundlePath(),fileName);
		var result = AssetBundle.LoadFromFileAsync(path);
		yield return result;
		callback(result.assetBundle);
		
	}
	
}
