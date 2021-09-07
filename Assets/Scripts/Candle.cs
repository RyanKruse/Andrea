﻿// Author: Ryan Kruse
// VRChat: Clearly
// Discord: Clearly#3238
// GitHub: https://github.com/RyanKruse/Candle

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

public class Candle : UdonSharpBehaviour
{
    [Header("Variables that must be specified by the inspector before run-time.")]
    [SerializeField] public Wax Wax;
    [SerializeField] private bool _isScrollWheelActive;
    [SerializeField] private bool _isRejectNumPadInputs;
    [SerializeField] private bool _isDontExecuteScript;
    [SerializeField] private bool _isSkipAppendFlush;
    [SerializeField] private bool _isBookPreloaded;
    [SerializeField] private int _defaultBlockIndex;
    [SerializeField] private int _defaultFontSizeIndex;
    [SerializeField] private int _defaultFontTypeIndex;
    [SerializeField] private int _defaultBookIndex;
    [SerializeField] private string _defaultOrientation;
    [SerializeField] private TextAsset _styleSheetTextAsset;
    [SerializeField] private VRC_Pickup _candleVRCPickup;
    [SerializeField] private GameObject _candleGameObject;
    [SerializeField] private GameObject _mainMenuGameObject;
    [SerializeField] private GameObject _helveticaTMPGameObject;
    [SerializeField] private GameObject _helveticaCloneTMPGameObject;
    [SerializeField] private GameObject _tahomaTMPGameObject;
    [SerializeField] private GameObject _tahomaCloneTMPGameObject;
    [SerializeField] private GameObject _baskervilleTMPGameObject;
    [SerializeField] private GameObject _baskervilleCloneTMPGameObject;
    [SerializeField] private TextMeshProUGUI _helveticaTMP;
    [SerializeField] private TextMeshProUGUI _helveticaCloneTMP;
    [SerializeField] private TextMeshProUGUI _tahomaTMP;
    [SerializeField] private TextMeshProUGUI _tahomaCloneTMP;
    [SerializeField] private TextMeshProUGUI _baskervilleTMP;
    [SerializeField] private TextMeshProUGUI _baskervilleCloneTMP;
    [SerializeField] private TextMeshProUGUI _verticalPageInfoTMP;
    [SerializeField] private TextMeshProUGUI _verticalPercentageInfoTMP;
    [SerializeField] private TextMeshProUGUI _horizontalPageInfoTMP;
    [SerializeField] private TextMeshProUGUI _horizontalPercentageInfoTMP;

    [Header("Lists that are already pre-defined (Set By C# Script).")]
    [SerializeField] private int[] _header1FontSizePercentList = { 190, 175, 160 };
    [SerializeField] private int[] _header2FontSizePercentList = { 140, 128, 120 };
    [SerializeField] private int[] _header3FontSizePercentList = { 118, 115, 112 };
    [SerializeField] private int[] _fontSizeList = { 30, 35, 40 };
    [SerializeField] private char[] _badCharList = { '\n', ' ', '\t' };
    [SerializeField] private char[] _bracketCharList = { ')', '.', '-', ':', '|', '•' };
    [SerializeField] private string[] _fontTypeList = { "Helvetica Neue SDF", "Tahoma Regular font SDF", "BaskervilleBT SDF" };

    [Header("Variables that signal the state of Candle (Don't Populate).")] 
    [SerializeField] private int _mainBlockIndex;
    [SerializeField] private int _mainFontSize;
    [SerializeField] private int _mainFontSizeIndex;
    [SerializeField] private int _mainPageLength;
    [SerializeField] private int _mainPageIndex;
    [SerializeField] private int _mainCharZeroIndex;
    [SerializeField] private int _mainFontTypeIndex;
    [SerializeField] private string _mainFontType;
    [SerializeField] private string _mainOrientation;
    [SerializeField] private string _mainStatusCode;
    [SerializeField] private TextAsset _mainText;
    [SerializeField] private TextAsset _mainBlock;
    [SerializeField] private TextAsset[] _mainTextList;
    [SerializeField] private TextAsset[] _mainBlockList;
    [SerializeField] private GameObject _mainTMPGameObject;
    [SerializeField] private GameObject _mainCloneTMPGameObject;
    [SerializeField] private TextMeshProUGUI _mainTMP;
    [SerializeField] private TextMeshProUGUI _mainCloneTMP;
    [SerializeField] private TextMeshProUGUI _mainPageInfoTMP;
    [SerializeField] private TextMeshProUGUI _mainPercentageInfoTMP;

    [Header("Variables that are utilized in the entire workflow (Don't Populate).")]
    [SerializeField] private bool _isOverflowAuditDefinePage;
    [SerializeField] private int _overflowPageIndex;
    [SerializeField] private int _endCalibrationPageIndex;
    [SerializeField] private int[] _lastCharSliceList;

    [Header("Rich text data structure varaibles (Don't Populate).")]
    [SerializeField] private bool _isStringRichText;
    [SerializeField] private bool[] _richTextBoolList = new bool[10];
    [SerializeField] private string _richTextString;
    [SerializeField] private string[] _richTextStringList = new string[10];
    [SerializeField] private string[] _richTextOverflowList;
    [SerializeField] public string[] _styleSheetNameList;
    [SerializeField] public string[] _styleSheetTagList;

    [Header("GUI data structure variables (Don't Populate).")]
    [SerializeField] private bool _isHaltPageTurn;
    [SerializeField] private bool _isRightTriggerActive;
    [SerializeField] private bool _isLeftTriggerActive;

    private void Start()
    {
        if (_isDontExecuteScript) return;
        PopulateStyleSheet(_styleSheetTextAsset);
        DefineMainVariables();
        ExecuteCalibration();
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
        _mainMenuGameObject.SetActive(true);
        _mainTextList = Wax.GetTextOrBlock(_defaultBookIndex, true);
        _mainBlockList = Wax.GetTextOrBlock(_defaultBookIndex, false);
        _mainBlockIndex = Mathf.Clamp(_defaultBlockIndex, 0, _mainTextList.Length - 1);
        _mainFontSize = _fontSizeList[_defaultFontSizeIndex];
        _mainFontSizeIndex = _defaultFontSizeIndex;
        _mainFontType = _fontTypeList[_defaultFontTypeIndex].Trim(_badCharList);
        _mainFontTypeIndex = _defaultFontTypeIndex;
        _mainOrientation = _defaultOrientation;
        _mainPageInfoTMP = _verticalPageInfoTMP;
        _mainPercentageInfoTMP = _verticalPercentageInfoTMP;
        _overflowPageIndex = -1;
        _horizontalPageInfoTMP.text = "";
        _horizontalPercentageInfoTMP.text = "";
    }

    private void ExecuteCalibration()
    {
        if (_isBookPreloaded)
        {
            Calibrate(0);
            _mainMenuGameObject.SetActive(false);
            _isOverflowAuditDefinePage = true;
        }
    }

    private void Calibrate(int blockIncrementValue)
    {
        DefineMainTMP();
        DefineMainFiles(blockIncrementValue);
        DefineMainPageLength();
        PopulateLastCharSliceList();
        PrepareOverflowAudit();
    }

    private void DefineMainTMP()
    {
        if (_mainTMP != null)
        {
            _mainTMP.text = "";
            _mainCloneTMP.text = "";
        }
        if (_defaultFontTypeIndex == 0)
        {
            CopyRectTransform(_helveticaTMP, _helveticaCloneTMP);
            _mainTMP = _helveticaTMP;
            _mainCloneTMP = _helveticaCloneTMP;
            _mainTMPGameObject = _helveticaTMPGameObject;
            _mainCloneTMPGameObject = _helveticaCloneTMPGameObject;
        }   
        else if (_defaultFontTypeIndex == 1)
        {
            CopyRectTransform(_tahomaTMP, _tahomaCloneTMP);
            _mainTMP = _tahomaTMP;
            _mainCloneTMP = _tahomaCloneTMP;
            _mainTMPGameObject = _tahomaTMPGameObject;
            _mainCloneTMPGameObject = _tahomaCloneTMPGameObject;
        }
        else if (_defaultFontTypeIndex == 2)
        {
            CopyRectTransform(_baskervilleTMP, _baskervilleCloneTMP);
            _mainTMP = _baskervilleTMP;
            _mainCloneTMP = _baskervilleCloneTMP;
            _mainTMPGameObject = _baskervilleTMPGameObject;
            _mainCloneTMPGameObject = _baskervilleCloneTMPGameObject;
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
        _mainTMPGameObject.SetActive(false);
        _mainCloneTMPGameObject.SetActive(false);
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
        _mainTMPGameObject.SetActive(true);
        _mainCloneTMPGameObject.SetActive(true);
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
        // Process rich-text overflow from block change.
        if (ProcessRichTextOverflow()) return;

        // Process input commands if book is loaded.
        if (_isBookPreloaded)
        {
            IncrementPageInput();
            FormatPageInput();
            RotatePageInput();
        }
    }

    private bool ProcessRichTextOverflow()
    {
        // Calculate rich-text overflows for the new text block.
        if (_overflowPageIndex >= 0)
        {
            _mainPageInfoTMP.text = $"LOADING... ({(_mainPageLength == 1 ? 100 : _mainPageIndex * 100 / (_mainPageLength - 1))}%)";
            LoopOverflowAudit();

            // Overflow audits are complete.
            if (_overflowPageIndex == -1)
            {
                RefreshBottomGUI();
            }
            return true;
        }
        return false;
    }

    private void IncrementPageInput()
    {
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
            if (Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") <= 0.26f && _isRightTriggerActive)
            {
                _isRightTriggerActive = false;
            }
            else if (Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") <= 0.26f && _isLeftTriggerActive)
            {
                _isLeftTriggerActive = false;
            }

            // Enable active controls is trigger is pressed hard enough.
            if (Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") >= 0.28f && !_isRightTriggerActive)
            {
                _isRightTriggerActive = true;
                _mainPageIndex++;
                DefinePage(_mainPageIndex, true);
            }
            else if (Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") >= 0.28f && !_isLeftTriggerActive)
            {
                _isLeftTriggerActive = true;
                _mainPageIndex--;
                DefinePage(_mainPageIndex, false);
            }
        }
    }

    private void FormatPageInput()
    {
        if (_isRejectNumPadInputs) return;

        // Activate mouse scroll wheel for incrementing pages.
        if (Input.GetKeyDown("[5]"))
        {
            _isScrollWheelActive = !_isScrollWheelActive;
        }
        
        // Change text font size.
        else if (Input.GetKeyDown("[4]"))
        {
            _defaultFontSizeIndex = (_defaultFontSizeIndex + 1) % _fontSizeList.Length;
            _mainFontSizeIndex = _defaultFontSizeIndex;
            _mainFontSize = _fontSizeList[_defaultFontSizeIndex];
            _mainTMP.text = "";
            Calibrate(0);
            _isOverflowAuditDefinePage = true;
        }
        
        // Change text font type.
        else if (Input.GetKeyDown("[6]"))
        {
            _defaultFontTypeIndex = (_defaultFontTypeIndex + 1) % _fontTypeList.Length;
            _mainFontTypeIndex = _defaultFontTypeIndex;
            _mainFontType = _fontTypeList[_defaultFontTypeIndex].Trim(_badCharList);
            _mainTMP.text = "";
            Calibrate(0);
            _isOverflowAuditDefinePage = true;
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

    private void RotatePageInput()  // Parallel Code.
    {
        if (_isRejectNumPadInputs) return;

        // Rotate the tablet orientation. 
        if (Input.GetKeyDown("[0]"))
        {
            // Define variables.
            int _tempPageIndex = _mainPageIndex;
            _mainPageIndex = 0;
            _mainTMP.text = "";
            _mainPageInfoTMP.text = "";
            _mainPercentageInfoTMP.text = "";
            RectTransform _rectTransform = _mainTMP.GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.height, _rectTransform.rect.width);

            // Vertical or horizontal orientation.
            if (_mainOrientation == "V")
            {
                _mainOrientation = "H";
                _defaultOrientation = "H";
                _rectTransform.Rotate(new Vector3(0f, 0f, -90f));
                _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.y, _rectTransform.localPosition.x + 5, 0);
                _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width - 75, _rectTransform.rect.height + 75);
                if (!Networking.LocalPlayer.IsUserInVR())
                {
                    _candleGameObject.transform.Rotate(new Vector3(0f, 0f, 90f));
                }
                _mainPageInfoTMP = _horizontalPageInfoTMP;
                _mainPercentageInfoTMP = _horizontalPercentageInfoTMP;

            }
            else if (_mainOrientation == "H")
            {
                _mainOrientation = "V";
                _defaultOrientation = "V";
                _rectTransform.Rotate(new Vector3(0f, 0f, 90f));
                _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.y - 5, _rectTransform.localPosition.x, 0);
                _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width - 75, _rectTransform.rect.height + 75);
                if (!Networking.LocalPlayer.IsUserInVR())
                {
                    _candleGameObject.transform.Rotate(new Vector3(0f, 0f, -90f));
                }
                _mainPageInfoTMP = _verticalPageInfoTMP;
                _mainPercentageInfoTMP = _verticalPercentageInfoTMP;
            }

            // Have clone mimic the main text component.
            RectTransform _rectTransformClone = _mainCloneTMP.GetComponent<RectTransform>();
            _rectTransformClone.sizeDelta = _rectTransform.sizeDelta;
            _rectTransformClone.rotation = _rectTransform.rotation;
            _rectTransformClone.localPosition = _rectTransform.localPosition;
            Calibrate(0);

            // Overwrites endCalibrationMemory in Calibrate().
            _endCalibrationPageIndex = Mathf.Clamp(_tempPageIndex, 0, _mainPageLength - 1);
            _isOverflowAuditDefinePage = true;
        }
    }

    private void DefinePage(int pageIndexUnclamped, bool isIncrement)
    {
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

        // Populate the text.
        _mainTMP.text += _mainText.text.Substring(_mainPageIndex == 0 ? 0 : _lastCharSliceList[_mainPageIndex] + 1, _lastCharSliceList[_mainPageIndex + 1] - _lastCharSliceList[_mainPageIndex]);

        // Process the text.
        DecompressRichText(true);
        ReplaceRichText();
        RemoveImageRichText();
        FlushLastCharText();
        RefreshBottomGUI();

        // Insert all rich-text.
        if (_mainPageIndex > 0)
        {
            _mainTMP.text = _mainTMP.text.Insert(_insertIndex, _richTextOverflowList[_mainPageIndex - 1] == null ? "" : _richTextOverflowList[_mainPageIndex - 1]);
        }
        _mainTMP.text = _mainTMP.text.Insert(_insertIndex, $"<size={_mainFontSize}>");
        _mainTMP.text = _mainTMP.text.Substring(_insertIndex);  // Removes \n.
        _mainTMP.text = _mainTMP.text.Replace("##", $"<align=\"center\"><size=125><sprite name=\"Image_4\"></size><align=\"justified\">\n");  // Cutoff Picture Bottom.

        // Clone copies the main text.
        _mainCloneTMP.text = _mainTMP.text;

        // Rare scenario where next page doesn't display any text. Call itself again which will increment the block.
        if (IsPageEmpty())
        {
            _mainTMP.text = "";
            _mainPageIndex += isIncrement ? 1 : -1;
            DefinePage(_mainPageIndex, isIncrement);
        }
    }

    private void ChangeBlock(bool isIncrement)
    {
        // Return if incrementing down at block index 0.
        if (_mainBlockIndex == 0 && !isIncrement) return;

        // Return if incrementing up at max block index.
        if (_mainBlockIndex == _mainTextList.Length - 1 && isIncrement) return;

        // Page has exceeded bounds, so change text blocks.
        _mainTMP.text = "";
        Calibrate(isIncrement ? 1 : -1);
        _isOverflowAuditDefinePage = true;

        // If incrementing up, start at the first page. If incrementing down, start at the last page.
        _endCalibrationPageIndex = isIncrement ? 0 : -1;
    }

    private void RefreshBottomGUI()
    {
        // Refreshes the GUI text on the bottom of the page.
        string _blockName = _mainTextList[_mainBlockIndex].name;
        if (_blockName.IndexOf('-') >= 0)
        {
            _mainPageInfoTMP.text = $"{_blockName.Substring(_blockName.IndexOf('-') + 2)}";
        }
        else
        {
            _mainPageInfoTMP.text = $"{_blockName}";
        }
        _mainPercentageInfoTMP.text = $"{(_mainPageLength == 1 ? 100 : _mainPageIndex * 100 / (_mainPageLength - 1))}%";
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
            int _end = _mainTMP.text.IndexOf("jpg\">", _start) + "jpg\">".Length + 2;
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
        _mainPageIndex = 0;
        _mainBlockIndex = _defaultBlockIndex;
        _mainTextList = Wax.GetTextOrBlock(index, true);
        _mainBlockList = Wax.GetTextOrBlock(index, false);
        if (_mainTextList == null)
        {
            Debug.Log("_mainTextList variable not assigned!");
        }
        _mainBlockIndex = Mathf.Clamp(_mainBlockIndex, 0, _mainTextList.Length - 1);
        _isBookPreloaded = true;
        ExecuteCalibration();
    }

    public void BackButton()
    {
        // Called by clicking on the back button.
        _isBookPreloaded = false;
        _mainMenuGameObject.SetActive(true);
        _mainPercentageInfoTMP.text = "0%";
    }

    public void CatalogButton()
    {
        // Called by clicking on the catalog button.
    }
}
