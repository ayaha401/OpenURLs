//====================================================================================================
// OpenURLs         v.2.0.0
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

        private string _message = null;
        private MessageType _messageType = MessageType.Info;

        public bool isOpen = false;

        private Vector2 _startWindowSize = new Vector2(400.0f,260.0f);

        void OnEnable()
        {
            isOpen = true;
            Rect currentPosition = position;
            currentPosition.size = _startWindowSize;
            position = currentPosition;
        }

        private void StringReplace(string str)
        {
            str.Replace(" ","").Replace("　","");
        }

        private void CheckAssetElements()
        {
            _message = null;
            if(string.IsNullOrEmpty(_assetName))
            {
                _message = "AssetName is empty\n";
            }
            else
            {
                StringReplace(_assetName);
            }

            if(string.IsNullOrEmpty(_headerName))
            {
                _message += "HeaderName is empty\n";
            }

            if(string.IsNullOrEmpty(_url))
            {
                _message += "URL is empty";
            }           
        }

        void OnGUI()
        {
            // EditorGUILayout.LabelField($"大きさ{position.size}");
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
                    CheckAssetElements();
                    if(_message != null)
                    {
                        _messageType = MessageType.Error;
                        return;
                    }
                    else
                    {
                        _messageType = MessageType.Info;
                    }

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

                EditorGUILayout.HelpBox(_message, _messageType);
            }
        }

        public void CloseWindow()
        {
            isOpen = false;
            this.Close();
        }
    }
}

