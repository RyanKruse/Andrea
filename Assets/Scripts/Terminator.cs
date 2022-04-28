// Author: Ryan Kruse
// VRChat: Clearly
// Discord: Clearly#3238
// GitHub: https://github.com/RyanKruse

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

public class Terminator : UdonSharpBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _buttonTextList = new TextMeshProUGUI[40];
    [SerializeField] private string[] _authorizedUsers = new string[20];
    [SerializeField] private VRCPlayerApi[] _vrcPlayerAPIList = new VRCPlayerApi[120];
    [SerializeField] private int _pageNumber;
    [SerializeField] private GameObject _securityList;
    [SerializeField] private GameObject _hammerBoxObject;
    [SerializeField] private HammerBox _hammerBox;
    [SerializeField] private int _timer;
    [SerializeField] private GameObject _naughtyBoxBox;
    [SerializeField] public int _hammerLocal;
    [SerializeField] [UdonSynced(UdonSyncMode.None)] public int _hammerSync;
    [SerializeField] private Dropdown _dropDown;
    [SerializeField] private TextMeshProUGUI _debugText;

    private void Start()
    {
        /*
        if (!_IsAuthorizedUser())
        {
            _securityList.SetActive(false);
        }
        else if (_IsAuthorizedUser())
        {
            _naughtyBoxBox.SetActive(false);
        }
        */
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (_IsAuthorizedUser())
        {
            if (player != null)
            {
                for (int i = 0; i < _vrcPlayerAPIList.Length; i++)
                {
                    if (_vrcPlayerAPIList[i] == player)
                    {
                        return;
                    }
                }
                for (int i = 0; i < _vrcPlayerAPIList.Length; i++)
                {
                    if (_vrcPlayerAPIList[i] == null)
                    {
                        _vrcPlayerAPIList[i] = player;
                        return;
                    }
                }
            }
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (_IsAuthorizedUser())
        {
            if (player != null)
            {
                for (int i = 0; i < _vrcPlayerAPIList.Length; i++)
                {
                    if (_vrcPlayerAPIList[i] == player)
                    {
                        _vrcPlayerAPIList[i] = null;
                    }
                }
            }
        }
    }

    public bool _IsAuthorizedUser()
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        return Array.IndexOf(_authorizedUsers, player.displayName) != -1;
    }

    public bool _IsAuthorizedPlayer(VRCPlayerApi player)
    {
        if (player == null) return false;
        return Array.IndexOf(_authorizedUsers, player.displayName) != -1;
    }

    private void _ExecuteButton(int buttonNumber)
    {
        if (_IsAuthorizedUser())
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            int _number = 40 * (-1 + _pageNumber);
            if (_vrcPlayerAPIList[buttonNumber - 1 + _number] == null || _buttonTextList[buttonNumber - 1].text == "-") return;
            if (!_IsAuthorizedPlayer(_vrcPlayerAPIList[buttonNumber - 1 + _number]))
            {
                Networking.SetOwner(_vrcPlayerAPIList[buttonNumber - 1 + _number], _hammerBoxObject);
                _hammerSync = 2;
                _hammerLocal = _hammerSync;
            }
        }
    }

    private void _ExecutePage(int pageNumber)
    {
        if (_IsAuthorizedUser())
        {
            _pageNumber = pageNumber;
            _RefreshButtons();
        }
    }

    private void _RefreshButtons()
    {
        if (_IsAuthorizedUser())
        {
            int _number = 40 * (-1 + _pageNumber);
            for (int i = 0; i < _buttonTextList.Length; i++)
            {
                _buttonTextList[i].text = _vrcPlayerAPIList[i + _number] == null ? "-" : _vrcPlayerAPIList[i + _number].displayName;
            }
            if (Networking.IsOwner(gameObject) && _hammerSync == 1)
            {
                Networking.SetOwner(Networking.LocalPlayer, _hammerBoxObject);
            }
            if (_hammerSync == 0)
            {
                _hammerSync = 1;
            }
        }
    }

    public override void OnDeserialization()
    {
        if (_hammerLocal != _hammerSync && _hammerSync == 2)
        {
            _hammerLocal = _hammerSync;
            _hammerBox._countDownTimer = 2f;
            _hammerBoxObject.SetActive(true);
        }
        else if (_hammerLocal != _hammerSync && _hammerSync == 1)
        {
            _hammerLocal = _hammerSync;
            _hammerBox._countDownTimer = 0f;
        }
    }

    private void Update()
    {
        _timer++;
        if (_timer > 200)
        {
            _timer = 0;
            _RefreshButtons();
        }
        _debugText.text = _dropDown.value.ToString();
    }

    public void _Refresh() { _RefreshButtons(); }
    public void _Page1() { _ExecutePage(1); }
    public void _Page2() { _ExecutePage(2); }
    public void _Page3() { _ExecutePage(3); }
    public void _Button1() { _ExecuteButton(1); }
    public void _Button2() { _ExecuteButton(2); }
    public void _Button3() { _ExecuteButton(3); }
    public void _Button4() { _ExecuteButton(4); }
    public void _Button5() { _ExecuteButton(5); }
    public void _Button6() { _ExecuteButton(6); }
    public void _Button7() { _ExecuteButton(7); }
    public void _Button8() { _ExecuteButton(8); }
    public void _Button9() { _ExecuteButton(9); }
    public void _Button10() { _ExecuteButton(10); }
    public void _Button11() { _ExecuteButton(11); }
    public void _Button12() { _ExecuteButton(12); }
    public void _Button13() { _ExecuteButton(13); }
    public void _Button14() { _ExecuteButton(14); }
    public void _Button15() { _ExecuteButton(15); }
    public void _Button16() { _ExecuteButton(16); }
    public void _Button17() { _ExecuteButton(17); }
    public void _Button18() { _ExecuteButton(18); }
    public void _Button19() { _ExecuteButton(19); }
    public void _Button20() { _ExecuteButton(20); }
    public void _Button21() { _ExecuteButton(21); }
    public void _Button22() { _ExecuteButton(22); }
    public void _Button23() { _ExecuteButton(23); }
    public void _Button24() { _ExecuteButton(24); }
    public void _Button25() { _ExecuteButton(25); }
    public void _Button26() { _ExecuteButton(26); }
    public void _Button27() { _ExecuteButton(27); }
    public void _Button28() { _ExecuteButton(28); }
    public void _Button29() { _ExecuteButton(29); }
    public void _Button30() { _ExecuteButton(30); }
    public void _Button31() { _ExecuteButton(31); }
    public void _Button32() { _ExecuteButton(32); }
    public void _Button33() { _ExecuteButton(33); }
    public void _Button34() { _ExecuteButton(34); }
    public void _Button35() { _ExecuteButton(35); }
    public void _Button36() { _ExecuteButton(36); }
    public void _Button37() { _ExecuteButton(37); }
    public void _Button38() { _ExecuteButton(38); }
    public void _Button39() { _ExecuteButton(39); }
    public void _Button40() { _ExecuteButton(40); }
}
