using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GozaiNASU.AR.Data
{
    [CreateAssetMenu]
    public class MemorialData : ScriptableObject
    {
        public string Sample;
        public string ModelData;
        public List<string> Pictures;

        public DataType DataType;
    }

    public enum DataType
    {
        Asset = 0,
        File
    }
}

