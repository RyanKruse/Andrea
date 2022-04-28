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

public class HammerBox : UdonSharpBehaviour
{
    [SerializeField] private Terminator _terminator;
    [SerializeField] public float _countDownTimer;
    [SerializeField] private GameObject _vrcWorld;
    [SerializeField] private GameObject _naughtyBoxSpawn;
    [SerializeField] private bool _isBanned;
    [SerializeField] private GameObject _terminatorObject;

    private void Update()
    {
        if (_countDownTimer > 0)
        {
            _countDownTimer -= Time.deltaTime;
        }
        else if (Networking.IsOwner(gameObject) && !_terminator._IsAuthorizedUser() && _terminator._hammerSync == 2 && !_isBanned)
        {
            _isBanned = true;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HammerBoxInactive");
            _Banned();
        }
        else if (_isBanned)  // && Vector3.Distance(Networking.LocalPlayer.GetPosition(), _naughtyBoxSpawn.transform.position) > 4f)
        {
            _Banned();
        }    
    }

    private void _Banned()
    {
        _vrcWorld.transform.position = _naughtyBoxSpawn.transform.position;
        for (int i = 0; i < 100; i++)
        {
            Networking.LocalPlayer.TeleportTo(_naughtyBoxSpawn.transform.position + new Vector3(UnityEngine.Random.Range(-0.001f, 0.001f), UnityEngine.Random.Range(-0.001f, 0.001f), UnityEngine.Random.Range(-0.001f, 0.001f)), Quaternion.identity);
        }
    }

    public void HammerBoxInactive()
    {
        if (!_isBanned && _terminator._hammerSync == 2)
        {
            gameObject.SetActive(false);
            if (Networking.IsOwner(_terminatorObject) && _terminator._IsAuthorizedUser())
            {
                _terminator._hammerSync = 1;
                _terminator._hammerLocal = _terminator._hammerSync;
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
            }
        }
    }
}
