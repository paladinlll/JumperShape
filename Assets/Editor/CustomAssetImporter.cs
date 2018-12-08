using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class CustomAssetImporter : AssetPostprocessor {

	// Use this for initialization
    #region Methods

    //-------------Pre Processors

    // This event is raised when a texture asset is imported
    private void OnPreprocessTexture() 
    {
        var importer = assetImporter as TextureImporter;
        if (importer != null)
        {
            importer.mipmapEnabled = false;
			importer.spritePixelsPerUnit = 1;
			importer.textureType = TextureImporterType.Sprite;
        }
    }

    #endregion
}
