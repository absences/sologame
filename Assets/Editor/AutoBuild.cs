
using System.IO;
using UnityEditor;
using UnityEngine;
using WeChatWASM;
public class AutoBuild
{
    
    [MenuItem("Build/BuildPlatform")]
    public static void BuildWXMiniGame()
    {
        const string CDN = "http://172.16.1.20/";
        WXConvertCore.config.ProjectConf.Appid = "xxxxxx";

        WXConvertCore.config.ProjectConf.bundleExcludeExtensions = "json;version;";

        var path = string.Format("{0}/../AutoBuild/wx_export", Application.dataPath);

        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);

        WXConvertCore.config.ProjectConf.relativeDST = path;

        WXConvertCore.config.ProjectConf.CDN = CDN;
        WXConvertCore.config.CompileOptions.Webgl2 = true;

        EditorUtility.SetDirty(WXConvertCore.config);

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        WXConvertCore.DoExport();
    }
}
