using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour {
	//We make a static variable to our LoadingScreen instance
	static LoadingScript instance;
	//reference to gameobject with the static image 
	GameObject loadingScreenText;

	float barDisplay = 0;
	GUIStyle backgroundStyle = null;
	GUIStyle progressStyle = null;
	const int progressHeight = 20;
	const int progressWidth = 20;
	const int padding = 4;
	int currentProgressX = 0;

	public static Color HexToColor(string hex)
	{
		hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
		byte a = 255;//assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		//Only use alpha if the string has enough characters
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r,g,b,a);
	}

	private void InitStyles()
	{
		if( backgroundStyle == null )
		{
			backgroundStyle = new GUIStyle( GUI.skin.box );
			backgroundStyle.normal.background = MakeTex( 2, 2, HexToColor("#89b6d6"));
		}
		if( progressStyle == null )
		{
			progressStyle = new GUIStyle( GUI.skin.box );
			progressStyle.normal.background = MakeTex( 2, 2, HexToColor("#ffffff"));
		}
	}
	
	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}

	void OnGUI()
	{
		InitStyles ();
		// draw the background:
		RectTransform rt = (RectTransform)loadingScreenText.transform;

		GUI.BeginGroup (new Rect(rt.position.x - rt.rect.width/2, rt.position.y + rt.rect.height/2, rt.rect.width, progressHeight));
		GUI.Box (new Rect (0,0, rt.rect.width, progressHeight),"",backgroundStyle);

		// draw the filled-in part:
		currentProgressX = padding + (int)(((int)((float)rt.rect.width * barDisplay))%rt.rect.width) - progressWidth;

		GUI.Box (new Rect (currentProgressX, padding, progressWidth - padding*2, progressHeight-padding*2),"",progressStyle);

		GUI.EndGroup ();
	} 

	//function which executes on scene awake before the start function
	void Awake()
	{
		//destroy the already existing instance, if any
		if (instance)
		{
			Destroy(gameObject);
			return;
		}
		instance = this; 
		//find the ImageLS gameobject from the Hierarchy
		loadingScreenText = GameObject.Find("text_loading");

		Debug.Log("log thu phat");
		//loadingScreenText.SetActive(false);
		DontDestroyOnLoad(this);  //make this object persistent between scenes
	}
	
	void Update()
	{
		barDisplay = Time.time * 0.5f;
	}
}
