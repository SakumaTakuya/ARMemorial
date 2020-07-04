using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GozaiNASU.AR.Utils
{
    public class DataUploader
    {
#if UNITY_EDITOR
        public static string OpenFile(string title, string folder = "", string ext = "")
        {
            return UnityEditor.EditorUtility.OpenFilePanel(title, folder, ext);
        }
#else
        public static string OpenFile(string title, string folder = "", string ext = "")
        {
            Debug.LogWarning("This method is not supported");
            return string.Empty;
        }
#endif
    }

}
