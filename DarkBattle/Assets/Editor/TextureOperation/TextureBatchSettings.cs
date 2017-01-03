using UnityEngine;
using System.Collections;
using UnityEditor;

public class TextureBatchSettings : EditorWindow {

	private bool changed;

	private TextureImporterType oldTextureType;
	private TextureImporterType textureType;
	private string[] textureTypeString = new string[] { "Texture", "Normal Map", "GUI (Editor \\ Legacy)", "Sprite (2D \\ uGUI)", "Cursor", "Reflection", "Cookie", "Lightmap", "Advanced" };
	private int[] textureTypeArray = new int[] { 0, 1, 2, 8, 7, 3, 4, 6, 5 };

	private bool grayscaleToAlpha;

	private bool alphaIsTransparency;

	private TextureWrapMode wrapMode;
	private string[] wrapModeString = new string[] { "Repeat", "Clamp" };

	private FilterMode filterMode = FilterMode.Bilinear;
	private string[] filterModeString = new string[] { "Point", "Bilinear", "Trilinear" };

	private int anisoLevel = 1;

	private bool createFromGrayscale = true;

	private float bumpiness = 0.25f;

	private int filtering;
	private string[] filteringString = new string[] { "Sharp", "Smooth" };

	private SpriteImportMode spriteMode = SpriteImportMode.Single;
	private string[] spriteModeString = new string[] { "Single", "Multiple" };
	private string[] spriteModeStringFull = new string[] { "None", "Single", "Multiple" };
	private int[] spriteModeArray = new int[] { 1, 2 };

	private string packingTag = "";

	private float pixelsToUnits = 100;

	private SpriteMeshType meshType = SpriteMeshType.Tight;
	private string[] meshTypeString = new string[] { "Full Rect", "Tight" };

	private uint extrudeEdges = 1;

	private int pivot = 505;
	private string[] pivotString = new string[] { "Center", "Top Left", "Top", "Top Right", "Left", "Right", "Bottom Left", "Bottom", "Bottom Right", "Custom" };
	private int[] pivotArray = new int[] { 505, 0, 500, 1000, 5, 1005, 10, 510, 1010, -1 };
	private float pivotX = 0.5f;
	private float pivotY = 0.5f;

	private int mapping = 4;
	private string[] mappingString = new string[] { "Sphere mapped", "Cylindrical", "Simple Sphere", "Nice Sphere", "6 Frames Layout" };

	private bool fixupEdgeSeams;

	private LightType lightType;
	private string[] lightTypeString = new string[] { "Spotlight", "Directional", "Point" };

	private TextureImporterNPOTScale nonPowerOf2 = TextureImporterNPOTScale.ToNearest;
	private string[] nonPowerOf2String = new string[] { "None", "ToNearest", "ToLarger", "ToSmaller" };

	private TextureImporterGenerateCubemap generateCubemap;
	private string[] generateCubemapString = new string[] { "None", "Spheremap", "Cylindrical", "SimpleSpheremap", "NiceSpheremap", "FullCubemap" };

	private bool readWriteEnabled;

	private TextureImporterType importType;
	private string[] importTypeString = new string[] { "Default", "Normal Map", "Lightmap" };
	private int[] importTypeArray = new int[] { 0, 1, 6 };

	private bool bypassSRGBSampling;

	private bool generateMipMaps = true;

	private bool inLinearSpace;

	private bool borderMipMaps;

	private TextureImporterMipFilter mipMapFiltering;
	private string[] mipMapFilteringString = new string[] { "Box", "Kaiser" };

	private bool fadeoutMipMaps;
	private float fadeoutMin = 1;
	private float fadeoutMax = 3;


	private int platformIndex = 8;

	private bool overrideForPlatform;
	private Texture[] platformImages = new Texture[8];
	private string[] platformStrings = new string[] { "Web", "Standalone", "iPhone", "Android", "BlackBerry", "FlashPlayer", "Windows Store Apps", "WP8" };
	private bool[] overrideForPlatformBools = new bool[8];

	private int[] maxTextureSize = new int[9];
	private string[] maxSizeString = new string[] { "32", "64", "128", "256", "512", "1024", "2048", "4096" };

	private TextureImporterFormat[] textureFormats = new TextureImporterFormat[9];
	private string[][] textureFormatStrings = new string[8][];
	private int[][] textureFormatArrays = new int[8][];

	private int[] compressionQualitys = new int[9];
	private string[] compressionQualitysString = new string[] { "Fast", "Normal", "Best" };
	private int[] compressionQualitysArray = new int[] { 0, 50, 100 };

	private void Init()
	{
		for (int i = 0; i < platformStrings.Length; i++)
		{
			platformImages[i] = Resources.Load<Texture>("Platform/" + platformStrings[i]);
		}


		textureFormatStrings[0] = new string[] { "Compressed", "16 bits", "Truecolor" };
		textureFormatArrays[0] = new int[] { -1, -2, -3 };
		textureFormatStrings[1] = new string[] { "8 Bit Alpha" };
		textureFormatArrays[1] = new int[] { 1 };
		textureFormatStrings[2] = new string[] {
			"Automatic Compressed",
			"RGB Compressed DXT1", "RGBA Compressed DXT5", "RGB Compressed ETC 4 bits",
			"RGB Compressed PVRTC 2 bits", "RGBA Compressed PVRTC 2 bits", "RGB Compressed PVRTC 4 bits", "RGBA Compressed PVRTC 4 bits",
			"RGB Compressed ATC 4 bits", "RGBA Compressed ATC 8 bits",
			"RGB Compressed ETC2 4 bits", "RGB + 1-bit Alpha Compressed ETC2 4 bits", "RGBA Compressed ETC2 8 bits",
			"RGB Compressed ASTC 4x4 block", "RGB Compressed ASTC 5x5 block", "RGB Compressed ASTC 6x6 block",
			"RGB Compressed ASTC 8x8 block", "RGB Compressed ASTC 10x10 block", "RGB Compressed ASTC 12x12 block",
			"RGBA Compressed ASTC 4x4 block", "RGBA Compressed ASTC 5x5 block", "RGBA Compressed ASTC 6x6 block",
			"RGBA Compressed ASTC 8x8 block", "RGBA Compressed ASTC 10x10 block", "RGBA Compressed ASTC 12x12 block",
			"Automatic 16 bits", "RGB 16 bit", "ARGB 16 bit", "RGBA 16 bit",
			"Automatic Truecolor", "RGB 24 bit", "Alpha 8", "ARGB 32 bit", "RGBA 32 bit"
		};
		textureFormatArrays[2] = new int[] {
			-1,
			10, 12, 34,
			30, 31, 32, 33,
			35, 36,
			45, 46, 47,
			48, 49, 50,
			51, 52, 53,
			54, 55, 56,
			57, 58, 59,
			-2, 7, 2, 13,
			-3, 3, 1, 5, 4
		};
		textureFormatStrings[3] = new string[] { "RGB Compressed DXT1", "RGBA Compressed DXT5", "RGB 16 bit", "RGB 24 bit", "Alpha 8", "ARGB 16 bit", "ARGB 32 bit" };
		textureFormatArrays[3] = new int[] { 10, 12, 7, 3, 1, 2, 5 };
		textureFormatStrings[4] = new string[] {
			"RGB Compressed PVRTC 2 bits", "RGBA Compressed PVRTC 2 bits", "RGB Compressed PVRTC 4 bits", "RGBA Compressed PVRTC 4 bits",
			"RGB 16 bit", "RGB 24 bit", "Alpha 8", "RGBA 16 bit", "RGBA 32 bit"
		};
		textureFormatArrays[4] = new int[] {
			30, 31, 32, 33,
			7, 3, 1, 13, 4
		};
		textureFormatStrings[5] = new string[] {
			"RGB Compressed DXT1", "RGBA Compressed DXT5", "RGB Compressed ETC 4 bits",
			"RGB Compressed ETC2 4 bits", "RGB + 1-bit Alpha Compressed ETC2 4 bits", "RGBA Compressed ETC2 8 bits",
			"RGB Compressed PVRTC 2 bits", "RGBA Compressed PVRTC 2 bits", "RGB Compressed PVRTC 4 bits", "RGBA Compressed PVRTC 4 bits",
			"RGB Compressed ATC 4 bits", "RGBA Compressed ATC 8 bits",
			"RGB Compressed ASTC 4x4 block", "RGB Compressed ASTC 5x5 block", "RGB Compressed ASTC 6x6 block",
			"RGB Compressed ASTC 8x8 block", "RGB Compressed ASTC 10x10 block", "RGB Compressed ASTC 12x12 block",
			"RGBA Compressed ASTC 4x4 block", "RGBA Compressed ASTC 5x5 block", "RGBA Compressed ASTC 6x6 block",
			"RGBA Compressed ASTC 8x8 block", "RGBA Compressed ASTC 10x10 block", "RGBA Compressed ASTC 12x12 block",
			"RGB 16 bit", "RGB 24 bit", "Alpha 8", "RGBA 16 bit", "RGBA 32 bit"
		};
		textureFormatArrays[5] = new int[] {
			10, 12, 34,
			45, 46, 47,
			30, 31, 32, 33,
			35, 36,
			48, 49, 50,
			51, 52, 53,
			54, 55, 56,
			57, 58, 59,
			7, 3, 1, 13, 4
		};
		textureFormatStrings[6] = new string[] {
			"RGB Compressed ETC 4 bits",
			"RGB Compressed PVRTC 2 bits", "RGBA Compressed PVRTC 2 bits", "RGB Compressed PVRTC 4 bits", "RGBA Compressed PVRTC 4 bits",
			"RGB 16 bit", "RGB 24 bit", "Alpha 8", "RGBA 16 bit", "RGBA 32 bit"
		};
		textureFormatArrays[6] = new int[] {
			34,
			30, 31, 32, 33,
			7, 3, 1, 13, 4
		};
		textureFormatStrings[7] = new string[] { "RGB JPG Compressed", "RGBA JPG Compressed", "RGB 24 bit", "RGBA 32 bit" };
		textureFormatArrays[7] = new int[] { 40, 39, 3, 4 };

		Revert();
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(10f);
		textureType = (TextureImporterType) DrawPopup("Texture Type", (int) textureType, textureTypeString, textureTypeArray);
		GUILayout.Space(10f);
		GUILayout.EndHorizontal();

		if (DrawHeader(textureType.ToString()))
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(11f);
			EditorGUILayout.BeginVertical("As TextArea", GUILayout.MinHeight(18));
			GUILayout.Space(2f);
			DrawTextureSettings(textureType);
			GUILayout.Space(2f);
			EditorGUILayout.EndVertical();
			GUILayout.Space(15f);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(5f);
		}
		oldTextureType = textureType;

		platformIndex = DrawPlatformHeader(platformImages, platformIndex);
		GUILayout.Space(-4f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(11f);
		EditorGUILayout.BeginVertical("As TextArea", GUILayout.MinHeight(18));
		GUILayout.Space(2f);
		DrawPlatformSettings(platformStrings, textureType, platformIndex);
		GUILayout.Space(2f);
		EditorGUILayout.EndVertical();
		GUILayout.Space(15f);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(5f);
		if (GUI.changed)
		{
			changed = true;
		}

		GUILayout.BeginHorizontal();
		GUILayout.Space(10f);
		if (GUILayout.Button("Select all"))
		{
			SelectAll();
		}

		GUI.enabled = changed;
		if (GUILayout.Button("Revert"))
		{
			Revert();
			changed = false;
		}

		GUI.enabled = true;
		if (GUILayout.Button("Apply all"))
		{
			ApplyAll();
		}
		GUILayout.Space(10f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void SelectAll()
	{
		Object[] selections = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);

		Selection.objects = selections;
	}

	private void ApplyAll()
	{
		Object[] selections = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);

		foreach (Texture2D texture in selections)
		{
			string path = AssetDatabase.GetAssetPath(texture);
			Apply(path);
		}
	}

	private void Apply(string path)
	{
		TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
		TextureImporterSettings settings = new TextureImporterSettings();
		textureImporter.ReadTextureSettings(settings);

		textureImporter.textureType = textureType;
		settings.grayscaleToAlpha = grayscaleToAlpha;
		if (textureImporter.DoesSourceTextureHaveAlpha())
		{
			settings.alphaIsTransparency = textureType == TextureImporterType.GUI || alphaIsTransparency;
		}
		settings.wrapMode = wrapMode;
		if ((settings.filterMode = filterMode) != FilterMode.Point)
		{
			textureImporter.anisoLevel = anisoLevel;
		}
		//if (textureImporter.createFromGrayscale = createFromGrayscale)
		//{
		//	textureImporter.bumpiness = bumpiness;
		//	textureImporter.filtering = filtering;
		//}
		textureImporter.spriteImportMode = spriteMode;
		if (spriteMode != SpriteImportMode.None)
		{
			textureImporter.spritePackingTag = packingTag;
#if UNITY_4_7
			settings.spritePixelsPerUnit = pixelsToUnits;
#else
			settings.spritePixelsToUnits = pixelsToUnits;
#endif
			settings.spriteMeshType = (SpriteMeshType) meshType;
			settings.spriteExtrude = extrudeEdges;
			if (spriteMode == SpriteImportMode.Single)
			{
				settings.spritePivot = pivot < 0 ? new Vector2(pivotX, pivotY) : new Vector2(pivot / 100 / 10f, pivot % 100 / 10f);
			}
		}
		//if (textureImporter.lightType = lightType) == LightType.Point)
		{
			//textureImporter.mapping = mapping;
			//textureImporter.fixupEdgeSeams = fixupEdgeSeams;
		}
		settings.npotScale = nonPowerOf2;
		settings.generateCubemap = generateCubemap;
		settings.readable = readWriteEnabled;
		if (importType == TextureImporterType.Image)
		{
			//textureImporter.bypassSRGBSampling = bypassSRGBSampling;
		}
		if (settings.mipmapEnabled = generateMipMaps)
		{
			settings.generateMipsInLinearSpace = inLinearSpace;
			settings.borderMipmap = borderMipMaps;
			settings.mipmapFilter = mipMapFiltering;
			if (settings.fadeOut = fadeoutMipMaps)
			{
				settings.mipmapFadeDistanceStart = Mathf.RoundToInt(fadeoutMin);
				settings.mipmapFadeDistanceStart = Mathf.RoundToInt(fadeoutMin);
			}
		}

		int currentPlatformIndex = 0;
		switch (Application.platform)
		{
			case RuntimePlatform.WindowsWebPlayer:
			case RuntimePlatform.OSXWebPlayer:
				currentPlatformIndex = 0;
				break;
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.LinuxPlayer:
				currentPlatformIndex = 1;
				break;
			case RuntimePlatform.IPhonePlayer:
				currentPlatformIndex = 2;
				break;
			case RuntimePlatform.Android:
				currentPlatformIndex = 3;
				break;
			case RuntimePlatform.BlackBerryPlayer:
				currentPlatformIndex = 4;
				break;
			case RuntimePlatform.FlashPlayer:
				currentPlatformIndex = 5;
				break;
			case RuntimePlatform.WP8Player:
				currentPlatformIndex = 7;
				break;
			default:
				currentPlatformIndex = 8;
				break;
		}
		if (!overrideForPlatformBools[currentPlatformIndex])
		{
			currentPlatformIndex = 8;
		}
		settings.maxTextureSize = 1 << (maxTextureSize[currentPlatformIndex] + 5);
		settings.textureFormat = textureFormats[currentPlatformIndex];
		settings.compressionQuality = compressionQualitys[currentPlatformIndex];

		textureImporter.SetTextureSettings(settings);
		AssetDatabase.ImportAsset(path);
	}

	private void Revert()
	{
		textureType = TextureImporterType.Image;
		grayscaleToAlpha = false;
		alphaIsTransparency = false;
		wrapMode = TextureWrapMode.Repeat;
		filterMode = FilterMode.Bilinear;
		anisoLevel = 1;
		createFromGrayscale = true;
		bumpiness = 0.25f;
		filtering = 0;
		spriteMode = SpriteImportMode.Single;
		packingTag = "";
		pixelsToUnits = 100;
		meshType = SpriteMeshType.Tight;
		extrudeEdges = 1;
		pivot = 505;
		pivotX = 0.5f;
		pivotY = 0.5f;
		mapping = 4;
		fixupEdgeSeams = false;
		lightType = LightType.Spot;
		nonPowerOf2 = TextureImporterNPOTScale.ToNearest;
		generateCubemap = TextureImporterGenerateCubemap.None;
		readWriteEnabled = false;
		importType = 0;
		bypassSRGBSampling = false;
		generateMipMaps = true;
		inLinearSpace = false;
		borderMipMaps = false;
		mipMapFiltering = TextureImporterMipFilter.BoxFilter;
		fadeoutMipMaps = false;
		fadeoutMin = 1;
		fadeoutMax = 3;

		for (int i = 0, length = textureFormats.Length; i < length; i++)
		{
			textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
		}
		for (int i = 0, length = compressionQualitys.Length; i < length; i++)
		{
			compressionQualitys[i] = (int) TextureCompressionQuality.Normal;
		}
	}

	private void Reset()
	{
		switch (textureType)
		{
			case TextureImporterType.Image:
				grayscaleToAlpha = false;
				alphaIsTransparency = false;
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
				}
				break;
			case TextureImporterType.Bump:
				createFromGrayscale = true;
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
				}
				break;
			case TextureImporterType.GUI:
				spriteMode = SpriteImportMode.Single;
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
				}
				break;
			case TextureImporterType.Sprite:
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
				}
				break;
			case TextureImporterType.Cursor:
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
				}
				break;
			case TextureImporterType.Reflection:
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
				}
				mapping = 4;
				fixupEdgeSeams = false;
				break;
			case TextureImporterType.Cookie:
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.Alpha8;
				}
				lightType = LightType.Spot;
				mapping = 2;
				fixupEdgeSeams = false;
				grayscaleToAlpha = false;
				alphaIsTransparency = false;
				break;
			case TextureImporterType.Lightmap:
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
					textureFormats[i] = TextureImporterFormat.AutomaticCompressed;
				}
				break;
			case TextureImporterType.Advanced:
				nonPowerOf2 = 0;
				readWriteEnabled = false;
				importType = 0;
				grayscaleToAlpha = false;
				alphaIsTransparency = false;
				bypassSRGBSampling = false;
				spriteMode = SpriteImportMode.None;
				generateMipMaps = true;
				inLinearSpace = false;
				borderMipMaps = false;
				for (int i = 0, length = textureFormats.Length; i < length; i++)
				{
					maxTextureSize[i] = 5;
				}
				textureFormats[8] = TextureImporterFormat.AutomaticCompressed;
				textureFormats[0] = TextureImporterFormat.DXT5;
				textureFormats[1] = TextureImporterFormat.DXT5;
				textureFormats[2] = TextureImporterFormat.PVRTC_RGBA4;
				textureFormats[3] = TextureImporterFormat.RGBA16;
				textureFormats[4] = TextureImporterFormat.RGBA16;
                textureFormats[5] = TextureImporterFormat.DXT5;
				textureFormats[6] = TextureImporterFormat.DXT5;
				textureFormats[7] = TextureImporterFormat.DXT5;
				break;
		}
	}

	private void DrawTextureSettings(TextureImporterType textureType)
	{
		if (textureType != oldTextureType)
		{
			Reset();
		}
		switch (textureType)
		{
			case TextureImporterType.Image:
				if ((grayscaleToAlpha = EditorGUILayout.Toggle("Alpha from Grayscale", grayscaleToAlpha)))
				{
					alphaIsTransparency = EditorGUILayout.Toggle("Alpha is Transparency", alphaIsTransparency);
				}
				GUILayout.Space(8f);
				wrapMode = (TextureWrapMode) DrawPopup("Wrap Mode", (int) wrapMode, wrapModeString);
				if ((filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString)) != FilterMode.Point)
				{
					anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 9);
				}
				break;

			case TextureImporterType.Bump:
				if (createFromGrayscale = EditorGUILayout.Toggle("(Invalid) Create from Grayscale", createFromGrayscale))
				{
					bumpiness = EditorGUILayout.Slider("(Invalid) Bumpiness", bumpiness, 0, 0.3f);
					filtering = DrawPopup("(Invalid) Filtering", filtering, filteringString);
				}
				GUILayout.Space(8f);
				wrapMode = (TextureWrapMode) DrawPopup("Wrap Mode", (int) wrapMode, wrapModeString);
				if ((filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString)) != FilterMode.Point)
				{
					anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 9);
				}
				break;

			case TextureImporterType.GUI:
				GUILayout.Space(8f);
				filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString);
				break;

			case TextureImporterType.Sprite:
				spriteMode = (SpriteImportMode) DrawPopup("Sprite Mode", (int) spriteMode, spriteModeString, spriteModeArray);
				packingTag = EditorGUILayout.TextField("    Packing Tag", packingTag);
				pixelsToUnits = EditorGUILayout.FloatField("    Pixels To Units", pixelsToUnits);
				if (spriteMode == SpriteImportMode.Single)
				{
					if ((pivot = DrawPopup("    Pivot", pivot, pivotString, pivotArray)) == -1)
					{
						GUILayout.BeginHorizontal();
						GUILayout.Space(EditorGUIUtility.labelWidth);
						GUILayout.Label("X");
						pivotX = EditorGUILayout.FloatField(pivotX);
						GUILayout.Label("Y");
						pivotY = EditorGUILayout.FloatField(pivotY);
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.Space(8f);
				filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString);
				break;

			case TextureImporterType.Cursor:
				GUILayout.Space(8f);
				wrapMode = (TextureWrapMode) DrawPopup("Wrap Mode", (int) wrapMode, wrapModeString);
				filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString);
				break;

			case TextureImporterType.Reflection:
				mapping = DrawPopup("Mapping", mapping, mappingString);
				fixupEdgeSeams = EditorGUILayout.Toggle("Fixup Edge Seams", fixupEdgeSeams);
				GUILayout.Space(8f);
				if ((filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString)) != FilterMode.Point)
				{
					anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 9);
				}
				break;

			case TextureImporterType.Cookie:
				if ((lightType = (LightType) DrawPopup("(Invalid) Light Type", (int) lightType, lightTypeString)) == LightType.Point)
				{
					mapping = DrawPopup("(Invalid) Mapping", mapping, mappingString);
					fixupEdgeSeams = EditorGUILayout.Toggle("(Invalid) Fixup Edge Seams", fixupEdgeSeams);
				}
				grayscaleToAlpha = EditorGUILayout.Toggle("Alpha from Grayscale", grayscaleToAlpha);
				GUILayout.Space(8f);
				if ((filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString)) != FilterMode.Point)
				{
					anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 9);
				}
				break;

			case TextureImporterType.Lightmap:
				GUILayout.Space(8f);
				if ((filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString)) != FilterMode.Point)
				{
					anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 9);
				}
				break;

			case TextureImporterType.Advanced:
				nonPowerOf2 = (TextureImporterNPOTScale) DrawPopup("Non Power of 2", (int) nonPowerOf2, nonPowerOf2String);
				generateCubemap = (TextureImporterGenerateCubemap) DrawPopup("Generate Cubemap", (int) generateCubemap, generateCubemapString);
				readWriteEnabled = EditorGUILayout.Toggle("Read/Write Enabled", readWriteEnabled);
				importType = (TextureImporterType) DrawPopup("Import Type", (int) importType, importTypeString, importTypeArray);
				if (importType == TextureImporterType.Image)
				{
					grayscaleToAlpha = EditorGUILayout.Toggle("    Alpha from Grayscale", grayscaleToAlpha);
					alphaIsTransparency = EditorGUILayout.Toggle("    Alpha is Transparency", alphaIsTransparency);
					bypassSRGBSampling = EditorGUILayout.Toggle("    (Invalid) Bypass sRGB Sampling", bypassSRGBSampling);
					spriteMode = (SpriteImportMode) DrawPopup("    Sprite Mode", (int) spriteMode, spriteModeStringFull);
					if (spriteMode != SpriteImportMode.None)
					{
						packingTag = EditorGUILayout.TextField("        Packing Tag", packingTag);
						pixelsToUnits = EditorGUILayout.FloatField("        Pixels To Units", pixelsToUnits);
						meshType = (SpriteMeshType) DrawPopup("        Mesh Type", (int) meshType, meshTypeString);
						extrudeEdges = (uint) EditorGUILayout.IntSlider("        Extrude Edges", (int) extrudeEdges, 0, 32);
						if (spriteMode == SpriteImportMode.Single)
						{
							if ((pivot = DrawPopup("        Pivot", pivot, pivotString, pivotArray)) == -1)
							{
								GUILayout.BeginHorizontal();
								GUILayout.Space(EditorGUIUtility.labelWidth);
								GUILayout.Label("X");
								pivotX = EditorGUILayout.FloatField(pivotX);
								GUILayout.Label("Y");
								pivotY = EditorGUILayout.FloatField(pivotY);
								GUILayout.EndHorizontal();
							}
						}
					}
				}
				else if (importType == TextureImporterType.Bump)
				{
					if (createFromGrayscale = EditorGUILayout.Toggle("    (Invalid) Create from Grayscale", createFromGrayscale))
					{
						bumpiness = EditorGUILayout.Slider("    (Invalid) Bumpiness", bumpiness, 0, 0.3f);
						filtering = DrawPopup("    Filtering", filtering, filteringString);
					}
				}
				GUILayout.Space(8f);
				if (generateMipMaps = EditorGUILayout.Toggle("Generate Mip Maps", generateMipMaps))
				{
					inLinearSpace = EditorGUILayout.Toggle("    In Linear Space", inLinearSpace);
					borderMipMaps = EditorGUILayout.Toggle("    Border Mip Maps", borderMipMaps);
					mipMapFiltering = (TextureImporterMipFilter) DrawPopup("    Mip Map Filtering", (int) mipMapFiltering, mipMapFilteringString);
					if (fadeoutMipMaps = EditorGUILayout.Toggle("    Fadeout Mip Maps", fadeoutMipMaps))
					{
						GUILayout.BeginHorizontal();
						GUILayout.Label("        Fade Range");
						GUILayout.Space(43f);
						EditorGUILayout.MinMaxSlider(ref fadeoutMin, ref fadeoutMax, 0, 10);
						fadeoutMin = Mathf.RoundToInt(fadeoutMin);
						fadeoutMax = Mathf.RoundToInt(fadeoutMax);
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.Space(8f);
				wrapMode = (TextureWrapMode) DrawPopup("Wrap Mode", (int) wrapMode, wrapModeString);
				if ((filterMode = (FilterMode) DrawPopup("Filter Mode", (int) filterMode, filterModeString)) != FilterMode.Point)
				{
					anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 9);
				}
				break;

			default:
				break;
		}
		GUILayout.Space(8f);
	}

	private void DrawPlatformSettings(string[] platformStrings, TextureImporterType textureType, int platformIndex)
	{
		int formatIndex = 0;
		switch (textureType)
		{
			case TextureImporterType.Cookie:
				formatIndex = 1;
				break;

			case TextureImporterType.Advanced:
				switch (platformIndex)
				{
					case 8:
						formatIndex = 2;
						break;

					case 0:
					case 1:
						formatIndex = 3;
						break;

					case 2:
						formatIndex = 4;
						break;

					case 3:
						formatIndex = 5;
						break;

					case 4:
						formatIndex = 6;
						break;

					case 5:
						formatIndex = 7;
						break;

					case 6:
					case 7:
						formatIndex = 3;
						break;
				}
				break;

			default:
				formatIndex = 0;
				break;
		}
		string[] textureFormatString = textureFormatStrings[formatIndex];
		int[] textureFormatArray = textureFormatArrays[formatIndex];

		GUI.enabled = platformIndex == 8 || (overrideForPlatformBools[platformIndex] = EditorGUILayout.ToggleLeft("Override for " + platformStrings[platformIndex], overrideForPlatformBools[platformIndex]));
		maxTextureSize[platformIndex] = DrawPopup("Max Size", maxTextureSize[platformIndex], maxSizeString);
		if (textureType == TextureImporterType.Cookie)
		{
			GUI.enabled = false;
		}
		textureFormats[platformIndex] = (TextureImporterFormat) DrawPopup("Format", (int) textureFormats[platformIndex], textureFormatString, textureFormatArray);

		switch (platformIndex)
		{
			case 2:
				switch (textureFormats[platformIndex])
				{
					case TextureImporterFormat.PVRTC_RGB2:
					case TextureImporterFormat.PVRTC_RGB4:
					case TextureImporterFormat.PVRTC_RGBA2:
					case TextureImporterFormat.PVRTC_RGBA4:
						compressionQualitys[platformIndex] = DrawPopup("Compression Quality", compressionQualitys[platformIndex], compressionQualitysString, compressionQualitysArray);
						break;
				}
				break;
			case 3:
				switch (textureFormats[platformIndex])
				{
					case TextureImporterFormat.DXT1:
					case TextureImporterFormat.DXT5:
					case TextureImporterFormat.RGB16:
					case TextureImporterFormat.RGB24:
					case TextureImporterFormat.Alpha8:
					case TextureImporterFormat.RGBA16:
					case TextureImporterFormat.RGBA32:
						break;
					default:
						compressionQualitys[platformIndex] = DrawPopup("Compression Quality", compressionQualitys[platformIndex], compressionQualitysString, compressionQualitysArray);
						break;
				}
				break;
			case 4:
				switch (textureFormats[platformIndex])
				{
					case TextureImporterFormat.ETC_RGB4:
					case TextureImporterFormat.PVRTC_RGB2:
					case TextureImporterFormat.PVRTC_RGBA2:
					case TextureImporterFormat.PVRTC_RGB4:
					case TextureImporterFormat.PVRTC_RGBA4:
						break;
					default:
						compressionQualitys[platformIndex] = DrawPopup("Compression Quality", compressionQualitys[platformIndex], compressionQualitysString, compressionQualitysArray);
						break;
				}
				break;
            //case 5:
            //    switch (textureFormats[platformIndex])
            //    {
            //        case TextureImporterFormat.ATF_RGB_JPG:
            //        case TextureImporterFormat.ATF_RGBA_JPG:
            //            break;
            //        default:
            //            compressionQualitys[platformIndex] = EditorGUILayout.IntSlider("Compression Quality", compressionQualitys[platformIndex], 0, 100);
            //            break;
            //    }
            //    break;
			default:
				break;
		}
		GUI.enabled = true;
	}

	private int DrawPlatformHeader(Texture[] platformImages, int platformIndex)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(10f);
		GUI.backgroundColor = platformIndex == 8 ? Color.white : new Color(0.85f, 0.85f, 0.85f);
		if (GUILayout.Button("Default", platformIndex == 8 ? "Box" : "Button", GUILayout.Width(EditorGUIUtility.currentViewWidth - 265), GUILayout.Height(20)))
		{
			platformIndex = 8;
		}
		for (int i = 0, length = platformImages.Length; i < length; i++)
		{
			GUILayout.Space(-4f);
			GUI.backgroundColor = i == platformIndex ? Color.white : new Color(0.85f, 0.85f, 0.85f);
			if (GUILayout.Button(platformImages[i], i == platformIndex ? "Box" : "Button", GUILayout.Width(30), GUILayout.Height(20)))
			{
				platformIndex = i;
			}
		}
		GUI.backgroundColor = Color.white;
		GUILayout.Space(4f);
		GUILayout.EndHorizontal();

		return platformIndex;
	}

	private bool DrawHeader(string title)
	{
		string key = title;
		bool state = EditorPrefs.GetBool(key, true);

		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(10f);
		title = "<b><size=11>" + title + "</size></b>";
		if (state)
		{
			title = "\u25BC " + title;
			GUI.backgroundColor = new Color(1, 1, 1);
		}
		else
		{
			title = "\u25BA " + title;
			GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
		}
		if (!GUILayout.Toggle(true, title, "dragTab", GUILayout.MinWidth(20f)))
			state = !state;

		GUILayout.Space(14f);
		EditorGUILayout.EndHorizontal();

		if (GUI.changed)
		{
			EditorPrefs.SetBool(key, state);
		}

		return state;
	}
	
	private int DrawPopup(string label, int selectedIndex, string[] displayedOptions, int[] optionValues = null, params GUILayoutOption[] options)
	{
		GUILayout.BeginHorizontal();
		int selection = selectedIndex;
		if (optionValues == null)
		{
			selection = EditorGUILayout.Popup(label, selectedIndex, displayedOptions, options);
		}
		else
		{
			selection = EditorGUILayout.IntPopup(label, selectedIndex, displayedOptions, optionValues, options);
		}
		GUILayout.EndHorizontal();
		return selection;
	}

	[@MenuItem("Window/Batch Setting/Texture Setting")]
	private static void Window()
	{
		TextureBatchSettings window = (TextureBatchSettings) GetWindow(typeof(TextureBatchSettings), false, "Texture Batch Setting");
		window.minSize = new Vector2(320, 450);
		window.ShowPopup();

		window.Init();
		window.Show();
	}
}