using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.Rendering;

public class AssetDefaultSetting : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = (ModelImporter)assetImporter;
        modelImporter.isReadable = false;
    }

    void OnPostprocessModel(GameObject go)
    {
        ModelImporter modelImporter = (ModelImporter)assetImporter;
    }

    void OptmizeRenderer(GameObject go)
    {
        SkinnedMeshRenderer smr1 = go.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr1 != null)
        {
            //smr1.motionVectors = false;
            //smr1.skinnedMotionVectors = false;
            smr1.shadowCastingMode = ShadowCastingMode.Off;
            smr1.receiveShadows = false;
            //smr1.lightProbeUsage = LightProbeUsage.Off;
            smr1.reflectionProbeUsage = ReflectionProbeUsage.Off;
        }
        else
        {
            //MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
            MeshRenderer[] mrs = go.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < mrs.Length; i++)
            {
                MeshRenderer mr = mrs[i];
                if (mr != null)
                {
                    //mr.motionVectors = false;
                    mr.shadowCastingMode = ShadowCastingMode.Off;
                    mr.receiveShadows = false;
                    // mr.lightProbeUsage = LightProbeUsage.Off;
                    mr.reflectionProbeUsage = ReflectionProbeUsage.Off;

                    if (mr.sharedMaterial.name == "Default-Material")
                    {
                        Object.DestroyImmediate(mr);
                        mr = null;
                    }
                }
            }
        }
    }

    void OnPreprocessAnimation()
    {
        ModelImporter modelImporter = (ModelImporter)assetImporter;
    }

    void OnPreprocessAudio()
    {
        var audioImporter = (AudioImporter)assetImporter;
        AudioImporterSampleSettings s = audioImporter.defaultSampleSettings;

        s.quality = 0.8f;
        FileInfo info = new FileInfo(audioImporter.assetPath);
        if (info.Length > 100000)
        {
            s.loadType = AudioClipLoadType.CompressedInMemory;
        }

        audioImporter.defaultSampleSettings = s;
    }

    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = assetImporter as TextureImporter;
        textureImporter.mipmapEnabled = false;
        textureImporter.isReadable = false;
        if (assetPath.Contains("Assets/ResourcesLib/AtlasResource"))
        {
            string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePackingTag = AtlasName;
        }
        else
        {
            textureImporter.textureType = TextureImporterType.Default;
        }
    }

    void OnPostprocessTexture(Texture2D texture)
    {

    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (importedAssets != null && importedAssets.Length > 0)
        {
            for (int i = 0; i < importedAssets.Length; i++)
            {
                string asset = importedAssets[i];
                if ((asset.Contains(" ") || asset.Contains(" ")) && asset.Contains("Assets/ResourcesLib"))
                {
                    Debug.LogError(asset + ":::<color=red>文件名包含空格，请修改文件名</color>");
                    if (asset.ToLower().Contains("no name"))
                    {
                        AssetDatabase.DeleteAsset(asset);
                    }
                    else
                    {
                        string temp = asset.Replace(" ", "_");
                        temp = asset.Replace(" ", "_");
                        temp = Path.GetFileNameWithoutExtension(temp);
                        string s = AssetDatabase.RenameAsset(asset, temp);
                        if (string.IsNullOrEmpty(s))
                        {
                            Debug.LogError("自动重命名为:::<color=red>" + temp + "</color>");
                        }
                        else
                        {
                            Debug.LogError("自动重命名失败::" + s);
                        }
                    }
                }
            }
        }
    }

}
