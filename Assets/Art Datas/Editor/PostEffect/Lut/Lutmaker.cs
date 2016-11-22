using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Lutmaker : EditorWindow 
{
	string[] mSelStrings = new string[] {"8", "16", "32"};
	int[] mSelIntegers = new int[] {8, 16, 32};
	int mSelIndx = 0;

	[MenuItem ("Tools/LutMaker")]
	static void Init () 
	{
		// Get existing open window or if none, make a new one:
		Lutmaker maker = (Lutmaker)EditorWindow.GetWindow (typeof (Lutmaker));
		maker.Show();
	}

	void OnGUI () 
	{
		GUILayout.BeginHorizontal("box");
		GUILayout.Label ("精準度:", EditorStyles.boldLabel);
		GUILayout.Space (4F);
		mSelIndx = GUILayout.SelectionGrid (mSelIndx, mSelStrings, mSelStrings.Length);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal("box");
		if (GUILayout.Button ("產生Lut"))
			CreateLut ();
		GUILayout.EndHorizontal();
	}

	void CreateLut()
	{
		int size = mSelIntegers[mSelIndx];
		int maxSize = size - 1;
		int width = size * size;
		int height = size;


		Texture2D normalLut = new Texture2D( width, height, TextureFormat.RGB24, false, true );
		normalLut.name = "NormalLut";
		normalLut.anisoLevel = 1;
		normalLut.filterMode = FilterMode.Bilinear;

		Color32[] colors = new Color32[ width * height ];
		for ( int z = 0; z < size; z++ )
		{
			int zoffset = z * size;
			for ( int y = 0; y < size; y++ )
			{
				int yoffset = zoffset + y * width;
				for ( int x = 0; x < size; x++ )
				{
					float fr = x / ( float ) maxSize;
					float fg = y / ( float ) maxSize;
					float fb = z / ( float ) maxSize;
					byte br = ( byte ) ( fr * 255 );
					byte bg = ( byte ) ( fg * 255 );
					byte bb = ( byte ) ( fb * 255 );
					colors[ yoffset + x ] = new Color32( br, bg, bb, 255 );
				}
			}
		}
		normalLut.SetPixels32( colors );
		normalLut.Apply();


		byte[] bytes = normalLut.EncodeToPNG();

		FileStream file = System.IO.File.Open(Application.dataPath + "/"+"Lut.png",FileMode.Create);
		BinaryWriter binary= new BinaryWriter(file);
		binary.Write(bytes);
		file.Close();

		EditorUtility.DisplayDialog ("完成", "產生檔案:" + Application.dataPath + "/" + "Lut.png", "確定");
	}
}
