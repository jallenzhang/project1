using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextureModify : EditorWindow {

	private Color mBackgroundColor = new Color(1, 0.9F, 0.9F);
	private Color mContentColor = new Color(0.85F, 1, 1);

	private const string NOTICE = "Nothing to notice...";

	private bool mTrim;
	private bool mSymTrim;

	private int mExtend;
	private bool mResizeExtend = true;

	void OnGUI()
	{
		GUI.backgroundColor = mBackgroundColor;
		GUILayout.BeginVertical("As TextArea");
		GUI.backgroundColor = mContentColor;

		GUILayout.BeginHorizontal();
		{
			GUILayout.Space(10F);
			EditorGUILayout.HelpBox(NOTICE, MessageType.Info);
			GUILayout.Space(8F);
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		{
			GUILayout.Space(10F);
			EditorGUILayout.BeginHorizontal("As TextArea", GUILayout.MinHeight(18));
			{
				GUILayout.Space(10F);
				OnContentPaint();
				GUILayout.Space(10F);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10F);
		}
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}

	private void OnContentPaint()
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(10F);

			OnOptionsPaint();

			GUILayout.Space(5F);

			if (GUILayout.Button("Modify"))
			{
				OnBatchModify();
			}

			GUILayout.Space(10F);
		}
		GUILayout.EndVertical();
	}

	private void OnOptionsPaint()
	{
		NGUIEditorTools.SetLabelWidth(80f);

		GUILayout.BeginHorizontal();
		mTrim = EditorGUILayout.Toggle("Trim", mTrim, GUILayout.Width(100F));
		GUILayout.Label("Remove empty space");
		GUILayout.EndHorizontal();
		{
			GUI.enabled = mTrim;
			GUILayout.BeginHorizontal();
			GUILayout.Space(20F);
			mSymTrim = EditorGUILayout.Toggle("Symmetry", mSymTrim, GUILayout.Width(100F));
			GUILayout.Label("Symmetry Trim");
			GUILayout.EndHorizontal();
			GUI.enabled = true;
		}

		GUILayout.BeginHorizontal();
		mExtend = Mathf.Clamp(EditorGUILayout.IntField("Extend", mExtend, GUILayout.Width(100F)), 0, 8);
		GUILayout.Label(string.Format("{0} between sprites", mExtend == 1 ? "pixel" : "pixels"));
		GUILayout.EndHorizontal();
		{
			bool extend = mExtend > 0;
			GUI.enabled = extend;
			GUILayout.BeginHorizontal();
			GUILayout.Space(20F);
			mResizeExtend = EditorGUILayout.Toggle("Resize", mResizeExtend, GUILayout.Width(100F));
			GUILayout.Label("Resize Extend");
			GUILayout.EndHorizontal();
			GUI.enabled = true;
		}
	}

	private void OnBatchModify()
	{
		List<Texture2D> targetList = new List<Texture2D>();
		foreach (Texture2D target in Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets))
		{
			targetList.Add(target);
		}
		int total = targetList.Count;
		for (int index = 0; index < total; index++)
		{
			ShowProgress("Modifying Texture2D", index, total);
			string texPath = AssetDatabase.GetAssetPath(targetList[index].GetInstanceID());
			Texture2D tex = ImportTexture(texPath, true, true, true);
			Texture2D newTex = ModifyTexture(tex);
			if (newTex && newTex != tex)
			{
				byte[] bytes = newTex.EncodeToPNG();
				System.IO.File.WriteAllBytes(texPath, bytes);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
				ImportTexture(texPath, false, true, false);
			}
		}
		ShowProgress("Modifying Texture2D", total, total);
		Debug.Log(string.Format("Modifying finished, {0} has been modified.", total));
	}

	private Texture2D ModifyTexture(Texture2D tex)
	{
		if (mTrim)
		{
			tex = TrimTexture(tex);
		}
		if (mExtend > 0)
		{
			tex = ExtendTexture1(tex);
		}
		return tex;
	}

	private Texture2D TrimTexture(Texture2D tex)
	{
		if (!tex)
		{
			Debug.LogError("TrimException: Texture is null!");
			return null;
		}

		int width = tex.width;
		int height = tex.height;
		if (width <= 0 || height <= 0)
		{
			Debug.LogError("TrimException: Texture with {0}×{1} is empty!");
			return null;
		}

		int minX = width - 1;
		int minY = height - 1;
		int maxX = 0;
		int maxY = 0;
		Color32[] pixels = tex.GetPixels32();
		for (int y = 0; y < height; ++y)
		{
			for (int x = 0; x < width; ++x)
			{
				Color32 c = pixels[y * width + x];
				if (c.a != 0)
				{
					if (x < minX) minX = x;
					if (y < minY) minY = y;
					if (x > maxX) maxX = x;
					if (y > maxY) maxY = y;
				}
			}
		}

		int trimedWidth = maxX - minX + 1;
		int trimedHeight = maxY - minY + 1;
		if (trimedWidth <= 0 || trimedHeight <= 0)
		{
			Debug.LogError(string.Format("TrimException: Texture with {0}×{1} is empty!", width, height));
			return null;
		}

		if (mSymTrim)
		{
			int emptyWidth = Mathf.Min(minX, width - 1 - maxX);
			minX = emptyWidth;
			trimedWidth = width - emptyWidth - emptyWidth;
			int emptyHeight = Mathf.Min(minY, height - 1 - maxY);
			minY = emptyHeight;
			trimedHeight = height - emptyHeight - emptyHeight;
		}

		Color32[] trimedPixels = new Color32[trimedWidth * trimedHeight];
		for (int y = 0; y < trimedHeight; ++y)
		{
			for (int x = 0; x < trimedWidth; ++x)
			{
				int newIndex = y * trimedWidth + x;
				int oldIndex = (minY + y) * width + (minX + x);
				trimedPixels[newIndex] = pixels[oldIndex];
			}
		}
		Texture2D trimedTex = new Texture2D(trimedWidth, trimedHeight);
		trimedTex.SetPixels32(trimedPixels);
		trimedTex.Apply();

		return trimedTex;
	}

	private Texture2D ExtendTexture(Texture2D tex)
	{
		if (!tex)
		{
			Debug.LogError("ExtendTexture: Texture is null!");
			return null;
		}

		int width = tex.width;
		int height = tex.height;
		if (width <= 0 || height <= 0)
		{
			Debug.LogError("ExtendTexture: Texture with {0}×{1} is empty!");
			return null;
		}

		Color32[] pixels = tex.GetPixels32();
		int extendedWidth = width + mExtend + mExtend;
		int extendedHeight = height + mExtend + mExtend;
		Color32[] extendedPixels = new Color32[extendedWidth * extendedHeight];
		for (int y = 0; y < extendedHeight; ++y)
		{
			for (int x = 0; x < extendedWidth; ++x)
			{
				int newIndex = y * extendedWidth + x;
				int oldx = Mathf.Clamp(x - mExtend, 0, width - 1);
				int oldY = Mathf.Clamp(y - mExtend, 0, height - 1);
				int oldIndex = oldY * width + oldx;
				extendedPixels[newIndex] = pixels[oldIndex];
				if (oldx != x - mExtend || oldY != y - mExtend)
				{
					extendedPixels[newIndex].a = 1;
				}
			}
		}
		Texture2D extendedTex = new Texture2D(extendedWidth, extendedHeight);
		extendedTex.SetPixels32(extendedPixels);
		extendedTex.Apply();

		return extendedTex;

		//if (mTrim)
		//{
		//	xmin = 0;
		//	xmax = width;
		//	ymin = 0;
		//	ymax = height;
		//}
		//else
		//{
		//	xmin = width - 1;
		//	xmax = 0;
		//	ymin = height - 1;
		//	ymax = 0;
		//	for (int y = 0; y < height; ++y)
		//	{
		//		for (int x = 0; x < width; ++x)
		//		{
		//			Color32 c = pixels[y * width + x];
		//			if (c.a != 0)
		//			{
		//				if (y < ymin)
		//					ymin = y;
		//				if (y > ymax)
		//					ymax = y;
		//				if (x < xmin)
		//					xmin = x;
		//				if (x > xmax)
		//					xmax = x;
		//			}
		//		}
		//	}

		//	int trimedWidth = xmax - xmin + 1;
		//	int trimedHeight = ymax - ymin + 1;
		//	if (trimedWidth <= 0 || trimedHeight <= 0)
		//	{
		//		Debug.LogError(string.Format("ExtendTexture: Texture with {0}×{1} is empty!", width, height));
		//		return null;
		//	}
		//}

	}

	private Texture2D ExtendTexture1(Texture2D tex)
	{
		if (!tex)
		{
			Debug.LogError("ExtendTexture: Texture is null!");
			return null;
		}

		int width = tex.width;
		int height = tex.height;
		if (width <= 0 || height <= 0)
		{
			Debug.LogError("ExtendTexture: Texture with {0}×{1} is empty!");
			return null;
		}

		Color32[] pixels = tex.GetPixels32();
		int minX, minY;
		int maxX, maxY;
		if (mTrim)
		{
			if (!mResizeExtend)
			{
				return tex;
			}
			minX = 0;
			minY = 0;
			maxX = width;
			maxY = height;
		}
		else
		{
			minX = width - 1;
			minY = height - 1;
			maxX = 0;
			maxY = 0;
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					Color32 c = pixels[y * width + x];
					if (c.a != 0)
					{
						if (x < minX)
							minX = x;
						if (y < minY)
							minY = y;
						if (x > maxX)
							maxX = x;
						if (y > maxY)
							maxY = y;
					}
				}
			}

			int trimedWidth = maxX - minX + 1;
			int trimedHeight = maxY - minY + 1;
			if (trimedWidth <= 0 || trimedHeight <= 0)
			{
				Debug.LogError(string.Format("ExtendTexture: Texture with {0}×{1} is empty!", width, height));
				return null;
			}
			if (!mResizeExtend && trimedWidth == width && trimedHeight == height)
			{
				return tex;
			}
		}

		if (mResizeExtend)
		{
			int fillMinX = minX - mExtend;
			int fillMinY = minY - mExtend;
			int fillMaxX = maxX + mExtend;
			int fillMaxY = maxY + mExtend;

			if (fillMinX < 0 || fillMinY < 0 || fillMaxX >= width || fillMaxY >= height)
			{
				int extendWidth = Mathf.Max(-fillMinX, fillMaxX + 1 - width, 0);
				int extendHeight = Mathf.Max(-fillMinY, fillMaxY + 1 - height, 0);
				int tempWidth = width + extendWidth + extendWidth;
				int tempHeight = height + extendHeight + extendHeight;
				Color32[] extendedPixels = new Color32[tempWidth * tempHeight];
				for (int y = 0; y < tempHeight; ++y)
				{
					for (int x = 0; x < tempWidth; ++x)
					{
						int newIndex = y * tempWidth + x;
						int oldx = Mathf.Clamp(x - extendWidth, 0, width - 1);
						int oldY = Mathf.Clamp(y - extendHeight, 0, height - 1);
						int oldIndex = oldY * width + oldx;
						extendedPixels[newIndex] = pixels[oldIndex];
						if (oldx != x - extendWidth || oldY != y - extendHeight)
						{
							extendedPixels[newIndex].a = 0;
						}
					}
				}
				pixels = extendedPixels;
				width = tempWidth;
				height = tempHeight;
				minX += extendWidth;
				minY += extendHeight;
				maxX += extendWidth;
				maxY += extendHeight;
			}
		}

		for (int y = 0; y < height; ++y)
		{
			for (int x = 0; x < width; ++x)
			{
				int newIndex = y * width + x;
				if (((x >= minX - mExtend && x < minX) || (x > maxX && x <= maxX + mExtend)) &&
					((y >= minY - mExtend && y < minY) || (y > maxY && y <= maxY + mExtend)))
				{
					pixels[newIndex].a = 1;
				}
			}
		}

		Texture2D extendedTex = new Texture2D(width, height);
		extendedTex.SetPixels32(pixels);
		extendedTex.Apply();
		return extendedTex;
	}

	private Texture2D ImportTexture(string path, bool forInput, bool force, bool alphaTransparency)
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

	private bool MakeTextureReadable(string path, bool force)
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

	private bool SetTextureImporter(string path, bool force, bool alphaTransparency)
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

	[@MenuItem("Window/Texture Operation/Texture Modify")]
	private static void Window()
	{
		TextureModify window = GetWindow(typeof(TextureModify), false, "Texture Modify") as TextureModify;
		window.minSize = new Vector2(320, 320);
		window.ShowTab();
	}
}
