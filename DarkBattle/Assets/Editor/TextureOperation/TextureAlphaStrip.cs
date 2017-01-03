using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextureAlphaStrip : EditorWindow {

	public enum TextureType {
		TT_PNG,
		TT_TGA
	}

	private static Regex mPngRegex = new Regex("\\.png$", RegexOptions.IgnoreCase);

	private abstract class ParentStrip<T> where T : Object {

		protected T mTarget;
		protected bool mOperable;

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

			if (mTarget != null && mOperable)
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
				if (GUILayout.Button("Batch Strip"))
				{
					OnBatch(true);
				}
				if (GUILayout.Button("Batch Merge"))
				{
					OnBatch(false);
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

		public abstract void OnBatch(bool strip);
	}

	private class Texture2DStrip : ParentStrip<Texture2D> {

		private const string NOTICE = "if texture has pass alpha, strip the alpha pass to "
								+ "another texture.\nelse if texture has no pass alpha, "
								+ "merge the other texture to the alpha pass of this texture.";

		private string mTexPath;
		private bool mAlphaPassExist;
		private TextureType mType;

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
					if (mOperable = IsMatch(mTexPath))
					{
						mType = TextureType.TT_PNG;
						mAlphaPassExist = AlphaPassExist(mTexPath);
					}
					else if (mOperable = IsMatch(mTexPath, TextureType.TT_TGA))
					{
						mType = TextureType.TT_TGA;
						mAlphaPassExist = AlphaPassExist(mTexPath, TextureType.TT_TGA);
					}
				}
			}
		}

		protected override void OnButton()
		{
			if (mAlphaPassExist)
			{
				if (GUILayout.Button("Strip"))
				{
					if (mType == TextureType.TT_PNG)
					{
						mAlphaPassExist = !StripTextureAlpha(mTexPath);
					}
					else if (mType == TextureType.TT_TGA)
					{
						mAlphaPassExist = !StripTgaTextureAlpha(mTexPath);
					}
				}
			}
			else
			{
				if (GUILayout.Button("Merge"))
				{
					if (mType == TextureType.TT_PNG)
					{
						mAlphaPassExist = MergeTextureAlpha(mTexPath);
					}
					else if (mType == TextureType.TT_TGA)
					{
						mAlphaPassExist = MergeTgaTextureAlpha(mTexPath);
					}
				}
			}
		}

		public override void OnBatch(bool strip)
		{
			List<Texture2D> targetList = new List<Texture2D>();
			foreach (Texture2D target in Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets))
			{
				targetList.Add(target);
			}
			int total = targetList.Count;
			int succeeded = 0;
			if (strip)
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Stripping Texture2D", index, total);
					string texPath = AssetDatabase.GetAssetPath(targetList[index].GetInstanceID());
					if (IsMatch(texPath))
					{
						if (AlphaPassExist(texPath) && StripTextureAlpha(texPath))
						{
							succeeded++;
						}
					}
					else if (IsMatch(texPath, TextureType.TT_TGA))
					{
						if (AlphaPassExist(texPath, TextureType.TT_TGA) && StripTgaTextureAlpha(texPath))
						{
							succeeded++;
						}
					}
				}
				ShowProgress("Stripping Texture2D", total, total);
				Debug.Log(string.Format("Stripping finished, {0} of {1} has been stripped.", succeeded, total));
			}
			else
			{
				List<string> alphaPathList = new List<string>();
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Merging Texture2D", index, total);
					string texPath = AssetDatabase.GetAssetPath(targetList[index].GetInstanceID());
					if (!alphaPathList.Contains(texPath))
					{
						if (IsMatch(texPath))
						{
							if (!AlphaPassExist(texPath) && MergeTextureAlpha(texPath))
							{
								alphaPathList.Add(mPngRegex.Replace(texPath, "_alpha.png"));
								succeeded++;
							}
						}
						else if (IsMatch(texPath, TextureType.TT_TGA))
						{
							if (!AlphaPassExist(texPath, TextureType.TT_TGA) && MergeTgaTextureAlpha(texPath))
							{
								alphaPathList.Add(mPngRegex.Replace(texPath, "_alpha.png"));
								succeeded++;
							}
						}
					}
				}
				ShowProgress("Merging Texture2D", total, total);
				Debug.Log(string.Format("Merging finished, {0} of {1} has been merged.", succeeded, total));
			}
		}
	}

	private Texture2DStrip mTexture2DStrip = new Texture2DStrip();

	private class UITextureStrip : ParentStrip<UITexture> {

		private const string NOTICE = "if texture has pass alpha, strip the alpha pass to "
								+ "another texture.\nelse if texture has no pass alpha, "
								+ "merge the other texture to the alpha pass of this texture.";

		private Texture mTex;
		private string mTexPath;
		private bool mAlphaPassExist;

		protected override void OnNotice()
		{
			EditorGUILayout.HelpBox(NOTICE, MessageType.Info);
		}

		protected override void OnTarget()
		{
			EditorGUIUtility.labelWidth = 90 + 10;
			UITexture newTarget = EditorGUILayout.ObjectField("NGUI Texture", mTarget, typeof(UITexture), true) as UITexture;
			if (newTarget == mTarget)
			{
				if (newTarget == null || newTarget.mainTexture == mTex)
				{
					return;
				}
			}
			else
			{
				mTarget = newTarget;
			}
			if (mTarget != null)
			{
				mTex = mTarget.mainTexture;
				if (mTex != null)
				{
					mTexPath = AssetDatabase.GetAssetPath(mTex.GetInstanceID());
					if (mOperable = IsMatch(mTexPath))
					{
						mAlphaPassExist = AlphaPassExist(mTexPath);
					}
				}
			}
		}

		protected override void OnButton()
		{
			if (mAlphaPassExist)
			{
				if (mTarget.alphaTexture == null && GUILayout.Button("Strip"))
				{
					string alphaTexPath = mPngRegex.Replace(mTexPath, "_alpha.png");
					mAlphaPassExist = !StripTextureAlpha(mTexPath, alphaTexPath);
					if (!mAlphaPassExist)
					{
						mTarget.alphaTexture = AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D;
					}
				}
			}
			else
			{
				if (GUILayout.Button("Merge"))
				{
					mAlphaPassExist = MergeTextureAlpha(mTex as Texture2D, mTarget.alphaTexture as Texture2D);
					if (mAlphaPassExist)
					{
						mTarget.alphaTexture = null;
					}
				}
			}
		}

		public override void OnBatch(bool strip)
		{
			List<UITexture> targetList = new List<UITexture>();
			foreach (UITexture target in Selection.GetFiltered(typeof(UITexture), SelectionMode.Deep))
			{
				targetList.Add(target);
			}
			foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
			{
				if (obj is GameObject)
				{
					UITexture target = (obj as GameObject).GetComponent<UITexture>();
					if (target != null && !targetList.Contains(target))
					{
						targetList.Add(target);
					}
				}
			}
			int total = targetList.Count;
			int succeeded = 0;
			if (strip)
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Stripping UITexture", index, total);
					if (targetList[index].mainTexture != null && targetList[index].alphaTexture == null)
					{
						string texPath = AssetDatabase.GetAssetPath(targetList[index].mainTexture.GetInstanceID());
						if (IsMatch(texPath) && AlphaPassExist(texPath))
						{
							string alphaTexPath = mPngRegex.Replace(texPath, "_alpha.png");
							if (StripTextureAlpha(texPath, alphaTexPath))
							{
								targetList[index].alphaTexture = AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D;

								succeeded++;
							}
						}
					}
				}
				ShowProgress("Stripping UITexture", total, total);
				Debug.Log(string.Format("Stripping finished, {0} of {1} has been stripped.", succeeded, total));
			}
			else
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Merging UITexture", index, total);
					if (targetList[index].mainTexture != null && targetList[index].alphaTexture != null)
					{
						string texPath = AssetDatabase.GetAssetPath(targetList[index].mainTexture.GetInstanceID());
						if (IsMatch(texPath) && !AlphaPassExist(texPath))
						{
							string alphaTexPath = AssetDatabase.GetAssetPath(targetList[index].alphaTexture.GetInstanceID());
							if (MergeTextureAlpha(texPath, alphaTexPath))
							{
								targetList[index].alphaTexture = null;

								succeeded++;
							}
						}
					}
				}
				ShowProgress("Merging UITexture", total, total);
				Debug.Log(string.Format("Merging finished, {0} of {1} has been merged.", succeeded, total));
			}
		}
	}

	private UITextureStrip mUITextureStrip = new UITextureStrip();

	private class UI2DSpriteStrip : ParentStrip<UI2DSprite> {

		private const string NOTICE = "if texture has pass alpha, strip the alpha pass to "
								+ "another texture.\nelse if texture has no pass alpha, "
								+ "merge the other texture to the alpha pass of this texture.";

		private Texture mTex;
		private string mTexPath;
		private bool mAlphaPassExist;

		protected override void OnNotice()
		{
			EditorGUILayout.HelpBox(NOTICE, MessageType.Info);
		}

		protected override void OnTarget()
		{
			EditorGUIUtility.labelWidth = 100;
			UI2DSprite newTarget = EditorGUILayout.ObjectField("NGUI 2D Sprite", mTarget, typeof(UI2DSprite), true) as UI2DSprite;
			if (newTarget == mTarget)
			{
				if (newTarget == null || newTarget.mainTexture == mTex)
				{
					return;
				}
			}
			else
			{
				mTarget = newTarget;
			}
			if (mTarget != null)
			{
				mTex = mTarget.mainTexture;
				if (mTex != null)
				{
					mTexPath = AssetDatabase.GetAssetPath(mTex.GetInstanceID());
					if (mOperable = IsMatch(mTexPath))
					{
						mAlphaPassExist = AlphaPassExist(mTexPath);
					}
				}
			}
		}

		protected override void OnButton()
		{
			if (mAlphaPassExist)
			{
				if (mTarget.alphaTexture == null && GUILayout.Button("Strip"))
				{
					string alphaTexPath = mPngRegex.Replace(mTexPath, "_alpha.png");
					mAlphaPassExist = !StripTextureAlpha(mTexPath, alphaTexPath);
					if (!mAlphaPassExist)
					{
						TextureImporter texImporter = AssetImporter.GetAtPath(mTexPath) as TextureImporter;
						texImporter.textureType = TextureImporterType.Sprite;
						AssetDatabase.ImportAsset(mTexPath);
						TextureImporter alphaTexImporter = AssetImporter.GetAtPath(alphaTexPath) as TextureImporter;
						alphaTexImporter.textureType = TextureImporterType.Sprite;
						AssetDatabase.ImportAsset(alphaTexPath);

						mTarget.sprite2D = AssetDatabase.LoadAssetAtPath(mTexPath, typeof(Sprite)) as Sprite;
						mTarget.alphaSprite2D = AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Sprite)) as Sprite;
					}
				}
			}
			else
			{
				if (GUILayout.Button("Merge"))
				{
					Texture2D alphaTex = mTarget.alphaTexture as Texture2D;
					mAlphaPassExist = MergeTextureAlpha(mTarget.mainTexture as Texture2D, alphaTex);
					if (mAlphaPassExist)
					{
						TextureImporter texImporter = AssetImporter.GetAtPath(mTexPath) as TextureImporter;
						texImporter.textureType = TextureImporterType.Sprite;
						AssetDatabase.ImportAsset(mTexPath);

						mTarget.sprite2D = AssetDatabase.LoadAssetAtPath(mTexPath, typeof(Sprite)) as Sprite;
						mTarget.alphaSprite2D = null;
					}
				}
			}
		}

		public override void OnBatch(bool strip)
		{
			List<UI2DSprite> targetList = new List<UI2DSprite>();
			foreach (UI2DSprite target in Selection.GetFiltered(typeof(UI2DSprite), SelectionMode.Deep))
			{
				targetList.Add(target);
			}
			foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
			{
				if (obj is GameObject)
				{
					UI2DSprite target = (obj as GameObject).GetComponent<UI2DSprite>();
					if (target != null && !targetList.Contains(target))
					{
						targetList.Add(target);
					}
				}
			}
			int total = targetList.Count;
			int succeeded = 0;
			if (strip)
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Stripping UI2DSprite", index, total);
					if (targetList[index].mainTexture != null && targetList[index].alphaTexture == null)
					{
						string texPath = AssetDatabase.GetAssetPath(targetList[index].mainTexture.GetInstanceID());
						if (IsMatch(texPath) && AlphaPassExist(texPath))
						{
							string alphaTexPath = mPngRegex.Replace(texPath, "_alpha.png");
							if (StripTextureAlpha(texPath, alphaTexPath))
							{
								TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;
								texImporter.textureType = TextureImporterType.Sprite;
								AssetDatabase.ImportAsset(mTexPath);
								TextureImporter alphaTexImporter = AssetImporter.GetAtPath(alphaTexPath) as TextureImporter;
								alphaTexImporter.textureType = TextureImporterType.Sprite;
								AssetDatabase.ImportAsset(alphaTexPath);

								targetList[index].sprite2D = AssetDatabase.LoadAssetAtPath(texPath, typeof(Sprite)) as Sprite;
								targetList[index].alphaSprite2D = AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Sprite)) as Sprite;

								succeeded++;
							}
						}
					}
				}
				ShowProgress("Stripping UI2DSprite", total, total);
				Debug.Log(string.Format("Stripping finished, {0} of {1} has been stripped.", succeeded, total));
			}
			else
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Merging UI2DSprite", index, total);
					if (targetList[index].mainTexture != null && targetList[index].alphaTexture != null)
					{
						string texPath = AssetDatabase.GetAssetPath(targetList[index].mainTexture.GetInstanceID());
						if (IsMatch(texPath) && !AlphaPassExist(texPath))
						{
							string alphaTexPath = AssetDatabase.GetAssetPath(targetList[index].alphaTexture.GetInstanceID());
							if (MergeTextureAlpha(texPath, alphaTexPath))
							{
								TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;
								texImporter.textureType = TextureImporterType.Sprite;
								AssetDatabase.ImportAsset(mTexPath);

								targetList[index].sprite2D = AssetDatabase.LoadAssetAtPath(texPath, typeof(Sprite)) as Sprite;
								targetList[index].alphaSprite2D = null;

								succeeded++;
							}
						}
					}
				}
				ShowProgress("Merging UI2DSprite", total, total);
				Debug.Log(string.Format("Merging finished, {0} of {1} has been merged.", succeeded, total));
			}
		}
	}

	private UI2DSpriteStrip mUI2DSpriteStrip = new UI2DSpriteStrip();

	private class MaterialStrip : ParentStrip<Material> {

		private const string NOTICE = "if texture has pass alpha, strip the alpha pass to "
								+ "another texture.\nelse if texture has no pass alpha, "
								+ "merge the other texture to the alpha pass of this texture.";

		private Texture mTex;
		private string mTexPath;
		private bool mAlphaPassExist;
		private TextureType mType;

		protected override void OnNotice()
		{
			EditorGUILayout.HelpBox(NOTICE, MessageType.Info);
		}

		protected override void OnTarget()
		{
			EditorGUIUtility.labelWidth = 90 + 10;
			Material newTarget = EditorGUILayout.ObjectField("Unity Material", mTarget, typeof(Material), true) as Material;

			if (newTarget == null)
			{
				if (mTarget != null)
				{
					mTarget = newTarget;
				}
			}
			else if (newTarget.HasProperty("_AlphaTex"))
			{
				if (newTarget.mainTexture != mTex)
				{
					mTarget = newTarget;
					mTex = mTarget.mainTexture;
					if (mTex != null)
					{
						mTexPath = AssetDatabase.GetAssetPath(mTex.GetInstanceID());
						if (mOperable = IsMatch(mTexPath))
						{
							mType = TextureType.TT_PNG;
							mAlphaPassExist = AlphaPassExist(mTexPath);
						}
						else if (mOperable = IsMatch(mTexPath, TextureType.TT_TGA))
						{
							mType = TextureType.TT_TGA;
							mAlphaPassExist = AlphaPassExist(mTexPath, TextureType.TT_TGA);
						}
					}
				}
			}
		}

		protected override void OnButton()
		{
			if (mAlphaPassExist)
			{
				if (mTarget.GetTexture("_AlphaTex") == null && GUILayout.Button("Strip"))
				{
					if (mType == TextureType.TT_PNG)
					{
						string alphaTexPath = mPngRegex.Replace(mTexPath, "_alpha.png");
						if (!(mAlphaPassExist = !StripTextureAlpha(mTexPath, alphaTexPath)))
						{
							mTarget.SetTexture("_AlphaTex", AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D);
						}
					}
					else if (mType == TextureType.TT_TGA)
					{
						string alphaTexPath = TgaManager.GetAlphaPath(mTexPath);
						if (!(mAlphaPassExist = !StripTgaTextureAlpha(mTexPath, alphaTexPath)))
						{
							mTarget.SetTexture("_AlphaTex", AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D);
						}
					}
				}
			}
			else
			{
				if (GUILayout.Button("Merge"))
				{
					if (mType == TextureType.TT_PNG)
					{
						mAlphaPassExist = MergeTextureAlpha(mTex as Texture2D, mTarget.GetTexture("_AlphaTex") as Texture2D);
						if (mAlphaPassExist)
						{
							mTarget.SetTexture("_AlphaTex", mTex);
							mTarget.SetTexture("_AlphaTex", null);
						}
					}
					else if (mType == TextureType.TT_TGA)
					{
						mAlphaPassExist = MergeTgaTextureAlpha(mTex as Texture2D, mTarget.GetTexture("_AlphaTex") as Texture2D);
						if (mAlphaPassExist)
						{
							mTarget.SetTexture("_AlphaTex", mTex);
							mTarget.SetTexture("_AlphaTex", null);
						}
					}
				}
			}
		}

		public override void OnBatch(bool strip)
		{
			List<Material> targetList = new List<Material>();
			foreach (Material target in Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets))
			{
				if (target.shader != null && target.HasProperty("_AlphaTex"))
				{
					targetList.Add(target);
				}
			}
			int total = targetList.Count;
			int succeeded = 0;
			if (strip)
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Stripping Material", index, total);
					if (targetList[index].mainTexture != null && targetList[index].GetTexture("_AlphaTex") == null)
					{
						string texPath = AssetDatabase.GetAssetPath(targetList[index].mainTexture.GetInstanceID());
						if (IsMatch(texPath))
						{
							if (AlphaPassExist(texPath))
							{
								string alphaTexPath = mPngRegex.Replace(texPath, "_alpha.png");
								if (StripTextureAlpha(texPath, alphaTexPath))
								{
									targetList[index].SetTexture("_AlphaTex", AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D);

									succeeded++;
								}
							}
						}
						else if (IsMatch(texPath, TextureType.TT_TGA))
						{
							if (AlphaPassExist(texPath, TextureType.TT_TGA))
							{
								string alphaTexPath = TgaManager.GetAlphaPath(texPath);
								if (StripTgaTextureAlpha(texPath, alphaTexPath))
								{
									targetList[index].SetTexture("_AlphaTex", AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D);

									succeeded++;
								}
							}
						}
					}
				}
				ShowProgress("Stripping Material", total, total);
				Debug.Log(string.Format("Stripping finished, {0} of {1} has been stripped.", succeeded, total));
			}
			else
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Merging Material", index, total);
					if (targetList[index].mainTexture != null && targetList[index].GetTexture("_AlphaTex") != null)
					{
						Texture2D rgbTex = targetList[index].mainTexture as Texture2D;
						Texture2D alphaTex = targetList[index].GetTexture("_AlphaTex") as Texture2D;
						string texPath = AssetDatabase.GetAssetPath(rgbTex.GetInstanceID());
						if (IsMatch(texPath))
						{
							if (!AlphaPassExist(texPath))
							{
								if (MergeTextureAlpha(rgbTex, alphaTex))
								{
									targetList[index].SetTexture("_AlphaTex", rgbTex);
									targetList[index].SetTexture("_AlphaTex", null);

									succeeded++;
								}
							}
						}
						else if (IsMatch(texPath, TextureType.TT_TGA))
						{
							if (!AlphaPassExist(texPath, TextureType.TT_TGA))
							{
								if (MergeTgaTextureAlpha(rgbTex, alphaTex))
								{
									targetList[index].SetTexture("_AlphaTex", rgbTex);
									targetList[index].SetTexture("_AlphaTex", null);

									succeeded++;
								}
							}
						}
					}
				}
				ShowProgress("Merging Material", total, total);
				Debug.Log(string.Format("Merging finished, {0} of {1} has been merged.", succeeded, total));
			}
		}
	}

	private MaterialStrip mMaterialStrip = new MaterialStrip();

	private class UIAtlasStrip : ParentStrip<UIAtlas> {

		private const string NOTICE = "if texture has pass alpha, strip the alpha pass to "
								+ "another texture.\nelse if texture has no pass alpha, "
								+ "merge the other texture to the alpha pass of this texture.";

		private Texture mTex;
		private string mTexPath;
		private bool mAlphaPassExist;

		protected override void OnNotice()
		{
			EditorGUILayout.HelpBox(NOTICE, MessageType.Info);
		}

		protected override void OnTarget()
		{
			EditorGUIUtility.labelWidth = 75 + 25;
			UIAtlas newTarget = EditorGUILayout.ObjectField("NGUI Atlas", mTarget, typeof(UIAtlas), true) as UIAtlas;
			if (newTarget == mTarget)
			{
				if (newTarget == null || newTarget.texture == mTex)
				{
					return;
				}
			}
			else
			{
				mTarget = newTarget;
			}
			if (mTarget != null)
			{
				mTex = mTarget.texture;
				if (mTex != null)
				{
					mTexPath = AssetDatabase.GetAssetPath(mTex.GetInstanceID());
					if (mOperable = IsMatch(mTexPath) && mTarget.spriteMaterial.HasProperty("_AlphaTex"))
					{
						mAlphaPassExist = AlphaPassExist(mTexPath);
					}
				}
			}
		}

		protected override void OnButton()
		{
			if (mAlphaPassExist)
			{
				if (mTarget.alphaTexture == null && GUILayout.Button("Strip"))
				{
					string alphaTexPath = mPngRegex.Replace(mTexPath, "_alpha.png");
					mAlphaPassExist = !StripTextureAlpha(mTexPath, alphaTexPath);
					if (!mAlphaPassExist)
					{
						mTarget.spriteMaterial.SetTexture("_AlphaTex", AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D);
					}
				}
			}
			else
			{
				if (GUILayout.Button("Merge"))
				{
					mAlphaPassExist = MergeTextureAlpha(mTex as Texture2D, mTarget.alphaTexture as Texture2D);
					if (mAlphaPassExist)
					{
						mTarget.spriteMaterial.SetTexture("_AlphaTex", null);
					}
				}
			}
		}

		public override void OnBatch(bool strip)
		{
			List<UIAtlas> targetList = new List<UIAtlas>();
			foreach (UIAtlas target in Selection.GetFiltered(typeof(UIAtlas), SelectionMode.Deep))
			{
				if (target.spriteMaterial.HasProperty("_AlphaTex"))
				{
					targetList.Add(target);
				}
			}
			foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
			{
				if (obj is GameObject)
				{
					UIAtlas target = (obj as GameObject).GetComponent<UIAtlas>();
					if (target != null && target.spriteMaterial.HasProperty("_AlphaTex") && !targetList.Contains(target))
					{
						targetList.Add(target);
					}
				}
			}
			int total = targetList.Count;
			int succeeded = 0;
			if (strip)
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Stripping UIAtlas", index, total);
					if (targetList[index].texture != null && targetList[index].alphaTexture == null)
					{
						string texPath = AssetDatabase.GetAssetPath(targetList[index].texture.GetInstanceID());
						if (IsMatch(texPath) && AlphaPassExist(texPath))
						{
							string alphaTexPath = mPngRegex.Replace(texPath, "_alpha.png");
							if (StripTextureAlpha(texPath, alphaTexPath))
							{
								targetList[index].spriteMaterial.SetTexture("_AlphaTex", AssetDatabase.LoadAssetAtPath(alphaTexPath, typeof(Texture2D)) as Texture2D);

								succeeded++;
							}
						}
					}
				}
				ShowProgress("Stripping UIAtlas", total, total);
				Debug.Log(string.Format("Stripping finished, {0} of {1} has been stripped.", succeeded, total));
			}
			else
			{
				for (int index = 0; index < total; index++)
				{
					ShowProgress("Merging UIAtlas", index, total);
					if (targetList[index].texture != null && targetList[index].alphaTexture != null)
					{
						string texPath = AssetDatabase.GetAssetPath(targetList[index].texture.GetInstanceID());
						if (IsMatch(texPath) && !AlphaPassExist(texPath))
						{
							string alphaTexPath = AssetDatabase.GetAssetPath(targetList[index].alphaTexture.GetInstanceID());
							if (MergeTextureAlpha(texPath, alphaTexPath))
							{
								targetList[index].spriteMaterial.SetTexture("_AlphaTex", null);

								succeeded++;
							}
						}
					}
				}
				ShowProgress("Merging UIAtlas", total, total);
				Debug.Log(string.Format("Merging finished, {0} of {1} has been merged.", succeeded, total));
			}
		}
	}

	private UIAtlasStrip mUIAtlasStrip = new UIAtlasStrip();

	private enum DisplayType {
		All,
		Texture2D,
		UITexture,
		UI2DSprite,
		Material,
		UIAtlas
	}
	private string[] typeStrs = { "   All", "Tex2D", "UITex", "2DSpt", "  Mat", " Atlas" };
	private DisplayType mDisplayType = 0;
	private bool[] typeBatch = new bool[5];

	private Vector2 mScroll = Vector2.zero;
	private Color mBackgroundColor = new Color(1, 0.9F, 0.9F);
	private Color mContentColor = new Color(0.85F, 1, 1);

	void OnGUI()
	{
		GUILayout.Space(5F);

		GUI.backgroundColor = mBackgroundColor;
		mDisplayType = DrawSelectionHeader(mDisplayType, typeStrs);
		mScroll = EditorGUILayout.BeginScrollView(mScroll);
		GUILayout.BeginVertical("As TextArea");

		GUILayout.Space(5F);
		GUI.backgroundColor = mContentColor;

		if (mDisplayType == DisplayType.All)
		{
			if (DrawHeader("Texture2D"))
			{
				GUILayout.Space(-5F);
				mTexture2DStrip.OnGUI(false);
				GUILayout.Space(5F);
			}
			GUILayout.Space(5F);
			if (DrawHeader("UITexture"))
			{
				GUILayout.Space(-5F);
				mUITextureStrip.OnGUI(false);
				GUILayout.Space(5F);
			}
			GUILayout.Space(5F);
			if (DrawHeader("UI2DSprite"))
			{
				GUILayout.Space(-5F);
				mUI2DSpriteStrip.OnGUI(false);
				GUILayout.Space(5F);
			}
			GUILayout.Space(5F);
			if (DrawHeader("Material"))
			{
				GUILayout.Space(-5F);
				mMaterialStrip.OnGUI(false);
				GUILayout.Space(5F);
			}
			GUILayout.Space(5F);
			if (DrawHeader("UIAtlas"))
			{
				GUILayout.Space(-5F);
				mUIAtlasStrip.OnGUI(false);
				GUILayout.Space(5F);
			}
			GUILayout.Space(5F);

			#region Batch

			if (DrawHeader("Batch"))
			{
				GUILayout.BeginHorizontal();
				GUILayout.Space(10F);
				EditorGUILayout.BeginVertical("As TextArea", GUILayout.MinHeight(18));
				GUILayout.Space(5F);
				for (int index = 0; index < typeBatch.Length; index++)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(20F);
					typeBatch[index] = EditorGUILayout.Toggle("", typeBatch[index], GUILayout.Width(16F));
					GUILayout.Label(((DisplayType) index + 1).ToString());
					GUILayout.Space(20F);
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(5F);
				GUILayout.BeginHorizontal();
				GUILayout.Space(20F);
				if (GUILayout.Button("Batch Strip"))
				{
					if (typeBatch[0])
					{
						mTexture2DStrip.OnBatch(true);
					}
					if (typeBatch[1])
					{
						mUITextureStrip.OnBatch(true);
					}
					if (typeBatch[2])
					{
						mUI2DSpriteStrip.OnBatch(true);
					}
					if (typeBatch[3])
					{
						mMaterialStrip.OnBatch(true);
					}
					if (typeBatch[4])
					{
						mUIAtlasStrip.OnBatch(true);
					}
				}
				if (GUILayout.Button("Batch Merge"))
				{
					if (typeBatch[0])
					{
						mTexture2DStrip.OnBatch(false);
					}
					if (typeBatch[1])
					{
						mUITextureStrip.OnBatch(false);
					}
					if (typeBatch[2])
					{
						mUI2DSpriteStrip.OnBatch(false);
					}
					if (typeBatch[3])
					{
						mMaterialStrip.OnBatch(false);
					}
					if (typeBatch[4])
					{
						mUIAtlasStrip.OnBatch(false);
					}
				}
				GUILayout.Space(18F);
				GUILayout.EndHorizontal();
				GUILayout.Space(10F);
				GUILayout.EndVertical();
				GUILayout.Space(10F);
				GUILayout.EndHorizontal();
				GUILayout.Space(5F);
			}

			#endregion
		}
		else
		{
			if (mDisplayType == DisplayType.Texture2D)
			{
				mTexture2DStrip.OnGUI();
			}
			else if (mDisplayType == DisplayType.UITexture)
			{
				mUITextureStrip.OnGUI();
			}
			else if (mDisplayType == DisplayType.UI2DSprite)
			{
				mUI2DSpriteStrip.OnGUI();
			}
			else if (mDisplayType == DisplayType.Material)
			{
				mMaterialStrip.OnGUI();
			}
			else if (mDisplayType == DisplayType.UIAtlas)
			{
				mUIAtlasStrip.OnGUI();
			}
		}
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

	private static DisplayType DrawSelectionHeader(DisplayType displayType, string[] typeOptions)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(5F);
		displayType = (DisplayType) GUILayout.Toolbar((int) displayType, typeOptions, "DragTab");
		//displayType = (DisplayType) GUILayout.SelectionGrid((int) displayType, typeOptions, 6, "DragTab");
		GUILayout.Space(5F);
		GUILayout.EndHorizontal();

		return displayType;
	}

	public static bool IsMatch(string texPath, TextureType type = TextureType.TT_PNG)
	{
		if (type == TextureType.TT_PNG)
		{
			return mPngRegex.IsMatch(texPath);
		}
		else if (type == TextureType.TT_TGA)
		{
			return TgaManager.IsMatchTga(texPath);
		}
		return false;
	}

	public static bool AlphaPassExist(Texture2D tex, TextureType type = TextureType.TT_PNG)
	{
		return AlphaPassExist(AssetDatabase.GetAssetPath(tex.GetInstanceID()), type);
	}

	public static bool AlphaPassExist(string texPath, TextureType type = TextureType.TT_PNG)
	{
		if (type == TextureType.TT_PNG)
		{
			return File.ReadAllBytes(texPath)[25] == 6;
		}
		else if (type == TextureType.TT_TGA)
		{
			return File.ReadAllBytes(texPath)[16] == 32;
		}
		return false;
	}

	public static bool StripTgaTextureAlpha(Texture2D tex)
	{
		return StripTgaTextureAlpha(AssetDatabase.GetAssetPath(tex.GetInstanceID()));
	}

	public static bool StripTgaTextureAlpha(string texPath, string alphaPath = null)
	{
		if (File.Exists(texPath))
		{
			alphaPath = alphaPath ?? TgaManager.GetAlphaPath(texPath);

			TgaManager.AlphaStrip(ImportTexture(texPath, true, true, true));

			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

			ImportTexture(texPath, false, true, false);
			ImportTexture(alphaPath, false, true, false);

			return true;
		}
		return false;
	}

	public static bool MergeTgaTextureAlpha(Texture2D rgbTex, Texture2D alphaTex = null)
	{
		string rgbTexPath = AssetDatabase.GetAssetPath(rgbTex.GetInstanceID());
		string alphaTexPath = alphaTex == null ? null : AssetDatabase.GetAssetPath(alphaTex.GetInstanceID());
		return MergeTgaTextureAlpha(rgbTexPath, alphaTexPath);
	}

	public static bool MergeTgaTextureAlpha(string rgbTexPath, string alphaTexPath = null)
	{
		alphaTexPath = alphaTexPath ?? TgaManager.GetAlphaPath(rgbTexPath);
		if (File.Exists(rgbTexPath) && File.Exists(alphaTexPath))
		{
			Texture2D rgbTex = ImportTexture(rgbTexPath, true, true, false);
			Texture2D alphaTex = ImportTexture(alphaTexPath, true, true, false);
			TgaManager.AlphaMerge(rgbTex, alphaTex);

			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

			ImportTexture(rgbTexPath, false, true, true);

			return true;
		}
		return false;
	}

	public static bool StripTextureAlpha(Texture2D tex)
	{
		return StripTextureAlpha(AssetDatabase.GetAssetPath(tex.GetInstanceID()));
	}

	public static bool StripTextureAlpha(string texPath, string alphaPath = null)
	{
		if (File.Exists(texPath))
		{
			alphaPath = alphaPath ?? mPngRegex.Replace(texPath, "_alpha.png");

			Texture2D tex = ImportTexture(texPath, true, true, true);
			Texture2D alphaTex = AlphaStrip(ref tex);

			byte[] bytes = tex.EncodeToPNG();
			System.IO.File.WriteAllBytes(texPath, bytes);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			ImportTexture(texPath, false, true, false);

			byte[] alphaBytes = alphaTex.EncodeToPNG();
			System.IO.File.WriteAllBytes(alphaPath, alphaBytes);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			ImportTexture(alphaPath, false, true, false);

			return true;
		}
		return false;
	}

	public static bool MergeTextureAlpha(Texture2D rgbTex, Texture2D alphaTex = null)
	{
		string rgbTexPath = AssetDatabase.GetAssetPath(rgbTex.GetInstanceID());
		string alphaTexPath = alphaTex == null ? null : AssetDatabase.GetAssetPath(alphaTex.GetInstanceID());
		return MergeTextureAlpha(rgbTexPath, alphaTexPath);
	}

	public static bool MergeTextureAlpha(string rgbTexPath, string alphaTexPath = null)
	{
		alphaTexPath = alphaTexPath ?? mPngRegex.Replace(rgbTexPath, "_alpha.png");
		if (File.Exists(rgbTexPath) && File.Exists(alphaTexPath))
		{
			Texture2D rgbTex = ImportTexture(rgbTexPath, true, true, false);
			Texture2D alphaTex = ImportTexture(alphaTexPath, true, true, false);

			Texture2D tex = AlphaMerge(rgbTex, alphaTex);
			if (tex != null)
			{
				AssetDatabase.DeleteAsset(alphaTexPath);

				byte[] bytes = tex.EncodeToPNG();
				System.IO.File.WriteAllBytes(rgbTexPath, bytes);
				bytes = null;

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			}

			ImportTexture(rgbTexPath, false, true, true);

			return true;
		}
		return false;
	}

	public static Texture2D AlphaStrip(ref Texture2D tex)
	{
		int width = tex.width;
		int height = tex.height;
		if (width > 0 && height > 0)
		{
			Color32[] pixels = tex.GetPixels32();
			Color[] rgbPixels = new Color[width * height];
			Color[] alphaPixels = new Color[width * height];

			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					int index = y * width + x;
					Color color = pixels[index];
					rgbPixels[index] = color;
					alphaPixels[index] = color.a * Color.white;
				}
			}

			Texture2D alphaTex = new Texture2D(width, height, TextureFormat.RGB24, false);
			alphaTex.SetPixels(alphaPixels);
			alphaTex.Apply();
			Texture2D rgbTex = new Texture2D(width, height, TextureFormat.RGB24, false);
			rgbTex.SetPixels(rgbPixels);
			rgbTex.Apply();
			tex = rgbTex;

			return alphaTex;
		}
		return null;
	}

	public static Texture2D AlphaMerge(Texture2D rgbTex, Texture2D alphaTex)
	{
		int width = rgbTex.width;
		int height = rgbTex.height;

		if (width > 0 && height > 0 && alphaTex.width == width && alphaTex.height == height)
		{
			Color[] rgbPixels = rgbTex.GetPixels();
			Color[] alphaPixels = alphaTex.GetPixels();
			Color32[] pixels = new Color32[width * height];

			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					int index = y * width + x;
					Color color = rgbPixels[index];
					color.a = alphaPixels[index].r;
					pixels[index] = color;
				}
			}

			Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
			tex.SetPixels32(pixels);
			tex.Apply();

			return tex;
		}
		return null;
	}

	public static Texture2D ImportTexture(string path, bool forInput, bool force, bool alphaTransparency)
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

	public static bool SetTextureImporter(string path, bool force, bool alphaTransparency)
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

	[@MenuItem("Window/Texture Operation/Texture Alpha Strip")]
	private static void Window()
	{
		TextureAlphaStrip window = GetWindow(typeof(TextureAlphaStrip), false, "Alpha Strip") as TextureAlphaStrip;
		window.minSize = new Vector2(320, 320);
		window.ShowTab();
	}
}
