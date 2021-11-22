
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;




public class screenCapture: MonoBehaviour  {
	
	// 截图尺寸
	public enum CaptureSize {
		CameraSize,
		ScreenResolution,
		FixedSize
	}


	// 目标摄像机
	public Camera targetCamera = null;
	public Camera uiCamera = null;
	public Camera uiCamera2 = null;

    public float csWidthMax = 640.0f;
    public float csHeightMax = 480.0f;
    // 截图尺寸
    //public CaptureSize captureSize = CaptureSize.CameraSize;
    // 像素尺寸
    private Vector2 pixelSize;
	private int width = 0;
	private int height = 0;

    
    private const string csFilePath = "D:/stone_maze/";
    // 保存路径
    //private string savePath = "StreamingAssets/";
    // 文件名称
    public string fileName = "cameraCapture";
//	public int index =0;
	public bool isPng = true;
	public int type = 1;
	public bool mipmap = false;
	public bool linear = true;
	public int depth = 24;
	public int antiAliasing = 8;
	// Use this for initialization
	void Start () {
	//	index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.E)){
				if(targetCamera == null)
					targetCamera = GetComponent<Camera>();
				if(targetCamera != null)
				{
					pixelSize = new Vector2(csWidthMax, csHeightMax);
					saveCapture ();
				}
				else{
					Debug.LogError("no find targetCamera");
				}
			}
		#endif
	}
	//aram name="camera">目标相机</param>
		/// <param name="width">宽度</param>
		/// <param name="height">高度</param>
	public void saveCapture() {
		Vector2 size = pixelSize;
		//if (captureSize == CaptureSize.CameraSize) {
		//	size = new Vector2(targetCamera.pixelWidth, targetCamera.pixelHeight);
		//} else if (captureSize == CaptureSize.ScreenResolution) {
		//	size = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
		//}
		//index = index + 1;
		string extname = "";
		if (isPng)
			extname = ".png";
		else
			extname = ".jpg";
		var date = string.Format("{0:00}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		var time = string.Format("{0:00}{1:00}{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
		string outputFileName = "";
		outputFileName = string.Format("_{0}_{1}",   date, time);

		width = (int)size.x;
		height = (int)size.y;
		//string path = "D:/lk/" + savePath + outputFileName + extname;
		string path = csFilePath + fileName + outputFileName + extname;
		saveTexture(path, capture());
		//saveTexture(path, CaptureScreen((int)size.x, (int)size.y));
	}

	/// <summary> 相机截图 </summary>
	/// <param name="camera">目标相机</param>
	//public Texture2D capture(Camera camera) {
	//	return capture(camera, Screen.width, Screen.height);
		//return CaptureScreen(Screen.width, Screen.height);
	//}

	/// <summary> 相机截图 </summary>
	/// <param name="camera">目标相机</param>
	/// <param name="width">宽度</param>
	/// <param name="height">高度</param>
	public Texture2D capture() {
		
		RenderTexture rt = new RenderTexture(width, height, depth);
		rt.depth = depth;
		rt.antiAliasing = antiAliasing;
		targetCamera.targetTexture = rt;
		targetCamera.RenderDontRestore();

		if (uiCamera != null) {
			uiCamera.targetTexture = rt;
			uiCamera.RenderDontRestore ();
		}
			
		if (uiCamera2 != null) {
			uiCamera2.targetTexture = rt;
			uiCamera2.RenderDontRestore ();
		}

		RenderTexture.active = rt;
		Texture2D texture = null;
		if(type == 0)
			texture = new Texture2D(width, height, TextureFormat.ARGB32, mipmap, linear);
		else if(type == 1)
			texture = new Texture2D(width, height, TextureFormat.RGB24, mipmap, linear);
		else if(type == 2)
			texture = new Texture2D(width, height, TextureFormat.RGBA32, mipmap, linear);
		else if(type == 3)
			texture = new Texture2D(width, height, TextureFormat.Alpha8, mipmap, linear);
		else if(type == 4)
			texture = new Texture2D(width, height, TextureFormat.RGBAFloat, mipmap, linear);
		else// if(type == 5)
			texture = new Texture2D(width, height, TextureFormat.DXT5, mipmap, linear);
		
		Rect rect = new Rect(0, 0, width, height);
		texture.ReadPixels(rect, 0, 0);
		texture.filterMode = FilterMode.Point;
		texture.Apply();
		targetCamera.targetTexture = null;

		if(uiCamera != null)
			uiCamera.targetTexture = null;
		
		if(uiCamera2 != null)
			uiCamera2.targetTexture = null;
		RenderTexture.active = null;
		Destroy(rt);
		return texture;


	}


	public Texture2D CaptureScreen()
	{
		Rect rect = new Rect(0, 0, width, height);
		Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, mipmap, linear);

		screenShot.ReadPixels(rect, 0, 0);

		screenShot.Apply();


		return screenShot;
	}

	/// <summary> 保存贴图 </summary>
	/// <param name="path">保存路径</param>
	/// <param name="texture">Texture2D</param>
	public void saveTexture(string path, Texture2D texture) {
		if(isPng)
			File.WriteAllBytes(path, texture.EncodeToPNG());
		else
			File.WriteAllBytes(path, texture.EncodeToJPG());
		#if UNITY_EDITOR
		Debug.Log("已保存截图到:" + path);
		#endif
	}
}

