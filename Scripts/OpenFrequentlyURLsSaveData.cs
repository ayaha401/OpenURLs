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

namespace AyahaTools.OpenURLs
{
    [CreateAssetMenu(menuName ="AyahaTools/OpenURLs/SaveData")]
    public class OpenFrequentlyURLsSaveData : ScriptableObject
    {
        [SerializeField] public List<URL_SObj> _URLs = new List<URL_SObj>();

        public void UpdateData(List<URL_SObj> urls)
        {
            _URLs = urls;
        }
    }
}

