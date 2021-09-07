// Author: Ryan Kruse
// VRChat: Clearly
// Discord: Clearly#3238
// GitHub: https://github.com/RyanKruse/Candle
// Prefab: https://clearly.booth.pm/items/3258223

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

public class Wax : UdonSharpBehaviour
{
    [SerializeField] public Candle Candle;
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

    public TextAsset[] GetTextOrBlock(int compilerIndex, bool isText)
    {
        if (compilerIndex == 0) return isText ? _00_Text : _00_Block;
        if (compilerIndex == 1) return isText ? _01_Text : _01_Block;
        if (compilerIndex == 2) return isText ? _02_Text : _02_Block;
        if (compilerIndex == 3) return isText ? _03_Text : _03_Block;
        if (compilerIndex == 4) return isText ? _04_Text : _04_Block;
        if (compilerIndex == 5) return isText ? _05_Text : _05_Block;
        if (compilerIndex == 6) return isText ? _06_Text : _06_Block;
        if (compilerIndex == 7) return isText ? _07_Text : _07_Block;
        if (compilerIndex == 8) return isText ? _08_Text : _08_Block;
        if (compilerIndex == 9) return isText ? _09_Text : _09_Block;
        if (compilerIndex == 10) return isText ? _10_Text : _10_Block;
        if (compilerIndex == 11) return isText ? _11_Text : _11_Block;
        if (compilerIndex == 12) return isText ? _12_Text : _12_Block;
        if (compilerIndex == 13) return isText ? _13_Text : _13_Block;
        if (compilerIndex == 14) return isText ? _14_Text : _14_Block;
        if (compilerIndex == 15) return isText ? _15_Text : _15_Block;
        if (compilerIndex == 16) return isText ? _16_Text : _16_Block;
        if (compilerIndex == 17) return isText ? _17_Text : _17_Block;
        if (compilerIndex == 18) return isText ? _18_Text : _18_Block;
        if (compilerIndex == 19) return isText ? _19_Text : _19_Block;
        else return null;
    }

    // Each function must be assigned individually.
    public void _00_Function() { Candle.CalibrateMemory(0); }
    public void _01_Function() { Candle.CalibrateMemory(1); }
    public void _02_Function() { Candle.CalibrateMemory(2); }
    public void _03_Function() { Candle.CalibrateMemory(3); }
    public void _04_Function() { Candle.CalibrateMemory(4); }
    public void _05_Function() { Candle.CalibrateMemory(5); }
    public void _06_Function() { Candle.CalibrateMemory(6); }
    public void _07_Function() { Candle.CalibrateMemory(7); }
    public void _08_Function() { Candle.CalibrateMemory(8); }
    public void _09_Function() { Candle.CalibrateMemory(9); }
    public void _10_Function() { Candle.CalibrateMemory(10); }
    public void _11_Function() { Candle.CalibrateMemory(11); }
    public void _12_Function() { Candle.CalibrateMemory(12); }
    public void _13_Function() { Candle.CalibrateMemory(13); }
    public void _14_Function() { Candle.CalibrateMemory(14); }
    public void _15_Function() { Candle.CalibrateMemory(15); }
    public void _16_Function() { Candle.CalibrateMemory(16); }
    public void _17_Function() { Candle.CalibrateMemory(17); }
    public void _18_Function() { Candle.CalibrateMemory(18); }
    public void _19_Function() { Candle.CalibrateMemory(19); }
}