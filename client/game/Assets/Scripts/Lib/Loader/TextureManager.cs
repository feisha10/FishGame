using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

public class TextureManager : Singleton<TextureManager>
{

    public void SetTexure(RawImage image,string path, bool isForceShow = false)
    {
        string _path = path.ToLower();
        string extension = Path.GetExtension(_path);
        _path = string.IsNullOrEmpty(extension)
                                 ? (_path + ".assetbundle")
                                 : _path.Replace(extension, ".assetbundle");
        ResourceManager.Instance.LoadAsset(_path, (o, p) =>
        {
            if (o != null)
            {
                Texture tex = o as Texture;
                if (image!=null)
                {
                    image.texture = tex;
                    if (isForceShow)
                        image.gameObject.SetActive(true);
                }
            }
        });
    }

    public void GetTexture(string path, Material material)
    {
        string _path = path.ToLower();
        string extension = Path.GetExtension(_path);
        _path = string.IsNullOrEmpty(extension)
                                 ? (_path + ".assetbundle")
                                 : _path.Replace(extension, ".assetbundle");
        ResourceManager.Instance.LoadAsset(_path, (o, p) =>
        {
            if (o != null)
            {
                Texture tex = o as Texture;
                if (material!=null)
                {
                    material.mainTexture = tex;
                }
            }
        });
    }
//    
    public void GetTexture(string path, RawImage rawImage, Action callBack = null)
    {
        string _path = path.ToLower();
        string extension = Path.GetExtension(_path);
        _path = string.IsNullOrEmpty(extension)
                                 ? (_path + ".assetbundle")
                                 : _path.Replace(extension, ".assetbundle");
        ResourceManager.Instance.LoadAsset(_path, (o, p) =>
        {
            if (o != null)
            {
                Texture tex = o as Texture;
                if (rawImage!=null)
                {
                    rawImage.texture = tex;
                }
            }
            if (callBack != null)
            {
                callBack();
            }
        });
    }

    public void GetWebTexture(string url, RawImage rawImage)
    {
        ResourceManager.Instance.LoadAsset(url, (o, p) =>
        {
            if (o != null)
            {
                Texture texture = o as Texture;
                if (rawImage != null)
                {
                    rawImage.texture = texture;
                }
            }
        }, 5);
    }
}
