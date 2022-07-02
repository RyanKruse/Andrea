// Author: Ryan C. Kruse
// VRChat: Clearly
// Discord: Clearly Ryan#3238
// GitHub: https://github.com/RyanKruse/Andrea
// Booth: https://clearly.booth.pm/

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.IO;
using UnityEditor;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Linq;

public class Memory : UdonSharpBehaviour
{
    [SerializeField] public Andrea Andrea;
    [SerializeField] private TextAsset[] _00_Text;
    [SerializeField] private TextAsset[] _00_Block;
    [SerializeField] private TextAsset[] _01_Text;
    [SerializeField] private TextAsset[] _01_Block;
    [SerializeField] private TextAsset[] _02_Text;
    [SerializeField] private TextAsset[] _02_Block;
    [SerializeField] private TextAsset[] _03_Text;
    [SerializeField] private TextAsset[] _03_Block;
    [SerializeField] private TextAsset[] _04_Text;
    [SerializeField] private TextAsset[] _04_Block;
    [SerializeField] private TextAsset[] _05_Text;
    [SerializeField] private TextAsset[] _05_Block;
    [SerializeField] private TextAsset[] _06_Text;
    [SerializeField] private TextAsset[] _06_Block;
    [SerializeField] private TextAsset[] _07_Text;
    [SerializeField] private TextAsset[] _07_Block;
    [SerializeField] private TextAsset[] _08_Text;
    [SerializeField] private TextAsset[] _08_Block;
    [SerializeField] private TextAsset[] _09_Text;
    [SerializeField] private TextAsset[] _09_Block;
    [SerializeField] private TextAsset[] _10_Text;
    [SerializeField] private TextAsset[] _10_Block;
    [SerializeField] private TextAsset[] _11_Text;
    [SerializeField] private TextAsset[] _11_Block;
    [SerializeField] private TextAsset[] _12_Text;
    [SerializeField] private TextAsset[] _12_Block;
    [SerializeField] private TextAsset[] _13_Text;
    [SerializeField] private TextAsset[] _13_Block;
    [SerializeField] private TextAsset[] _14_Text;
    [SerializeField] private TextAsset[] _14_Block;
    [SerializeField] private TextAsset[] _15_Text;
    [SerializeField] private TextAsset[] _15_Block;
    [SerializeField] private TextAsset[] _16_Text;
    [SerializeField] private TextAsset[] _16_Block;
    [SerializeField] private TextAsset[] _17_Text;
    [SerializeField] private TextAsset[] _17_Block;
    [SerializeField] private TextAsset[] _18_Text;
    [SerializeField] private TextAsset[] _18_Block;
    [SerializeField] private TextAsset[] _19_Text;
    [SerializeField] private TextAsset[] _19_Block;
    [SerializeField] private TextAsset[] _20_Text;
    [SerializeField] private TextAsset[] _20_Block;
    [SerializeField] private TextAsset[] _21_Text;
    [SerializeField] private TextAsset[] _21_Block;
    [SerializeField] private TextAsset[] _22_Text;
    [SerializeField] private TextAsset[] _22_Block;
    [SerializeField] private TextAsset[] _23_Text;
    [SerializeField] private TextAsset[] _23_Block;
    [SerializeField] private TextAsset[] _24_Text;
    [SerializeField] private TextAsset[] _24_Block;
    [SerializeField] private TextAsset[] _25_Text;
    [SerializeField] private TextAsset[] _25_Block;
    [SerializeField] private TextAsset[] _26_Text;
    [SerializeField] private TextAsset[] _26_Block;
    [SerializeField] private TextAsset[] _27_Text;
    [SerializeField] private TextAsset[] _27_Block;
    [SerializeField] private TextAsset[] _28_Text;
    [SerializeField] private TextAsset[] _28_Block;
    [SerializeField] private TextAsset[] _29_Text;
    [SerializeField] private TextAsset[] _29_Block;
    [SerializeField] private TextAsset[][] _TextAssetList;

    public void PopulateTextAssetList()
    {
        _TextAssetList = new TextAsset[][] { 
            _00_Text, _00_Block,
            _01_Text, _01_Block,
            _02_Text, _02_Block,
            _03_Text, _03_Block,
            _04_Text, _04_Block,
            _05_Text, _05_Block,
            _06_Text, _06_Block,
            _07_Text, _07_Block,
            _08_Text, _08_Block,
            _09_Text, _09_Block,
            _10_Text, _10_Block,
            _11_Text, _11_Block,
            _12_Text, _12_Block,
            _13_Text, _13_Block,
            _14_Text, _14_Block,
            _15_Text, _15_Block,
            _16_Text, _16_Block,
            _17_Text, _17_Block,
            _18_Text, _18_Block,
            _19_Text, _19_Block,
            _20_Text, _20_Block,
            _21_Text, _21_Block,
            _22_Text, _22_Block,
            _23_Text, _23_Block,
            _24_Text, _24_Block,
            _25_Text, _25_Block,
            _26_Text, _26_Block,
            _27_Text, _27_Block,
            _28_Text, _28_Block,
            _29_Text, _29_Block,
        };
    }

    public TextAsset[] GetTextAsset(int compilerIndex, bool isText)
    {
        return _TextAssetList[compilerIndex * 2 + Convert.ToInt32(!isText)];
    }

    public void _00_Function() { Andrea.CalibrateMemory(0); }
    public void _01_Function() { Andrea.CalibrateMemory(1); }
    public void _02_Function() { Andrea.CalibrateMemory(2); }
    public void _03_Function() { Andrea.CalibrateMemory(3); }
    public void _04_Function() { Andrea.CalibrateMemory(4); }
    public void _05_Function() { Andrea.CalibrateMemory(5); }
    public void _06_Function() { Andrea.CalibrateMemory(6); }
    public void _07_Function() { Andrea.CalibrateMemory(7); }
    public void _08_Function() { Andrea.CalibrateMemory(8); }
    public void _09_Function() { Andrea.CalibrateMemory(9); }
    public void _10_Function() { Andrea.CalibrateMemory(10); }
    public void _11_Function() { Andrea.CalibrateMemory(11); }
    public void _12_Function() { Andrea.CalibrateMemory(12); }
    public void _13_Function() { Andrea.CalibrateMemory(13); }
    public void _14_Function() { Andrea.CalibrateMemory(14); }
    public void _15_Function() { Andrea.CalibrateMemory(15); }
    public void _16_Function() { Andrea.CalibrateMemory(16); }
    public void _17_Function() { Andrea.CalibrateMemory(17); }
    public void _18_Function() { Andrea.CalibrateMemory(18); }
    public void _19_Function() { Andrea.CalibrateMemory(19); }
    public void _20_Function() { Andrea.CalibrateMemory(20); }
    public void _21_Function() { Andrea.CalibrateMemory(21); }
    public void _22_Function() { Andrea.CalibrateMemory(22); }
    public void _23_Function() { Andrea.CalibrateMemory(23); }
    public void _24_Function() { Andrea.CalibrateMemory(24); }
    public void _25_Function() { Andrea.CalibrateMemory(25); }
    public void _26_Function() { Andrea.CalibrateMemory(26); }
    public void _27_Function() { Andrea.CalibrateMemory(27); }
    public void _28_Function() { Andrea.CalibrateMemory(28); }
    public void _29_Function() { Andrea.CalibrateMemory(29); }
}