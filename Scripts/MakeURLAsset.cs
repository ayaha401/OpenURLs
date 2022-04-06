//====================================================================================================
// OpenURLs         v.1.1.1
//
// Copyright (C) 2022 ayaha401
// Twitter : @ayaha__401
//
// This software is released under the MIT License.
// https://github.com/ayaha401/OpenURLs/blob/main/LICENSE
//====================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AyahaTools.OpenURLs
{
    public class MakeURLAsset : EditorWindow
    {
        private string _assetName = null;
        private string _headerName = null;
        private string _url = null;

        public bool isOpen = false;

        public void OpenWindow()
        {
            isOpen = true;
            EditorWindow.GetWindow<MakeURLAsset>();
        }

        void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField("アセット名");
                _assetName = EditorGUILayout.TextField(_assetName);

                EditorGUILayout.LabelField("見出し名");
                _headerName = EditorGUILayout.TextField(_headerName);

                EditorGUILayout.LabelField("URL");
                _url = EditorGUILayout.TextField(_url);

                if(GUILayout.Button("作成"))
                {
                    const string PATH = "Assets/Editor/AyahaTools/OpenFrequentlyURLs/SObj";
                    URL_SObj newURL_SObj = ScriptableObject.CreateInstance<URL_SObj>();
                    string fileName = $"{_assetName}.asset";
                    newURL_SObj.URLHeaderName = _headerName;
                    newURL_SObj.URL = _url;
                    AssetDatabase.CreateAsset(newURL_SObj, Path.Combine(PATH, fileName));
                    _assetName = null;
                    _headerName = null;
                    _url = null;
                    this.Close();
                }
            }
        }

        public void CloseWindow()
        {
            isOpen = false;
            this.Close();
        }
    }
}

