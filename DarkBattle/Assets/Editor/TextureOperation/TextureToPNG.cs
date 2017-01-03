using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextureToPNG : EditorWindow {

	private abstract class ParentConvert<T> where T : Object {

		protected T mTarget;
		protected bool mIsPng;

		public virtual void OnGUI(bool batch = true)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(10F);
			OnNotice();
			GUILayout.Space(8F);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space(10F);
			EditorGUILayout.BeginVertical("As TextArea", GUILayout.MinHeight(18));
			GUILayout.Space(10F);
			GUILayout.BeginHorizontal();
			GUILayout.Space(20f);
			OnTarget();
			GUILayout.Space(20F);
			GUILayout.EndHorizontal();

			if (mTarget != null && !mIsPng)
			{
				GUILayout.Space(10F);
				GUILayout.BeginHorizontal();
				GUILayout.Space(20F);
				OnButton();
				GUILayout.Space(20F);
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(10F);
			GUILayout.EndVertical();
			GUILayout.Space(10F);
			GUILayout.EndHorizontal();

			GUILayout.Space(10F);

			if (batch && DrawHeader("Batch"))
			{
				GUILayout.BeginHorizontal();
				GUILayout.Space(10F);
				EditorGUILayout.BeginVertical("As TextArea", GUILayout.MinHeight(18));
				GUILayout.Space(10F);
				GUILayout.BeginHorizontal();
				GUILayout.Space(10F);
				if (GUILayout.Button("Batch Convert"))
				{
					OnBatch();
				}
				GUILayout.Space(8F);
				GUILayout.EndHorizontal();
				GUILayout.Space(10F);
				GUILayout.EndVertical();
				GUILayout.Space(10F);
				GUILayout.EndHorizontal();
			}
		}

		protected abstract void OnNotice();

		protected abstract void OnTarget();

		protected abstract void OnButton();

		public abstract void OnBatch();
	}

	private class TextureConvert : ParentConvert<Texture2D> {

		private const string NOTICE = "Nothing to notice...";

		private string mTexPath;

		protected override void OnNotice()
		{
			EditorGUILayout.HelpBox(NOTICE, MessageType.Info);
		}

		protected override void OnTarget()
		{
			EditorGUIUtility.labelWidth = 110 - 10;
			Texture2D newTarget = EditorGUILayout.ObjectField("Unity Texture2D", mTarget, typeof(Texture2D), true) as Texture2D;
			if (newTarget != mTarget)
			{
				mTarget = newTarget;
				if (newTarget != null)
				{
					mTexPath = AssetDatabase.GetAssetPath(mTarget.GetInstanceID());
					mIsPng = IsPng(mTexPath);
				}
			}
		}

		protected override void OnButton()
		{
			if (!mIsPng)
			{
				if (GUILayout.Button("Convert"))
				{
					mIsPng = ConvertToPng(ref mTarget);
				}
			}
		}

		public override void OnBatch()
		{
			List<Texture2D> targetList = new List<Texture2D>();
			foreach (Texture2D target in Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets))
			{
				targetList.Add(target);
			}
			int total = targetList.Count;
			int succeeded = 0;
			for (int index = 0; index < total; index++)
			{
				ShowProgress("Converting Texture2D", index, total);
				string texPath = AssetDatabase.GetAssetPath(targetList[index].GetInstanceID());
				if (!IsPng(texPath))
				{
					Texture2D newTex = targetList[index];
					if (ConvertToPng(ref newTex))
					{
						succeeded++;
					}
				}
			}
			ShowProgress("Converting Texture2D", total, total);
			Debug.Log(string.Format("Converting finished, {0} of {1} has been converted.", succeeded, total));
		}
	}

	private TextureConvert mTextureConvert = new TextureConvert();

	private Vector2 mScroll = Vector2.zero;
	private Color mBackgroundColor = new Color(1, 0.9F, 0.9F);
	private Color mContentColor = new Color(0.85F, 1, 1);

	void OnGUI()
	{
		GUILayout.Space(5F);

		GUI.backgroundColor = mBackgroundColor;
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		GUILayout.BeginVertical("As TextArea");

		GUILayout.Space(5F);
		GUI.backgroundColor = mContentColor;
		mTextureConvert.OnGUI();
		GUILayout.Space(5F);

		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	private static bool DrawHeader(string title)
	{
		string key = title;
		bool state = EditorPrefs.GetBool(key, true);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(10F);

		Color oldColor = GUI.backgroundColor;
		title = "<b><size=11>" + title + "</size></b>";
		if (state)
		{
			title = "\u25BC " + title;
			GUI.backgroundColor = new Color(1, 1, 1);
		}
		else
		{
			title = "\u25BA " + title;
			GUI.backgroundColor = new Color(0.8F, 0.8F, 0.8F);
		}
		if (!GUILayout.Toggle(true, title, "DragTab", GUILayout.MinWidth(20f)))
			state = !state;
		GUI.backgroundColor = oldColor;

		GUILayout.Space(10F);
		EditorGUILayout.EndHorizontal();

		if (GUI.changed)
		{
			EditorPrefs.SetBool(key, state);
		}

		return state;
	}

	public static bool IsPng(Texture2D tex)
	{
		return IsPng(AssetDatabase.GetAssetPath(tex.GetInstanceID()));
	}

	public static bool IsPng(string texPath)
	{
		byte[] b = File.ReadAllBytes(texPath);
		return (b[0] & 0xff) == 0x89 && (b[1] & 0xff) == 0x50;
	}

	public static bool ConvertToPng(ref Texture2D tex)
	{
		string texPath = AssetDatabase.GetAssetPath(tex.GetInstanceID());
		if (File.Exists(texPath))
		{
			string newTexPath = Regex.Replace(texPath, "\\.[^\\.]+$", ".png");
			tex = ImportTexture(texPath, true, true, true);
			byte[] bytes = FormatRGB24(tex).EncodeToPNG();
			System.IO.File.WriteAllBytes(newTexPath, bytes);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			tex = ImportTexture(newTexPath, false, true, false);

			return true;
		}
		return false;
	}

	public static Texture2D FormatRGB24(Texture2D tex)
	{
		int width = tex.width;
		int height = tex.height;
		if (width > 0 && height > 0)
		{
			Color32[] pixels = tex.GetPixels32();
			Color32[] newPixels = new Color32[width * height];

			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					int index = y * width + x;
					newPixels[index] = pixels[index];
				}
			}

			Texture2D newTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
			newTex.SetPixels32(newPixels);
			newTex.Apply();

			return newTex;
		}
		return null;
	}

	static public Texture2D ImportTexture(string path, bool forInput, bool force, bool alphaTransparency)
	{
		if (!string.IsNullOrEmpty(path))
		{
			if (forInput)
			{
				if (!MakeTextureReadable(path, force))
				{
					return null;
				}
			}
			else if (!SetTextureImporter(path, force, alphaTransparency))
			{
				return null;
			}

			Texture2D tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			return tex;
		}
		return null;
	}

	public static bool MakeTextureReadable(string path, bool force)
	{
		if (string.IsNullOrEmpty(path))
			return false;
		TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
		if (ti == null)
			return false;

		TextureImporterSettings settings = new TextureImporterSettings();
		ti.ReadTextureSettings(settings);

		if (force || !settings.readable || settings.npotScale != TextureImporterNPOTScale.None
#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1
 || settings.alphaIsTransparency
#endif
)
		{
			settings.readable = true;
			settings.npotScale = TextureImporterNPOTScale.None;
			settings.textureFormat = TextureImporterFormat.AutomaticTruecolor;
#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1
			settings.alphaIsTransparency = false;
#endif
			ti.SetTextureSettings(settings);
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
		}
		return true;
	}

	static bool SetTextureImporter(string path, bool force, bool alphaTransparency)
	{
		if (string.IsNullOrEmpty(path))
			return false;
		TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
		if (ti == null)
			return false;

		TextureImporterSettings settings = new TextureImporterSettings();
		ti.ReadTextureSettings(settings);

		if (force ||
			settings.readable ||
			settings.maxTextureSize < 4096 ||
			settings.wrapMode != TextureWrapMode.Clamp ||
			settings.npotScale != TextureImporterNPOTScale.ToNearest)
		{
			settings.readable = false;
			settings.maxTextureSize = 4096;
			settings.wrapMode = TextureWrapMode.Clamp;
			settings.npotScale = TextureImporterNPOTScale.ToNearest;
			settings.textureFormat = TextureImporterFormat.AutomaticCompressed;

			settings.aniso = 1;
			settings.alphaIsTransparency = alphaTransparency;
			ti.SetTextureSettings(settings);
			ti.mipmapEnabled = false;
			ti.textureType = TextureImporterType.Advanced;
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
		}
		return true;
	}

	static public void ShowProgress(string name, float index, float count)
	{
		float value = index / count;
		if (value < 1)
		{
			EditorUtility.DisplayProgressBar(name, index + " of " + count + " has been done, Please wait...", value);
		}
		else
		{
			EditorUtility.ClearProgressBar();
		}
	}

	[@MenuItem("Window/Texture Operation/Texture To PNG")]
	private static void Window()
	{
		TextureToPNG window = GetWindow(typeof(TextureToPNG), false, "Texture To PNG") as TextureToPNG;
		window.minSize = new Vector2(320, 320);
		window.ShowTab();
	}
}
