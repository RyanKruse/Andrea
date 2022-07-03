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

public class Andrea : UdonSharpBehaviour
{
    [Header("Populated Data Variables From Inspector:")]
    [SerializeField] public Memory Memory;
    [SerializeField] private bool _isScrollWheelActive;
    [SerializeField] private bool _isRejectNumPadInputs;
    [SerializeField] private bool _isDontExecuteScript;
    [SerializeField] private bool _isSkipAppendFlush;
    [SerializeField] private bool _isBookStaged;
    [SerializeField] private int _defaultBlockIndex;
    [SerializeField] private int _defaultFontSizeIndex;
    [SerializeField] private int _defaultFontTypeIndex;
    [SerializeField] private int _defaultBookIndex;
    [SerializeField] private int _defaultTurnPageIndex;
    [SerializeField] private string _defaultOrientation;
    [SerializeField] private AudioClip _softSwipeAudio;
    [SerializeField] private AudioClip _hardSwipeAudio;
    [SerializeField] private AudioClip _clickAudio;
    [SerializeField] private AudioClip _optionsAudio;
    [SerializeField] private AudioClip _selectBookAudio;
    [SerializeField] private AudioClip _backAudio;
    [SerializeField] private AudioSource _candleAudioSource;
    [SerializeField] private VRC_Pickup _candleVRCPickup;
    [SerializeField] private GameObject _candleGameObject;
    [SerializeField] private GameObject _compilerGameObject;
    [SerializeField] private GameObject _readingScreenGameObject;
    [SerializeField] private GameObject _mainMenuGameObject;
    [SerializeField] private GameObject _confirmationMenuGameObject;
    [SerializeField] private GameObject _optionsMenuGameObject;
    [SerializeField] private GameObject _backButtonGameObject;
    [SerializeField] private GameObject _categoryGameObject0;
    [SerializeField] private GameObject _categoryGameObject1;
    [SerializeField] private GameObject _categoryGameObject2;
    [SerializeField] private GameObject _categoryGameObject3;
    [SerializeField] private TextAsset _styleSheetTextAsset;
    [SerializeField] private TextMeshProUGUI _helveticaTMP;
    [SerializeField] private TextMeshProUGUI _helveticaCloneTMP;
    [SerializeField] private TextMeshProUGUI _tahomaTMP;
    [SerializeField] private TextMeshProUGUI _tahomaCloneTMP;
    [SerializeField] private TextMeshProUGUI _notoSerifTMP;
    [SerializeField] private TextMeshProUGUI _notoSerifCloneTMP;
    [SerializeField] private TextMeshProUGUI _timesTMP;
    [SerializeField] private TextMeshProUGUI _timesCloneTMP;
    [SerializeField] private TextMeshProUGUI _verdanaTMP;
    [SerializeField] private TextMeshProUGUI _verdanaCloneTMP;
    [SerializeField] private TextMeshProUGUI _avenirTMP;
    [SerializeField] private TextMeshProUGUI _avenirCloneTMP;
    [SerializeField] private TextMeshProUGUI _bottomLeftTMP;
    [SerializeField] private TextMeshProUGUI _bottomLeftCloneTMP;
    [SerializeField] private TextMeshProUGUI _bottomCenterTMP;
    [SerializeField] private TextMeshProUGUI _bottomCenterCloneTMP;
    [SerializeField] private TextMeshProUGUI _bottomRightTMP;
    [SerializeField] private TextMeshProUGUI _bottomRightCloneTMP;
    [SerializeField] private TextMeshProUGUI _topLeftTMP;
    [SerializeField] private TextMeshProUGUI _topLeftCloneTMP;
    [SerializeField] private TextMeshProUGUI _topCenterTMP;
    [SerializeField] private TextMeshProUGUI _topCenterCloneTMP;
    [SerializeField] private TextMeshProUGUI _topRightTMP;
    [SerializeField] private TextMeshProUGUI _topRightCloneTMP;
    [SerializeField] private TextMeshProUGUI[] _fontSizeTextList;
    [SerializeField] private TextMeshProUGUI[] _fontTypeTextList;
    [SerializeField] private TextMeshProUGUI[] _tabletScaleTextList;
    [SerializeField] private TextMeshProUGUI[] _tabletOrientationTextList;

    [Header("Fixed Data Variables From Field:")]
    [SerializeField] private int _cloneDelay;
    [SerializeField] private int[] _header1FontSizePercentList;
    [SerializeField] private int[] _header2FontSizePercentList;
    [SerializeField] private int[] _header3FontSizePercentList;
    [SerializeField] private int[] _fontSizeList;
    [SerializeField] private float _cloneCycle;
    [SerializeField] private char[] _badCharList;
    [SerializeField] private char[] _bracketCharList;
    [SerializeField] private string[] _fontTypeList;
    [SerializeField] private string[] _locationCodeList;

    [Header("Andrea Runtime Device Status:")]
    [SerializeField] private int _mainBlockIndex;
    [SerializeField] private int _mainFontSize;
    [SerializeField] private int _mainFontSizeIndex;
    [SerializeField] private int _mainPageLength;
    [SerializeField] private int _mainPageIndex;
    [SerializeField] private int _mainCharZeroIndex;
    [SerializeField] private int _mainFontTypeIndex;
    [SerializeField] private int _mainTurnPageIndex;
    [SerializeField] private int _mainLocationHeight;
    [SerializeField] private int _mainBlockHeight;
    [SerializeField] private int _mainMaxHeight;
    [SerializeField] private string _mainFontType;
    [SerializeField] private string _mainOrientation;
    [SerializeField] private string _mainStatusCode;
    [SerializeField] private TextAsset _mainText;
    [SerializeField] private TextAsset _mainBlock;
    [SerializeField] private TextAsset[] _mainTextList;
    [SerializeField] private TextAsset[] _mainBlockList;
    [SerializeField] private TextMeshProUGUI _mainTMP;
    [SerializeField] private TextMeshProUGUI _mainCloneTMP;
    [SerializeField] private TextMeshProUGUI _mainPreviousCloneTMP;

    [Header("Global Workflow Data Variables:")]
    [SerializeField] private bool _isHaltPageTurn;
    [SerializeField] private bool _isRightTriggerActive;
    [SerializeField] private bool _isLeftTriggerActive;
    [SerializeField] private bool _isOverflowAuditDefinePage;
    [SerializeField] private bool _isStringRichText;
    [SerializeField] private bool _isCategoryLerp0;
    [SerializeField] private bool _isCategoryLerp1;
    [SerializeField] private bool _isCategoryLerp2;
    [SerializeField] private bool _isCategoryMinimized0;
    [SerializeField] private bool _isCategoryMinimized1;
    [SerializeField] private bool _isCategoryMinimized2;
    [SerializeField] private bool _isCategoryMinimized3;
    [SerializeField] private bool[] _richTextBoolList;
    [SerializeField] private int _cloneTime;
    [SerializeField] private int _overflowPageIndex;
    [SerializeField] private int _endCalibrationPageIndex;
    [SerializeField] private int[] _lastCharSliceList;
    [SerializeField] private float _cloneTick;
    [SerializeField] private string _richTextString;
    [SerializeField] private string[] _richTextStringList;
    [SerializeField] private string[] _richTextOverflowList;
    [SerializeField] public string[] _styleSheetNameList;  // Formatting C#.
    [SerializeField] public string[] _styleSheetTagList;  // Formatting C#.
    [SerializeField] private Vector3 _categoryVectorDefault0;
    [SerializeField] private Vector3 _categoryVectorDefault1;
    [SerializeField] private Vector3 _categoryVectorDefault2;
    [SerializeField] private Vector3 _categoryVectorDefault3;
    [SerializeField] private Vector3 _categoryVectorTarget0;
    [SerializeField] private Vector3 _categoryVectorTarget1;
    [SerializeField] private Vector3 _categoryVectorTarget2;
    [SerializeField] private Vector3 _categoryVectorTarget3;
    [SerializeField] private Vector3 _categoryVectorStart0;
    [SerializeField] private Vector3 _categoryVectorStart1;
    [SerializeField] private Vector3 _categoryVectorStart2;
    [SerializeField] private Vector3 _categoryVectorStart3;



    // JUNK VARIABLES:
    [SerializeField] private int _adjustedLocationHeight;
    [SerializeField] private int _adjustedMaxHeight;
    [SerializeField] private float timer0;
    [SerializeField] private float timer1;
    [SerializeField] private float timer2;

    private void Start()
    {
        if (_isDontExecuteScript) return;
        PopulateStyleSheet(_styleSheetTextAsset);
        DefineMainVariables();
        ExecuteCalibration();

        // JUNK CODE:
        // _defaultColor = _readingScreenGameObject.GetComponent<Image>().color;
        // _blankScreenGameObject.SetActive(false);
        HomeInfo();
        if (Networking.LocalPlayer.displayName != "Local Player 1")
        {
            Debug.Log($"Playername: {Networking.LocalPlayer.displayName}");
            _compilerGameObject.SetActive(false);
        }
    }

    public void PopulateStyleSheet(TextAsset styleSheetTextAsset)  // Parallel Code.
    {
        // Define list size based on how many rows there are in the style sheet text asset.
        int _styleSheetRowCount = 0;
        for (int _styleIndex = 0; _styleIndex < styleSheetTextAsset.text.Length; _styleIndex++)
        {
            if (styleSheetTextAsset.text[_styleIndex] == '\n')
            {
                _styleSheetRowCount++;
            }
        }
        _styleSheetNameList = new string[_styleSheetRowCount];
        _styleSheetTagList = new string[_styleSheetRowCount];

        // Populate style sheet name list and tag list.
        int _state = 0;
        int _index = 0;
        string _text = "";
        for (int _characterIndex = 0; _characterIndex < styleSheetTextAsset.text.Length; _characterIndex++)
        {
            char _character = styleSheetTextAsset.text[_characterIndex];
            if (_character == '\n')
            {
                // Debug.Log($"PopulateStyleSheet() | A New-Line Was Found | _state: {_state} | _index: {_index} | _text: {_text}");

                if (_state == 3)
                {
                    _styleSheetTagList[_index] = _text.Substring(0, _text.Length - 1);
                    _text = "";
                    _index++;
                }
                _state = 0;
            }
            else if (_state == 0 && (_character == '<' || char.IsLetterOrDigit(_character) || _character == '\\'))
            {
                _state = 1;
            }
            else if (_character == '\t')
            {
                if (_state == 1)
                {
                    _styleSheetNameList[_index] = _text;
                    _text = "";
                }
                _state = 2;
            }
            else if (_state == 2 && (_character == '<' || char.IsLetterOrDigit(_character) || _character == '\\'))
            {
                _state = 3;
            }
            if (_state == 1 || _state == 3)
            {
                _text += _character;
            }
        }
    }

    private void DefineMainVariables()
    {
        Memory.PopulateTextAssetList();
        _mainMenuGameObject.SetActive(true);
        _confirmationMenuGameObject.SetActive(false);
        _optionsMenuGameObject.SetActive(false);
        _backButtonGameObject.SetActive(false);
        _topLeftTMP.gameObject.SetActive(false);
        _topLeftCloneTMP.gameObject.SetActive(false);
        _mainTextList = Memory.GetTextAsset(_defaultBookIndex, true);
        _mainBlockList = Memory.GetTextAsset(_defaultBookIndex, false);
        _mainBlockIndex = Mathf.Clamp(_defaultBlockIndex, 0, _mainTextList.Length - 1);
        _mainFontSize = _fontSizeList[_defaultFontSizeIndex];
        _mainFontSizeIndex = _defaultFontSizeIndex;
        _mainTurnPageIndex = _defaultTurnPageIndex;
        _mainFontType = _fontTypeList[_defaultFontTypeIndex].Trim(_badCharList);
        _mainFontTypeIndex = _defaultFontTypeIndex;
        _mainOrientation = _defaultOrientation;
        _overflowPageIndex = -1;
        _richTextBoolList = new bool[10];
        _richTextStringList = new string[10];
        _categoryVectorDefault0 = _categoryGameObject0.transform.localPosition;
        _categoryVectorDefault0 = _categoryGameObject1.transform.localPosition;
        _categoryVectorDefault0 = _categoryGameObject2.transform.localPosition;
        _categoryVectorDefault0 = _categoryGameObject3.transform.localPosition;
        _categoryGameObject1.transform.SetParent(_categoryGameObject0.transform);
        _categoryGameObject2.transform.SetParent(_categoryGameObject1.transform);
        _categoryGameObject3.transform.SetParent(_categoryGameObject2.transform);
        _categoryVectorTarget0 = _categoryVectorDefault0;
        _categoryVectorTarget1 = _categoryVectorDefault1;
        _categoryVectorTarget2 = _categoryVectorDefault2;
        _categoryVectorTarget3 = _categoryVectorDefault3;
        HomeInfo();
    }

    private void ExecuteCalibration()
    {
        if (_isBookStaged)
        {
            Calibrate(0);
            _topCenterTMP.text = $"{_mainBlock.text.Substring(0, _mainBlock.text.IndexOf('\n'))}";
            _topCenterCloneTMP.text = _topCenterTMP.text;
            _mainMenuGameObject.SetActive(false);
            _backButtonGameObject.SetActive(true);
            _topLeftTMP.gameObject.SetActive(true);
            _topLeftCloneTMP.gameObject.SetActive(true);
            _isOverflowAuditDefinePage = true;
        }
    }

    private void Calibrate(int blockIncrementValue)
    {
        DefineMainTMP();
        DefineMainFiles(blockIncrementValue);
        DefineLocation();
        DefineMainPageLength();
        PopulateLastCharSliceList();
        PrepareOverflowAudit();
    }

    private void DefineMainTMP()
    {
        // Can simplify this by getting direct expressions stored in list instead.
        if (_mainTMP != null)
        {
            _mainTMP.text = "";
        }
        if (_defaultFontTypeIndex == 0)
        {
            // CopyRectTransform(_helveticaTMP, _helveticaCloneTMP);
            _mainTMP = _helveticaTMP;
            _mainCloneTMP = _helveticaCloneTMP;
        }
        else if (_defaultFontTypeIndex == 1)
        {
            _mainTMP = _tahomaTMP;
            _mainCloneTMP = _tahomaCloneTMP;
        }
        else if (_defaultFontTypeIndex == 2)
        {
            _mainTMP = _notoSerifTMP;
            _mainCloneTMP = _notoSerifCloneTMP;
        }
        else if (_defaultFontTypeIndex == 3)
        {
            _mainTMP = _timesTMP;
            _mainCloneTMP = _timesCloneTMP;
        }
        else if (_defaultFontTypeIndex == 4)
        {
            _mainTMP = _verdanaTMP;
            _mainCloneTMP = _verdanaCloneTMP;
        }
        else if (_defaultFontTypeIndex == 5)
        {
            _mainTMP = _avenirTMP;
            _mainCloneTMP = _avenirCloneTMP;
        }
    }

    private void CopyRectTransform(TextMeshProUGUI newTMP, TextMeshProUGUI newCloneTMP)
    {
        if (_mainTMP == null) return;
        RectTransform _mainTMPRectTransform = _mainTMP.GetComponent<RectTransform>();
        RectTransform _newTMPRectTransform = newTMP.GetComponent<RectTransform>();
        RectTransform _newCloneTMPRectTransform = newCloneTMP.GetComponent<RectTransform>();
        _newTMPRectTransform.sizeDelta = _mainTMPRectTransform.sizeDelta;
        _newTMPRectTransform.rotation = _mainTMPRectTransform.rotation;
        _newTMPRectTransform.localPosition = _mainTMPRectTransform.localPosition;
        _newCloneTMPRectTransform.sizeDelta = _mainTMPRectTransform.sizeDelta;
        _newCloneTMPRectTransform.rotation = _mainTMPRectTransform.rotation;
        _newCloneTMPRectTransform.localPosition = _mainTMPRectTransform.localPosition;
    }

    private void DefineMainFiles(int blockIncrementValue)
    {
        _mainStatusCode = $"TO:{_mainOrientation},FS:{_mainFontSizeIndex},FT:{_mainFontTypeIndex},PC:";
        _mainBlockIndex += blockIncrementValue;
        _mainText = _mainTextList[_mainBlockIndex];
        _mainBlock = _mainBlockList[_mainBlockIndex];
    }

    private void DefineLocation()
    {
        // Junk Code:
        int _startLocationIndex = _mainBlock.text.IndexOf(_locationCodeList[0]) + _locationCodeList[0].Length;
        int _endLocationIndex = _mainBlock.text.IndexOf(_locationCodeList[1]) - 1;
        _mainLocationHeight = Convert.ToInt32(_mainBlock.text.Substring(_startLocationIndex, _endLocationIndex - _startLocationIndex));
        _startLocationIndex = _mainBlock.text.IndexOf(_locationCodeList[1]) + _locationCodeList[1].Length;
        _endLocationIndex = _mainBlock.text.IndexOf(_locationCodeList[2]) - 1;
        _mainBlockHeight = Convert.ToInt32(_mainBlock.text.Substring(_startLocationIndex, _endLocationIndex - _startLocationIndex));
        _startLocationIndex = _mainBlock.text.IndexOf(_locationCodeList[2]) + _locationCodeList[2].Length;
        _endLocationIndex = _mainBlock.text.IndexOf('\n', _startLocationIndex) - 1;
        _mainMaxHeight = Convert.ToInt32(_mainBlock.text.Substring(_startLocationIndex, _endLocationIndex - _startLocationIndex));
    }

    private void DefineMainPageLength()
    {
        int _startPageLengthIndex = _mainBlock.text.IndexOf(_mainStatusCode) + _mainStatusCode.Length;
        int _endPageLengthIndex = _mainBlock.text.IndexOf('\n', _startPageLengthIndex);
        string _pageLengthString = _mainBlock.text.Substring(_startPageLengthIndex, _endPageLengthIndex - _startPageLengthIndex);
        _mainPageLength = Convert.ToInt32(_pageLengthString);
        _richTextOverflowList = new string[_mainPageLength];
        _lastCharSliceList = new int[_mainPageLength + 2];  // Account for the double-0 rows.
        _mainCharZeroIndex = _endPageLengthIndex + 1;  // Account for the /n.

        // Debug.Log($"DefineMainPageLength() | _mainPageLength: {_mainPageLength} | _mainCharZeroIndex: {_mainCharZeroIndex}");
    }

    private void PopulateLastCharSliceList()
    {
        int _index = 0;
        string _text = "";
        for (int _characterIndex = _mainCharZeroIndex; _characterIndex < _mainBlock.text.Length; _characterIndex++)
        {
            char _character = _mainBlock.text[_characterIndex];
            if (_character != '\n')
            {
                _text += _character;
                continue;
            }

            // Debug.Log($"PopulateLastCharSliceList() | _index: {_index} | _text: {_text}");

            int _parsedText;
            if (int.TryParse(_text, out _parsedText))
            {
                _lastCharSliceList[_index] = _parsedText;
                _index++;
                _text = "";
            }
            else return;
        }
    }

    private void PrepareOverflowAudit()
    {
        // Debug.Log($"DefineOverflow() | _mainText.name: {_mainText.name} | _mainPageLength: {_mainPageLength} | _endCalibrationPageIndex: {_endCalibrationPageIndex}");

        _overflowPageIndex = 0;
        _endCalibrationPageIndex = _mainPageIndex;
        _mainPageIndex = _overflowPageIndex;
        _mainTMP.gameObject.SetActive(false);

        // We want clone to appear when changing chapters.
        if (!_isOverflowAuditDefinePage)
        {
            // _mainCloneTMPGameObject.SetActive(false);
        }
    }

    private void LoopOverflowAudit()
    {
        if (_overflowPageIndex < _mainPageLength)
        {
            // Populate the text of Main TMP.
            _mainPageIndex = _overflowPageIndex;

            // Debug.Log($"LoopOverflowAudit() | _mainPageIndex: {_mainPageIndex} | _lastCharSliceList[_mainPageIndex]: {_lastCharSliceList[_mainPageIndex]} | _lastCharSliceList[_mainPageIndex + 1]: {_lastCharSliceList[_mainPageIndex + 1]} | Char Count Of Sliced Page: {_lastCharSliceList[_mainPageIndex] - _lastCharSliceList[_mainPageIndex + 1]}");

            int _startIndex = _mainPageIndex == 0 ? 0 : _lastCharSliceList[_mainPageIndex] + 1;
            int _endIndex = _lastCharSliceList[_mainPageIndex + 1] - _lastCharSliceList[_mainPageIndex];
            _mainTMP.text = _mainText.text.Substring(_startIndex, _endIndex);

            // Processes the text so all rich-text is TextMeshPro compatible.
            DecompressRichText(true);
            InsertRichText();
            ReplaceRichText();
            RemoveImageRichText();
            DefineRichTextOverflowElements();
            PopulateRichTextOverflowList();
            _overflowPageIndex++;
        }
        else ExitOverflowAudit();
    }

    private void ExitOverflowAudit()
    {
        _overflowPageIndex = -1;
        _mainPageIndex = _endCalibrationPageIndex == -1 ? _mainPageLength - 1 : _endCalibrationPageIndex;
        _mainPageIndex = Mathf.Clamp(_mainPageIndex, 0, _mainPageLength - 1);
        _endCalibrationPageIndex = 0;

        // Debug.Log($"LoopOverflowAudit() | Audit Complete! | _mainPageIndex: {_mainPageIndex}");

        // Refresh page and make TMP visible again.
        if (_isOverflowAuditDefinePage)
        {
            DefinePage(_mainPageIndex, true);
            _isOverflowAuditDefinePage = false;
        }
        _mainTMP.gameObject.SetActive(true);
        _mainCloneTMP.gameObject.SetActive(true);
    }

    private void DecompressRichText(bool isAppendNewLine)
    {
        // Replaces all rich-text in text with custom rich-text.
        for (int _index = 0; _index < _styleSheetNameList.Length; _index++)
        {
            _mainTMP.text = _mainTMP.text.Replace(_styleSheetNameList[_index], _styleSheetTagList[_index]);
        }

        // Define variables.
        bool _isAfterNewLine = false;
        bool _isBullet = false;
        bool _isBreakBullet = false;
        bool _isDelayBreakBullet = false;
        bool _isDelayedExitBullet = false;
        bool _isTabbedBullet = false;
        bool _isIgnoreNextBullet = false;

        char _firstCharacterThisPage = _mainTMP.text[0];
        char _firstCharacterNextPage = _mainText.text[_lastCharSliceList[_mainPageIndex + 1]];
        char _lastCharacterLastPage = _mainText.text[_lastCharSliceList[_mainPageIndex]];

        string _tagStartBullet = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<START>")];
        string _tagBreakBullet = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<BREAK>")];
        string _tagExitBullet = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<RETURN>")];
        string _tagWideLeft = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<N-LEFT>")];
        string _tagBoldLeft = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<B-LEFT>")];
        string _tagNormalLeft = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<LEFT>")];
        string _tagBoldRight = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<BOLD_RIGHT>")];
        string _tagNormalRight = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<REG_RIGHT>")];
        string _tagTabBullet = _styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<TAB_BULLET>")];
        string _leftInsert = "";
        string _rightInsert = "";

        // Check if we want to append a new line to the text.
        if (isAppendNewLine && _firstCharacterNextPage == '\n')
        {
            _mainTMP.text += "\n";
        }

        // Check for bullet overflow. Check if text appears after a new line.
        if (_mainPageIndex > 0)
        {
            char _secondLastCharacterLastPage = _mainText.text[_lastCharSliceList[_mainPageIndex] - 1];
            string _richTextOverflow = _richTextOverflowList[_mainPageIndex - 1];
            if (_richTextOverflow.Contains("<indent="))
            {
                _isDelayBreakBullet = true;
                _isDelayedExitBullet = true;
                _isBullet = true;
            }
            else if (_richTextOverflow.Contains(_tagStartBullet) && _firstCharacterThisPage != '\n')
            {
                _isAfterNewLine = true;
            }
            else if ((_lastCharacterLastPage == '\n' || _secondLastCharacterLastPage == '\n') && _firstCharacterThisPage != '\n' && IsCharacterABullet(_firstCharacterThisPage, 0))
            {
                _isAfterNewLine = true;
            }
        }

        // Debug.Log($"_isDelayBreakBullet: {_isDelayBreakBullet} | _isDelayedExitBullet: {_isDelayedExitBullet} | _isBullet: {_isBullet} | _isAfterNewLine: {_isAfterNewLine} | Before Loop Execution!");

        // Loops through every character on the page. Formats rich-text for bulleting. 
        for (int _characterIndex = 0; _characterIndex < _mainTMP.text.Length; _characterIndex++)
        {
            char _character = _mainTMP.text[_characterIndex];

            // Debug.Log($"_mainBlockIndex: {_mainBlockIndex} | _mainPageIndex: {_mainPageIndex} | _characterIndex: {_characterIndex} | _character: {_character} | Single Loop Execution!");

            // Inserts indent rich-text to the right of the bullet.
            if (_rightInsert != "" && Array.IndexOf(_bracketCharList, _character) >= 0)
            {
                // Debug.Log($"_mainBlockIndex: {_mainBlockIndex} | _isBreakBullet: {_isBreakBullet} | _rightInsert: {_rightInsert} | _mainPageIndex: {_mainPageIndex} | _characterIndex: {_characterIndex} | _character: {_character} | Right Insert!");

                int _insertIndex = _characterIndex + 1;
                if (_isTabbedBullet)
                {
                    int _swapNumIndex = _rightInsert.IndexOf("<indent=", 0) + "<indent=".Length;
                    int _swapNum = Int32.Parse(_rightInsert.Substring(_swapNumIndex, 1)) + Int32.Parse(_tagTabBullet);
                    _rightInsert = _rightInsert.Replace($"<indent={ _rightInsert[_swapNumIndex]}", $"<indent={_swapNum}");
                    _insertIndex++;
                }
                _mainTMP.text = _mainTMP.text.Insert(_insertIndex, _rightInsert);
                _characterIndex += _rightInsert.Length;
                _rightInsert = "";
                _isBreakBullet = true;
                _isTabbedBullet = false;
                continue;
            }

            // Breaks the bullet when a new-line appears.
            if (_character == '\n')
            {
                // Debug.Log($"_mainBlockIndex: {_mainBlockIndex} | _isBreakBullet: {_isBreakBullet} | _isDelayBreakBullet: {_isDelayBreakBullet} | _mainPageIndex: {_mainPageIndex} | _characterIndex: {_characterIndex} | _character: {_character} | That's A New-Line!");

                _isAfterNewLine = true;
                if (_isDelayBreakBullet)
                {
                    _isBreakBullet = _isDelayBreakBullet;
                    _isDelayBreakBullet = false;
                    _isIgnoreNextBullet = _characterIndex == 0 ? true : false;
                }
                else if (_isBreakBullet)
                {
                    _isBreakBullet = false;
                    _isDelayedExitBullet = false;
                    _mainTMP.text = _mainTMP.text.Insert(_characterIndex - 1, _tagBreakBullet);
                    _characterIndex += _tagBreakBullet.Length;
                }
                continue;
            }

            // Creates a bullet if the text after a new line is the start of a bullet.
            if (_isAfterNewLine && IsCharacterABullet(_character, _characterIndex))
            {
                // Debug.Log($"_mainBlockIndex: {_mainBlockIndex} | _isBreakBullet: {_isBreakBullet} | _isDelayBreakBullet: {_isDelayBreakBullet} | _mainPageIndex: {_mainPageIndex} | _characterIndex: {_characterIndex} | _character: {_character} | Create Bullet!");

                _isAfterNewLine = false;

                // Edge Case: Catch accidental bullets while already in a bullet loop.
                if (_isIgnoreNextBullet)
                {
                    _isIgnoreNextBullet = false;
                    if (_characterIndex == 1) continue;
                }

                // Begin a bullet loop if not already in one.
                if (!_isBullet)
                {
                    _isBullet = true;
                    _mainTMP.text = _mainTMP.text.Insert(_characterIndex, _tagStartBullet);
                    _characterIndex += _tagStartBullet.Length;
                }

                // Process triple-length bullet.
                if (char.IsDigit(_character) && char.IsDigit(_mainTMP.text[_characterIndex + 1]))
                {
                    _leftInsert = _tagWideLeft;
                    _rightInsert = _tagNormalRight;
                    _isTabbedBullet = _mainTMP.text[_characterIndex + 3] == '​' ? true : false;
                }

                // Process single-length bullet.
                else if (_character == '•')
                {
                    _leftInsert = _tagBoldLeft;
                    _rightInsert = _tagBoldRight;
                    _isTabbedBullet = _mainTMP.text[_characterIndex + 1] == '​' ? true : false;
                }

                // Process double-length bullet.
                else
                {
                    _leftInsert = _tagNormalLeft;
                    _rightInsert = _tagNormalRight;
                    _isTabbedBullet = _mainTMP.text[_characterIndex + 2] == '​' ? true : false;
                }

                // Make the spacing wider if this is a tabbed bullet.
                if (_isTabbedBullet)
                {
                    int _swapNumIndex = _leftInsert.IndexOf("<space=", 0) + "<space=".Length;
                    int _swapNum = Int32.Parse(_leftInsert.Substring(_swapNumIndex, 1)) + Int32.Parse(_tagTabBullet);
                    _leftInsert = _leftInsert.Replace($"<space={_leftInsert[_swapNumIndex]}", $"<space={_swapNum}");
                }

                // Insert the text.
                _mainTMP.text = _mainTMP.text.Insert(_characterIndex, _leftInsert);
                _characterIndex += _leftInsert.Length;
                _characterIndex -= _character == '•' ? 1 : 0;
                continue;
            }

            // Terminate bullet loop.
            if (_isAfterNewLine && _isBullet && !_isDelayedExitBullet)
            {
                _mainTMP.text = _mainTMP.text.Insert(_characterIndex - 2, _tagExitBullet);
                _characterIndex += _tagExitBullet.Length;
                _isBullet = false;
                _isAfterNewLine = false;
                _isBreakBullet = false;
                _rightInsert = "";
                continue;
            }

            // Nothing to process.
            _isAfterNewLine = false;
        }
    }

    private bool IsCharacterABullet(char character, int characterIndex)
    {
        if (char.IsDigit(character))
        {
            return true;
        }
        else if (character == '•')
        {
            return true;
        }
        else if (char.IsLetter(character) && Array.IndexOf(_bracketCharList, _mainTMP.text[characterIndex + 1]) >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InsertRichText()
    {
        // Inserts any rich-text overflow at the start of text.
        if (_mainPageIndex > 0 && _richTextOverflowList[_mainPageIndex - 1] != null)
        {
            _mainTMP.text = _mainTMP.text.Insert(0, _richTextOverflowList[_mainPageIndex - 1]);
        }

        // Insert the proper text size.
        _mainTMP.text = _mainTMP.text.Insert(0, $"<size={_mainFontSize}>");
    }

    private void DefineRichTextOverflowElements()
    {
        // Define and format variables.
        _isStringRichText = false;
        _richTextString = "";
        for (int _index = 0; _index < _richTextBoolList.Length; _index++)
        {
            _richTextBoolList[_index] = false;
        }
        for (int _index = 0; _index < _richTextStringList.Length; _index++)
        {
            _richTextStringList[_index] = null;
        }

        // Identify rich-text overflow elements (rich-text that never had a closing command).
        for (int _characterIndex = 0; _characterIndex < _mainTMP.text.Length; _characterIndex++)
        {
            char _character = _mainTMP.text[_characterIndex];

            // Debug.Log($"_characterIndex: {_characterIndex} | _mainPageIndex: {_mainPageIndex} | _character: {_character} | _isStringRichText: {_isStringRichText} | _richTextString: {_richTextString}");

            // Means a rich-text command has opened or closed.
            if (_character == '<' || _character == '>')
            {
                if (_isStringRichText)
                {
                    bool _isExit = false;

                    // Error out code.
                    if (_richTextString == "" || _richTextString == null || _richTextString.Length == 0)
                    {
                        Debug.Log($"Error: Rich Command has a length of zero! Did you forget to add a tab somewhere?");
                        _richTextString.Substring(0, Int32.MaxValue);
                    }

                    // Debug.Log($"_characterIndex: {_characterIndex} | _mainPageIndex: {_mainPageIndex} | _character: {_character} | _isStringRichText: {_isStringRichText} | _richTextString: {_richTextString}");

                    // Closing command rich-text.
                    if (_richTextString[0] == '/')
                    {
                        _isExit = true;
                        _richTextString = _richTextString.Substring(1);
                    }

                    // Identify single-letter rich-text types (bold, italics, underline, strike-through).
                    if (_richTextString == "b")
                    {
                        _richTextBoolList[0] = !_isExit;
                    }
                    else if (_richTextString == "i")
                    {
                        _richTextBoolList[1] = !_isExit;
                    }
                    else if (_richTextString == "u")
                    {
                        _richTextBoolList[2] = !_isExit;
                    }
                    else if (_richTextString == "s")
                    {
                        _richTextBoolList[3] = !_isExit;
                    }

                    // Error out code.
                    else if (_richTextString.Length < 3)
                    {
                        Debug.Log($"Error: Rich Command ({_richTextString}) wasn't matched! Was capitalization accidently used (<B> instead of <b>)?");
                        _richTextString.Substring(0, Int32.MaxValue);
                    }

                    // Identity multi-letter rich-text types (smallcaps, align, indent, size, font, margin, cspace).
                    else if (_richTextString.Substring(0, 3) == "sma")
                    {
                        _richTextBoolList[4] = !_isExit;
                    }
                    else if (_richTextString.Substring(0, 3) == "ali")
                    {
                        _richTextStringList[0] = _isExit ? null : _richTextString.Substring(_richTextString.LastIndexOf('=') + 1);
                        if (_richTextStringList[0] == "\"justified\"")
                        {
                            _richTextStringList[0] = null;
                        }
                        // Debug.Log($"_mainPageIndex {_mainPageIndex} | _richTextStringList[0]: {_richTextStringList[0]}");
                    }
                    else if (_richTextString.Substring(0, 3) == "ind")
                    {
                        _richTextStringList[1] = _isExit ? null : _richTextString.Substring(_richTextString.LastIndexOf('=') + 1);
                    }
                    else if (_richTextString.Substring(0, 3) == "siz")
                    {
                        _richTextStringList[2] = _richTextString.Substring(_richTextString.LastIndexOf('=') + 1);

                        // If same as default font size, there is no overflow. 
                        if (_richTextStringList[2] == "100%" || _richTextStringList[2] == _mainFontSize.ToString())
                        {
                            _richTextStringList[2] = null;
                        }
                    }
                    else if (_richTextString.Substring(0, 3) == "fon")
                    {
                        _richTextStringList[3] = _isExit ? null : _richTextString.Substring(_richTextString.LastIndexOf('=') + 1);

                        // If same as default font type, there is no overflow. 
                        if (_richTextStringList[3] == '"' + _mainFontType + '"')
                        {
                            _richTextStringList[3] = null;
                        }
                    }
                    else if (_richTextString.Substring(0, 3) == "mar")
                    {
                        _richTextStringList[4] = _isExit ? null : _richTextString.Substring(_richTextString.LastIndexOf('=') + 1);
                    }
                    else if (_richTextString.Substring(0, 3) == "csp")
                    {
                        _richTextStringList[5] = _isExit ? null : _richTextString.Substring(_richTextString.LastIndexOf('=') + 1);
                    }
                    _richTextString = "";
                }
                _isStringRichText = !_isStringRichText;
            }

            // Append character to string if rich-text bracket is open.
            else if (_isStringRichText)
            {
                _richTextString += _character;
            }
        }
    }

    private void PopulateRichTextOverflowList()
    {
        // Converts the rich-text overflow into actual rich-text commands for insertion.
        string _compression = "";
        for (int _index = 0; _index < _richTextBoolList.Length; _index++)
        {
            if (_richTextBoolList[_index])
            {
                if (_index == 0)
                {
                    _compression += "<b>";
                }
                else if (_index == 1)
                {
                    _compression += "<i>";
                }
                else if (_index == 2)
                {
                    _compression += "<u>";
                }
                else if (_index == 3)
                {
                    _compression += "<s>";
                }
                else if (_index == 4)
                {
                    _compression += "<smallcaps>";
                }
            }
        }
        for (int i = 0; i < _richTextStringList.Length; i++)
        {
            if (_richTextStringList[i] != null)
            {
                if (i == 0)
                {
                    _compression += $"<align={_richTextStringList[i]}>";
                }
                else if (i == 1)
                {
                    _compression += $"<indent={_richTextStringList[i]}>";
                }
                else if (i == 2)
                {
                    _compression += $"<size={_richTextStringList[i]}>";
                }
                else if (i == 3)
                {
                    _compression += $"<font={_richTextStringList[i]}>";
                }
                else if (i == 4)
                {
                    _compression += $"<margin={_richTextStringList[i]}>";
                }
                else if (i == 5)
                {
                    _compression += $"<cspace={_richTextStringList[i]}>";
                }
            }
        }
        _richTextOverflowList[_mainPageIndex] = _compression;
    }

    private void Update()
    {
        // Process rich-text overflow.
        if (_overflowPageIndex >= 0)
        {
            ProcessRichTextOverflow();
            return;
        }

        // Process inputs only if book staged.
        if (_isBookStaged)
        {
            ProcessInputs();
        }

        if (_mainMenuGameObject.activeSelf)
        {
            LerpUpdate();
        }

        // Process clone timer.
        if (_mainTMP != null)
        {
            _cloneTick += Time.deltaTime;
            while (_cloneTick >= _cloneCycle)
            {
                _cloneTick -= _cloneCycle;
                if (_mainTMP.text != _mainCloneTMP.text)
                {
                    _cloneTime++;
                    if (_cloneTime >= _cloneDelay)  // Hundreths of Second.
                    {
                        _cloneTime = 0;
                        _mainCloneTMP.text = _mainTMP.text;
                        if (_mainPreviousCloneTMP != null)
                        {
                            _mainPreviousCloneTMP.text = "";
                        }
                    }
                }
            }
        }
        

        // Junk Code:
        /*
        time += Time.deltaTime;
        if (time >= interpolationPeriod)
        {
            time = time - interpolationPeriod;

            if (_mainMenuGameObject.activeSelf)
            {
                HomeInfo();
            }
        }
        */
    }

    private void ProcessRichTextOverflow()
    {
        // Calculate rich-text overflows for the new text block.
        // Removed for now, distracts from reading.
        // _mainCenterInfoTMP.text = $"LOADING... ({(_mainPageLength == 1 ? 100 : _mainPageIndex * 100 / (_mainPageLength - 1))}%)";
        LoopOverflowAudit();

        // Overflow audits are complete.
        if (_overflowPageIndex == -1)
        {
            RefreshBottomGUI();
        }
    }

    private void ProcessInputs()
    {
        // Do not turn page if another menu is active.
        if (_optionsMenuGameObject.activeSelf || _mainMenuGameObject.activeSelf || _confirmationMenuGameObject.activeSelf) return;

        // Desktop input for turning page left or right.
        if (!_isHaltPageTurn && Input.GetKeyDown("[3]") || Input.GetKey("[*]") || (Input.GetAxis("Mouse ScrollWheel") < 0f && _isScrollWheelActive))
        {
            _mainPageIndex++;
            DefinePage(_mainPageIndex, true);
        }
        else if (!_isHaltPageTurn && Input.GetKeyDown("[1]") || Input.GetKey("[/]") || (Input.GetAxis("Mouse ScrollWheel") > 0f && _isScrollWheelActive))
        {
            _mainPageIndex--;
            DefinePage(_mainPageIndex, false);
        }

        // VR input for turning page left or right.
        if (Networking.LocalPlayer.IsUserInVR())
        {
            // Disable active controls if trigger is not pressed hard enough.
            if (_isRightTriggerActive && ((_mainTurnPageIndex == 0 && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") <= 0.26f) ||
                (_mainTurnPageIndex == 2 && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryHandTrigger") <= 0.26f) ||
                (_mainTurnPageIndex == 1 && (Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryHandTrigger") <= 0.26f || Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") <= 0.26f))))
            {
                _isRightTriggerActive = false;
            }
            else if (_isLeftTriggerActive && ((_mainTurnPageIndex == 0 && Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") <= 0.26f) ||
                (_mainTurnPageIndex == 2 && Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryHandTrigger") <= 0.26f) ||
                (_mainTurnPageIndex == 1 && (Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryHandTrigger") <= 0.26f || Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") <= 0.26f))))
            {
                _isLeftTriggerActive = false;
            }

            // Enable active controls is trigger is pressed hard enough.
            else if (!_isRightTriggerActive && ((_mainTurnPageIndex == 0 && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") >= 0.28f) ||
                (_mainTurnPageIndex == 2 && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryHandTrigger") >= 0.28f) ||
                (_mainTurnPageIndex == 1 && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryHandTrigger") >= 0.28f && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") >= 0.28f)))
            {
                _isRightTriggerActive = true;
                _mainPageIndex++;
                DefinePage(_mainPageIndex, true);
            }
            else if (!_isLeftTriggerActive && ((_mainTurnPageIndex == 0 && Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") >= 0.28f) ||
                (_mainTurnPageIndex == 2 && Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryHandTrigger") >= 0.28f) ||
                (_mainTurnPageIndex == 1 && Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryHandTrigger") >= 0.28f && Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") >= 0.28f)))
            {
                _isLeftTriggerActive = true;
                _mainPageIndex--;
                DefinePage(_mainPageIndex, false);
            }
        }

        if (_isRejectNumPadInputs) return;

        // Activate mouse scroll wheel for incrementing pages.
        if (Input.GetKeyDown("[5]") || Input.GetKeyDown("5"))
        {
            _isScrollWheelActive = !_isScrollWheelActive;
        }

        // Change text font size.
        else if (Input.GetKeyDown("[4]") || Input.GetKeyDown("4"))
        {
            ChangeFontSize((_defaultFontSizeIndex + 1) % _fontSizeList.Length);
        }

        // Change text font type.
        else if (Input.GetKeyDown("[6]") || Input.GetKeyDown("6"))
        {
            ChangeFontType((_defaultFontTypeIndex + 1) % _fontTypeList.Length);
        }

        // Expand tablet scale.
        else if (Input.GetKeyDown("[7]"))
        {
            _candleGameObject.transform.localScale = Vector3.Scale(_candleGameObject.transform.localScale, new Vector3(0.9f, 0.9f, 0.9f));
        }

        // Shrink tablet scale.
        else if (Input.GetKeyDown("[9]"))
        {
            _candleGameObject.transform.localScale = Vector3.Scale(_candleGameObject.transform.localScale, new Vector3(1.1f, 1.1f, 1.1f));
        }

        // Move tablet position upward.
        else if (Input.GetKeyDown("[-]"))
        {
            _candleGameObject.transform.Translate(Vector3.up * 0.01f);
        }

        // Move tablet position downward.
        else if (Input.GetKeyDown("[+]"))
        {
            _candleGameObject.transform.Translate(Vector3.down * 0.01f);
        }
    }

    private void ChangeFontSize(int fontSizeIndex)
    {
        PlayAudio(_clickAudio);

        _defaultFontSizeIndex = fontSizeIndex;
        _mainFontSizeIndex = _defaultFontSizeIndex;
        _mainFontSize = _fontSizeList[_defaultFontSizeIndex];

        if (!_mainMenuGameObject.activeSelf)
        {
            _mainTMP.text = "";
            Calibrate(0);
            // Need to add loc logistics (find nearest loc before and after calibration)

            _isOverflowAuditDefinePage = true;
        }
    }

    private void ChangeFontType(int fontTypeIndex)
    {
        PlayAudio(_clickAudio);

        _defaultFontTypeIndex = fontTypeIndex;
        _mainFontTypeIndex = _defaultFontTypeIndex;
        _mainFontType = _fontTypeList[_defaultFontTypeIndex].Trim(_badCharList);

        if (!_mainMenuGameObject.activeSelf)
        {
            _mainTMP.text = "";

            // We clear clone text when changing font types.
            _mainPreviousCloneTMP = _mainCloneTMP;

            Calibrate(0);
            // Need to add loc logistics (find nearest loc before and after calibration)

            _isOverflowAuditDefinePage = true;
        }
    }

    private void ChangeTabletSize(float scaleSize)
    {
        PlayAudio(_clickAudio);

        _candleGameObject.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
    }

    private void DefinePage(int pageIndexUnclamped, bool isIncrement)
    {
        // Junk Code:
        // _blankScreenGameObject.SetActive(true);
        // _blankTahomaTMP.text = _mainTMP.text;

        // Detect if page has exceeded block bounds.
        _mainPageIndex = Mathf.Clamp(_mainPageIndex, 0, _mainPageLength - 1);
        if (pageIndexUnclamped != _mainPageIndex)
        {
            ChangeBlock(pageIndexUnclamped > _mainPageIndex);
            return;
        }

        // Determine if a new-line needs to be added as starting text.
        int _insertIndex = 1;
        if (_mainPageIndex == 0)
        {
            _mainTMP.text = "\n";
        }
        else
        {
            string _richTextOverflow = _richTextOverflowList[_mainPageIndex - 1];
            if (_mainText.text.Substring(_lastCharSliceList[_mainPageIndex] + 1, 1) == "\n")
            {
                _mainTMP.text = "\n";
            }
            else if (_mainText.text.Substring(_lastCharSliceList[_mainPageIndex], 1) == "\n")
            {
                _mainTMP.text = "\n";
            }
            else if (_mainText.text.Substring(_lastCharSliceList[_mainPageIndex] - 1, 2) == "\n ")
            {
                _mainTMP.text = "\n";
            }
            else if (_richTextOverflow.Contains("<indent="))
            {
                _mainTMP.text = "\n";
            }
            else if (_richTextOverflow.Contains(_styleSheetTagList[Array.IndexOf(_styleSheetNameList, "<START>")]))
            {
                _mainTMP.text = "\n";
            }
            else
            {
                // This occurs if no new line exists at the top of this page or the preceding text page.
                _mainTMP.text = "";
                _insertIndex = 0;
            }
        }

        PlayAudio(_softSwipeAudio);

        // Populate the text.
        _mainTMP.text += _mainText.text.Substring(_mainPageIndex == 0 ? 0 : _lastCharSliceList[_mainPageIndex] + 1, _lastCharSliceList[_mainPageIndex + 1] - _lastCharSliceList[_mainPageIndex]);

        // Process the text.
        DecompressRichText(true);
        ReplaceRichText();
        RemoveImageRichText();
        FlushLastCharText();

        // Insert all rich-text.
        if (_mainPageIndex > 0)
        {
            _mainTMP.text = _mainTMP.text.Insert(_insertIndex, _richTextOverflowList[_mainPageIndex - 1] == null ? "" : _richTextOverflowList[_mainPageIndex - 1]);
        }
        _mainTMP.text = _mainTMP.text.Insert(_insertIndex, $"<size={_mainFontSize}>");
        _mainTMP.text = _mainTMP.text.Substring(_insertIndex);  // Removes \n.
        _mainTMP.text = _mainTMP.text.Replace("##", $"<align=\"center\"><size=125><sprite name=\"Image_4\"></size><align=\"justified\">\n");  // Cutoff Picture Bottom.

        // Clone copies the main text.
        // _mainCloneTMP.text = _mainTMP.text;

        // Rare scenario where next page doesn't display any text. Call itself again which will increment the block.
        if (IsPageEmpty())
        {
            _mainTMP.text = "";
            _mainPageIndex += isIncrement ? 1 : -1;
            DefinePage(_mainPageIndex, isIncrement);
            return;
        }

        if (_overflowPageIndex == -1)
        {
            RefreshBottomGUI();
        }
    }

    private void ChangeBlock(bool isIncrement)
    {
        // Return if incrementing down at block index 0.
        if (_mainBlockIndex == 0 && !isIncrement) return;

        // Return if incrementing up at max block index.
        if (_mainBlockIndex == _mainTextList.Length - 1 && isIncrement) return;

        // Page has exceeded bounds, so change text blocks.
        PlayAudio(_hardSwipeAudio);

        _mainTMP.text = "";
        Calibrate(isIncrement ? 1 : -1);
        _isOverflowAuditDefinePage = true;

        // If incrementing up, start at the first page. If incrementing down, start at the last page.
        _endCalibrationPageIndex = isIncrement ? 0 : -1;
    }

    private void RefreshBottomGUI()
    {
        // Junk Code (replaces RefreshBottomGUI() function):
        int _locCurrent = Convert.ToInt32(Math.Max(1, Math.Floor((_mainLocationHeight + _lastCharSliceList[_mainPageIndex]) / 100f)));
        string _locStringRaw = $"Loc {_locCurrent}";

        // If on page index 0, need to check last page index of previous block to see if same locString.
        // If same locString, need to drill in while loops checking the last page indexes of each prior until they do not match.
        // After that, add +1 to the locString for each positive drill that occured. 
        // Same logic applies to the pages as well.
        // Urgently need to remove when looping on overflow page index.]

        int _drillDepth = 1;

        // Debug.Log($"Drill Depth Loop Activated.");

        if (_mainPageIndex == 0)
        {
            // Check to make sure we're not on block 0. Can't drill past it.
            while (_mainBlockIndex - _drillDepth >= 0)
            {
                // Get the block contents of the previous block.
                _mainBlock = _mainBlockList[_mainBlockIndex - _drillDepth];

                // Gets the exact global location index of last character in the block.
                int _startLocationIndex = _mainBlock.text.IndexOf(_locationCodeList[0]) + _locationCodeList[0].Length;

                int _endLocationIndex = _mainBlock.text.IndexOf(_locationCodeList[1]) - 1;


                int _locationHeightOfDrilledBlock = Convert.ToInt32(_mainBlock.text.Substring(_startLocationIndex, _endLocationIndex - _startLocationIndex));

                // Gets the exact global block index of last character in the block.
                int _startLocationIndex2 = _mainBlock.text.IndexOf(_locationCodeList[1]) + _locationCodeList[1].Length;

                int _endLocationIndex2 = _mainBlock.text.IndexOf(_locationCodeList[2]) - 1;


                int _blockHeightOfDrilledBlock = Convert.ToInt32(_mainBlock.text.Substring(_startLocationIndex2, _endLocationIndex2 - _startLocationIndex2));

                // Debug.Log($"_mainBlock: Skipped | _startLocationIndex: {_startLocationIndex} | _endLocationIndex: {_endLocationIndex} | _locationHeightOfDrilledBlock: {_locationHeightOfDrilledBlock} | _locationHeight: {_locationHeight} | _drillDepth: {_drillDepth}");

                if (Math.Floor(_mainLocationHeight / 100f) != Math.Floor((_locationHeightOfDrilledBlock + _blockHeightOfDrilledBlock) / 100f))
                {
                    break;
                }
                _drillDepth++;
            }
        }
        else
        {
            while (_mainPageIndex - _drillDepth >= 0)
            {
                if (Math.Floor(_lastCharSliceList[_mainPageIndex] / 100f) != Math.Floor(_lastCharSliceList[_mainPageIndex - _drillDepth] / 100f))
                {
                    break;
                }
                _drillDepth++;
            }
        }

        _adjustedLocationHeight = _locCurrent + _drillDepth - 1;
        _bottomLeftTMP.text = $"Loc {_adjustedLocationHeight}";
        _bottomLeftCloneTMP.text = _bottomLeftTMP.text;

        // Junk Code:
        // Refreshes the GUI text on the bottom of the page.


        // Display chapter header.
        string _blockName = _mainTextList[_mainBlockIndex].name;
        if (_blockName.IndexOf('-') >= 0)
        {
            _bottomCenterTMP.text = $"{_blockName.Substring(_blockName.IndexOf('-') + 2)}";
            _bottomCenterCloneTMP.text = $"{_blockName.Substring(_blockName.IndexOf('-') + 2)}";
        }
        else
        {
            _bottomCenterTMP.text = $"{_blockName}";
            _bottomCenterCloneTMP.text = $"{_blockName}";
        }

        // Display how far we are into the book percentage-wise.
        _adjustedMaxHeight = Convert.ToInt32(Math.Floor(_mainMaxHeight / 100f));  // Convert.ToInt32(Math.Floor(_maxHeight / 100f));

        double _checkMath = Math.Min(Math.Ceiling((double)_adjustedLocationHeight * 99 / (double)_adjustedMaxHeight), 99);

        // _mainCenterInfoTMP.text = "";  // Loop overflow audits complete.

        // Debug.Log($"Percent Complete: {_checkMath}%"); // 
        // _mainCenterInfoTMP.text = $"{_checkMath}%";
        // _bottomRightTMP.text = $"{ (_mainBlockIndex == 0 ? 0 : Convert.ToInt32(Math.Max(1, _checkMath))) }%";

        if (_mainBlockIndex == _mainTextList.Length - 1 && _mainPageIndex == _mainPageLength - 1)
        {
            _bottomCenterTMP.text = "★";
            _bottomCenterCloneTMP.text = _bottomCenterTMP.text;
            _bottomRightTMP.text = "100%";
        }
        else
        {
            _bottomCenterTMP.text = "";
            _bottomCenterCloneTMP.text = _bottomCenterTMP.text;
            _bottomRightTMP.text = $"{Convert.ToInt32(Math.Max(1, _checkMath))}%";
        }

        _bottomRightCloneTMP.text = _bottomRightTMP.text;
        // $"{Math.Round((double)_adjustedLocationHeight / (double)_adjustedMaxHeight) * 100}";  // $"{(Math.Min(_adjustedLocationHeight, _adjustedMaxHeight) / _adjustedMaxHeight) * 100f}%";

        // Chapter Progress: _mainPercentageInfoTMP.text = $"{(_mainPageLength == 1 ? 100 : _mainPageIndex * 100 / (_mainPageLength - 1))}%";
    }

    private void RemoveImageRichText()  // Parallel Code.
    {
        // Experimental imaging script. Ignore this if the book does not have any images.
        for (int _index = 0; _index < 100000; _index++)
        {
            int _start = _mainTMP.text.IndexOf("<image=");
            if (_start == 0 || _start == -1)
            {
                // Debug.Log($"LOOP TERMINATED. REMOVED ({_index}) RICH-TEXT IMAGE COMMANDS.");
                break;
            }
            int _end = _mainTMP.text.IndexOf("g\">", _start) + "g\">".Length + 2;
            // Add +1 to account for the new line... but can't since newline is not in rich-text brackets.
            _mainTMP.text = _mainTMP.text.Remove(_start, _end - _start);

            // Debug.Log($"REMOVED({_start}, {_end - _start}) | START: {_start} | END: {_end} | SUCCESSFULLY REMOVED");
        }
    }

    private void ReplaceRichText()  // Parallel Code.
    {
        // Replaces rich-text with custom variable settings.
        _mainTMP.text = _mainTMP.text.Replace("##", $"<align=\"center\"><size=125><sprite name=\"Image_4\"></size><align=\"justified\">\n​​");  // There are 2 hairlines at the end of \n.
        _mainTMP.text = _mainTMP.text.Replace("~", "");
        _mainTMP.text = _mainTMP.text.Replace("<size=h1>", $"<size={_mainFontSize * (_header1FontSizePercentList[_defaultFontSizeIndex] / 100f)}>");
        _mainTMP.text = _mainTMP.text.Replace("<size=h2>", $"<size={_mainFontSize * (_header2FontSizePercentList[_defaultFontSizeIndex] / 100f)}>");
        _mainTMP.text = _mainTMP.text.Replace("<size=h3>", $"<size={_mainFontSize * (_header3FontSizePercentList[_defaultFontSizeIndex] / 100f)}>");
        _mainTMP.text = _mainTMP.text.Replace("</size>", $"<size={_mainFontSize}>");
    }

    private void FlushLastCharText()
    {
        // Return if last page, flush is disabled, or alignment is active.
        if (_isSkipAppendFlush) return;
        if (_mainTMP.text.Contains("→")) return;
        if (_mainPageIndex == _mainPageLength - 1) return;
        if (_richTextOverflowList[_mainPageIndex] != null && _richTextOverflowList[_mainPageIndex].Contains("<align=")) return;

        // Return if next page is start of new paragraph, bullet, or section.
        char _firstCharNextPage = char.Parse(_mainText.text.Substring(_lastCharSliceList[_mainPageIndex + 1] + 1, 1));
        if (_firstCharNextPage == '\t' || _firstCharNextPage == '\n')  // Last line of paragraph.
        {
            // Debug.Log("Next Page is a Different Paragraph (New-Line)");
            return;
        }
        if (_mainText.text.Substring(_lastCharSliceList[_mainPageIndex + 1] + 1, 3) == "<t>")
        {
            // Debug.Log("Next Page is Linked Paragraph (Tab)");
            return;
        }
        if (_firstCharNextPage == '•') return;

        // Return if next page is the start of bullet. Needs to scan digits to be sure.
        if (_richTextOverflowList[_mainPageIndex] != null && _richTextOverflowList[_mainPageIndex].Contains("<margin="))
        {
            char _secondCharNextPage = char.Parse(_mainText.text.Substring(_lastCharSliceList[_mainPageIndex + 1] + 2, 1));
            if (char.IsLetterOrDigit(_firstCharNextPage) && Array.IndexOf(_bracketCharList, _secondCharNextPage) >= 0) return;
            char _thirdCharNextPage = char.Parse(_mainText.text.Substring(_lastCharSliceList[_mainPageIndex + 1] + 3, 1));
            if (char.IsLetterOrDigit(_firstCharNextPage) && char.IsLetterOrDigit(_secondCharNextPage) && Array.IndexOf(_bracketCharList, _thirdCharNextPage) >= 0) return;
        }

        // Debug.Log($"Page Index: {pageIndex} | firstCharNextPage: {firstCharNextPage}");

        // Determines the exact index to place the flush text, and whether an end-space needs to be added.
        int _textLength = _mainTMP.text.Length;
        bool _tempIsRich = false;
        bool _isEndSpace = false;
        int _insertIndex = 0;
        for (int _index = 1; _index < _textLength; _index++)
        {
            char _character = _mainTMP.text[_textLength - _index];
            if (_character == '<' || _character == '>')
            {
                _tempIsRich = !_tempIsRich;
            }
            else if (!_tempIsRich && _character == ' ')
            {
                _isEndSpace = true;
            }
            else if (!_tempIsRich && (_character != ' ' && _character != '\n' && _character != '\t'))
            {
                _insertIndex = _textLength + 1 - _index;
                break;
            }
        }
        _mainTMP.text = _mainTMP.text.Insert(_insertIndex, $"<align=\"flush\">{(_isEndSpace ? "" : " ")}");
    }

    private bool IsPageEmpty()
    {
        // This checks if the current page is empty or not and returns a boolean.
        _isStringRichText = false;
        bool _isPageEmpty = true;
        foreach (char _character in _mainTMP.text)
        {
            if (_character == '<' || _character == '>')
            {
                _isStringRichText = !_isStringRichText;
            }
            else if (!_isStringRichText && (_character == '.' || char.IsLetterOrDigit(_character)) || _character == '"' || _character == '\'' || _character == '.' || _character == ',' || _character == '=')
            {
                _isPageEmpty = false;
                break;
            }
        }
        return _isPageEmpty;
    }

    public void CalibrateMemory(int index)
    {
        // Called by memory script for processing.
        PlayAudio(_selectBookAudio);

        _mainPageIndex = 0;
        _mainBlockIndex = _defaultBlockIndex;
        _mainTextList = Memory.GetTextAsset(index, true);
        _mainBlockList = Memory.GetTextAsset(index, false);
        if (_mainTextList == null)
        {
            Debug.Log("_mainTextList variable not assigned!");
        }
        _mainBlockIndex = Mathf.Clamp(_mainBlockIndex, 0, _mainTextList.Length - 1);
        _isBookStaged = true;
        ExecuteCalibration();
    }

    public void BackButton()
    {
        // Called by clicking on the back button.
        PlayAudio(_backAudio);

        if (_optionsMenuGameObject.activeSelf)
        {
            _optionsMenuGameObject.SetActive(false);
        }
        else if (_confirmationMenuGameObject.activeSelf)
        {
            _confirmationMenuGameObject.SetActive(false);
        }
        else if (!_mainMenuGameObject.activeSelf)
        {
            _confirmationMenuGameObject.SetActive(true);
        }
        else
        {
            _isBookStaged = false;
            _mainTMP.text = "";
            _mainCloneTMP.text = "";
            _mainMenuGameObject.SetActive(true);
            _backButtonGameObject.SetActive(false);
            _topLeftTMP.gameObject.SetActive(false);
            _topLeftCloneTMP.gameObject.SetActive(false);
            HomeInfo();
        }
    }

    public void HomeInfo()
    {
        // _mainPercentageInfoTMP.text = DateTime.Now.ToShortTimeString();

        // Junk Code: Keep displays to a minimum.
        _topCenterTMP.text = "Home";
        _topCenterCloneTMP.text = _topCenterTMP.text;
        _bottomLeftTMP.text = "";
        _bottomLeftCloneTMP.text = _bottomLeftTMP.text;
        _bottomCenterTMP.text = "";
        _bottomCenterCloneTMP.text = _bottomCenterTMP.text;
        _bottomRightTMP.text = "";
        _bottomRightCloneTMP.text = _bottomRightTMP.text;

    }

    public void OptionsButton()
    {
        // Called by clicking on the catalog button.
        PlayAudio(_optionsAudio);

        if (_optionsMenuGameObject.activeSelf)
        {
            _optionsMenuGameObject.SetActive(false);
        }
        else
        {
            _optionsMenuGameObject.SetActive(true);
        }
    }

    public void SmallSize()
    {
        ChangeFontSize(0);
    }

    public void MediumSize()
    {
        ChangeFontSize(1);
    }

    public void LargeSize()
    {
        ChangeFontSize(2);
    }

    public void HelveticaType()
    {
        if (_defaultFontTypeIndex != 0)
        {
            ChangeFontType(0);
        }
    }

    public void TahomaType()
    {
        if (_defaultFontTypeIndex != 1)
        {
            ChangeFontType(1);
        }
    }

    public void NotoSerifType()
    {
        if (_defaultFontTypeIndex != 2)
        {
            ChangeFontType(2);
        }
    }

    public void TimesType()
    {
        if (_defaultFontTypeIndex != 3)
        {
            ChangeFontType(3);
        }
    }

    public void VerdanaType()
    {
        if (_defaultFontTypeIndex != 4)
        {
            ChangeFontType(4);
        }
    }

    public void AvenirType()
    {
        if (_defaultFontTypeIndex != 5)
        {
            ChangeFontType(5);
        }
    }

    public void SmallScale()
    {
        ChangeTabletSize(0.23f);
    }

    public void MediumScale()
    {
        ChangeTabletSize(0.30f);
    }

    public void LargeScale()
    {
        ChangeTabletSize(0.37f);
    }

    public void EnabledPickup()
    {
        PlayAudio(_clickAudio);

        _candleVRCPickup.pickupable = true;
        _candleGameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void DisabledPickup()
    {
        PlayAudio(_clickAudio);

        _candleVRCPickup.pickupable = false;
        _candleGameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void EnabledMovement()
    {
        PlayAudio(_clickAudio);

        Networking.LocalPlayer.Immobilize(false);
    }

    public void DisabledMovement()
    {
        PlayAudio(_clickAudio);

        Networking.LocalPlayer.Immobilize(true);
    }

    public void TriggerTurnPage()
    {
        PlayAudio(_clickAudio);

        _defaultTurnPageIndex = 0;
        _mainTurnPageIndex = _defaultTurnPageIndex;
    }

    public void BothTurnPage()
    {
        PlayAudio(_clickAudio);

        _defaultTurnPageIndex = 1;
        _mainTurnPageIndex = _defaultTurnPageIndex;
    }

    public void GripTurnPage()
    {
        PlayAudio(_clickAudio);

        _defaultTurnPageIndex = 2;
        _mainTurnPageIndex = _defaultTurnPageIndex;
    }

    public void YesConfirm()
    {
        _mainMenuGameObject.SetActive(true);
        _confirmationMenuGameObject.SetActive(false);
        _backButtonGameObject.SetActive(false);
        _topLeftTMP.gameObject.SetActive(false);
        _topLeftCloneTMP.gameObject.SetActive(false);
        BackButton();
    }

    public void NoConfirm()
    {
        _confirmationMenuGameObject.SetActive(false);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (_candleAudioSource.isPlaying && clip == _softSwipeAudio) return;
        _candleAudioSource.clip = clip;
        _candleAudioSource.Play();
    }

    public void LerpUpdate()
    {
        if (_isCategoryLerp0 && timer0 > 0f)
        {
            _categoryGameObject1.transform.localPosition = Vector3.Lerp(_categoryVectorStart1, _categoryVectorTarget1, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, timer0)));
            if (timer0 >= 1)
            {
                _isCategoryLerp0 = false;
            }
            timer0 += Time.deltaTime * 0.8f;
        }
        if (_isCategoryLerp1 && timer1 > 0f)
        {
            _categoryGameObject2.transform.localPosition = Vector3.Lerp(_categoryVectorStart2, _categoryVectorTarget2, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, timer1)));
            if (timer1 >= 1)
            {
                _isCategoryLerp1 = false;
            }
            timer1 += Time.deltaTime * 0.8f;
        }
        if (_isCategoryLerp2 && timer2 > 0f)
        {
            _categoryGameObject3.transform.localPosition = Vector3.Lerp(_categoryVectorStart3, _categoryVectorTarget3, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, timer2)));
            if (timer2 >= 1)
            {
                _isCategoryLerp2 = false;
            }
            timer2 += Time.deltaTime * 0.8f;
        }
    }

    public void CategoryMinimize0()
    {
        _isCategoryLerp0 = true;
        if (_isCategoryMinimized0)
        {
            timer0 = 0.001f;
            _categoryVectorStart1 = _categoryGameObject1.transform.localPosition;
            _categoryVectorTarget1 = _categoryVectorDefault1;
            _isCategoryMinimized0 = false;
        }
        else
        {
            timer0 = 0.001f;
            _categoryVectorStart1 = _categoryGameObject1.transform.localPosition;
            _categoryVectorTarget1 = _categoryVectorDefault1 + new Vector3(0f, 800f, 0f);
            _isCategoryMinimized0 = true;
        }
    }

    public void CategoryMinimize1()
    {
        _isCategoryLerp1 = true;
        if (_isCategoryMinimized1)
        {
            timer1 = 0.001f;
            _categoryVectorStart2 = _categoryGameObject2.transform.localPosition;
            _categoryVectorTarget2 = _categoryVectorDefault2;
            _isCategoryMinimized1 = false;
        }
        else
        {
            timer1 = 0.001f;
            _categoryVectorStart2 = _categoryGameObject2.transform.localPosition;
            _categoryVectorTarget2 = _categoryVectorDefault2 + new Vector3(0f, 800f, 0f);
            _isCategoryMinimized1 = true;
        }
    }

    public void CategoryMinimize2()
    {
        _isCategoryLerp2 = true;
        if (_isCategoryMinimized2)
        {
            timer2 = 0.001f;
            _categoryVectorStart3 = _categoryGameObject3.transform.localPosition;
            _categoryVectorTarget3 = _categoryVectorDefault3;
            _isCategoryMinimized2 = false;
        }
        else
        {
            timer2 = 0.001f;
            _categoryVectorStart3 = _categoryGameObject3.transform.localPosition;
            _categoryVectorTarget3 = _categoryVectorDefault3 + new Vector3(0f, 800f, 0f);
            _isCategoryMinimized2 = true;
        }
    }
}
