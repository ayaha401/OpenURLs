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
using System.Linq;

namespace AyahaTools.OpenURLs
{
    public class OpenFrequentlyURLs : EditorWindow
    {
        private MakeURLAsset _makeURLAsset= null;
        [SerializeField] private OpenFrequentlyURLsSaveData _data = null;
        [SerializeField] private List<URL_SObj> _URLs = new List<URL_SObj>();

        private SerializedObject so;
        private SerializedProperty _URLsProp;

        private Vector2 _scrollPosition = Vector2.zero;
        private Vector2 _deleteButtonSize = new Vector2(18.0f,18.0f);

        private string _version = "1.1.1";

        [MenuItem("AyahaTools/OpenURLs")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow<OpenFrequentlyURLs>();
        }

        void OnEnable()
        {
            so = new SerializedObject(this);
            _URLsProp = so.FindProperty("_URLs");
        }

        void SoInit()
        {
            so = new SerializedObject(this);
            _URLsProp = so.FindProperty("_URLs");
        }

        void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Version");
                    EditorGUILayout.LabelField("Version " + _version);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("How to use (Japanese)");
                    if(GUILayout.Button("How to use (Japanese)"))
                    {
                        System.Diagnostics.Process.Start("https://github.com/ayaha401/OpenURLs");
                    }
                }
            }

            GUI.color = Color.gray;
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            GUI.color = Color.white;

            if(GUILayout.Button("URLアセットを作成"))
            {
                _makeURLAsset = ScriptableObject.CreateInstance<MakeURLAsset>();
                _makeURLAsset.OpenWindow();
            }

            GUI.color = Color.gray;
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            GUI.color = Color.white;

            using (new EditorGUILayout.VerticalScope("box"))
            {
                if(_data == null) _data = LoadData();
                _URLs = _data._URLs;

                EditorGUI.BeginChangeCheck();

                SoInit();
                EditorGUILayout.PropertyField(_URLsProp,true);

                if(EditorGUI.EndChangeCheck())
                {
                    _data.UpdateData(_URLs);
                    EditorUtility.SetDirty(_data);
                }

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                GUI.color = Color.gray;
                GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
                GUI.color = Color.white;

                for(int i=0;i<_URLsProp.arraySize;i++)
                {
                    if(_URLs[i] != null)
                    {
                        using (new EditorGUILayout.VerticalScope("box"))
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(_URLs[i].URLHeaderName);
                                EditorGUILayout.Space();
                                if(GUILayout.Button("X", GUILayout.Width(_deleteButtonSize.x), GUILayout.Height(_deleteButtonSize.y)))
                                {
                                    _URLsProp.DeleteArrayElementAtIndex(i);
                                }
                            }
                            
                            if(GUILayout.Button("OpenURL"))
                            {
                                System.Diagnostics.Process.Start(_URLs[i].URL);
                            }
                            GUI.color = Color.gray;
                            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
                            GUI.color = Color.white;
                        }
                    }
                }

                EditorGUILayout.EndScrollView();
            }
            so.ApplyModifiedProperties();
        }

        void OnDestroy()
        {
            if(_makeURLAsset == null) return; 
            if(_makeURLAsset.isOpen == true)
            {
                _makeURLAsset.CloseWindow();
            }
        }

        private OpenFrequentlyURLsSaveData LoadData()
        {
            return (OpenFrequentlyURLsSaveData)AssetDatabase.FindAssets("t:ScriptableObject")
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(OpenFrequentlyURLsSaveData)))
                    .Where(obj => obj != null)
                    .FirstOrDefault();
        }
    }
}
