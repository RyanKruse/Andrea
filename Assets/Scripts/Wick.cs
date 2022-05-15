// Author: Ryan Kruse
// VRChat: Clearly
// Discord: Clearly#3238
// GitHub: https://github.com/RyanKruse/Candle
// Prefab: https://clearly.booth.pm/items/3258223

using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.IO;
using System;
using UnityEditor;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;

public class Wick : MonoBehaviour
{
    [Header("Variables that must be specified by the inspector before run-time.")]
    [SerializeField] private int _defaultBookIndex;
    [SerializeField] private bool _isExecuteCompiler;
    [SerializeField] private bool _isScrollWheelActive;
    [SerializeField] private bool _isRejectNumPadInputs;
    [SerializeField] private bool _isVerySlowDebug;
    [SerializeField] private bool _isMaxBlockRestrict;
    [SerializeField] private bool _isOneBlockOnly;
    [SerializeField] private bool _isFixedFontSizeOnly;
    [SerializeField] private bool _isFixedFontTypeOnly;
    [SerializeField] private bool _isSkipOrientation;
    [SerializeField] private int _fixedFontSize;
    [SerializeField] private int _fixedFontType;
    [SerializeField] private int _fixedBlock;
    [SerializeField] private int _maxBlockRestrict;
    [SerializeField] private string _tabletOrientation;
    [SerializeField] private GameObject _candleGameObject;
    [SerializeField] private GameObject _wickGameObject;
    [SerializeField] private GameObject _debugCubeGameObject;
    [SerializeField] private TextAsset _styleSheetTextAsset;
    [SerializeField] private TextMeshProUGUI _wickTMP;
    [SerializeField] private TextMeshProUGUI _refreshTMP;
    [SerializeField] private TextMeshProUGUI _mainTMP;
    [SerializeField] private TMP_FontAsset[] _fontTypeAssetList;

    [Header("Lists that are already pre-defined (Set By C# Script).")]
    [SerializeField] private int[] _header1FontSizePercentList = { 190, 175, 160, 150};
    [SerializeField] private int[] _header2FontSizePercentList = { 140, 128, 120, 115 };
    [SerializeField] private int[] _header3FontSizePercentList = { 118, 115, 112, 110 };
    [SerializeField] private int[] _fontSizeList = { 26, 30, 35, 40 };
    [SerializeField] private char[] _bracketCharList = { ')', '.', '-', ':', '|', '•' };
    [SerializeField] private string[] _tabletOrientationList = { "V", "H" };

    [Header("Variables that are utilized in the entire workflow (Don't Populate).")]
    [SerializeField] private int _updateIndex;
    [SerializeField] private StreamWriter _streamWriter;
    [SerializeField] private string[] _styleSheetNameList;
    [SerializeField] private string[] _styleSheetTagList;
    [SerializeField] private string[] _textFilePath;
    [SerializeField] private string[] _bookTitleList;
    [SerializeField] private int _charTransformIndex;
    [SerializeField] private char _charTransformElement;
    [SerializeField] private int _locationHeight;
    [SerializeField] private int _blockHeight;
    [SerializeField] private int _maxHeight;

    private void DelayedStart()
    {
        // This executes after 10 frames have passed since Start().
        DefineMainVariables();
        PopulateStyleSheet();
        PopulateTextPaths();
        CompileBook();
    }

    private void DefineMainVariables()
    {
        _locationHeight = 0;
        _blockHeight = 0;
        _maxHeight = 0;

        // Ensures whether we restrict the execution or not.
        if (_maxBlockRestrict < 0 || !_isMaxBlockRestrict)
        {
            _maxBlockRestrict = 999999;
        }
    }

    private void PopulateStyleSheet()  // Parallel Code.
    {
        // Populate style sheet name list and tag list.
        Candle _candle = _candleGameObject.GetComponent<Candle>();
        _candleGameObject.GetComponent<Candle>().PopulateStyleSheet(_styleSheetTextAsset);
        _styleSheetNameList = _candle._styleSheetNameList;
        _styleSheetTagList = _candle._styleSheetTagList;
    }

    private void PopulateTextPaths()
    {
        // Gets the directory paths of all book text files and stores them.
        var _directoryInfo = new DirectoryInfo($"{Application.dataPath}/Books");
        var _fileInfo = _directoryInfo.GetFiles();
        _textFilePath = new string[_fileInfo.Length];
        _bookTitleList = new string[_fileInfo.Length];

        for (int _index = 0; _index < _fileInfo.Length; _index++)
        {
            var _file = _fileInfo[_index];
            string _path = _file.ToString().Substring(0, _file.ToString().IndexOf(".meta")) + "/Texts";
            _textFilePath[_index] = _path.Replace('\\', '/');
            _bookTitleList[_index] = _file.Name.Substring(_file.Name.IndexOf('-') + 2, _file.Name.IndexOf(".meta") - 5);

            // Debug.Log($"File: {_file.Name} | Directory: {_file.Directory} | Directory Name: {_file.DirectoryName}");
        }
    }

    private void Update()
    {
        if (!_isExecuteCompiler)
        {
            gameObject.SetActive(false);
            return;
        }

        if (_updateIndex > 10)
        {
            // Refreshes text to show that script hasn't crashed.
            _refreshTMP.text = $"( {UnityEngine.Random.Range(10, 99)} )";

            // Process inputs.
            IncrementPageInput();
            FormatPageInput();
            RotatePageInput();

            // Ensures page numbers are valid.
            _mainTMP.pageToDisplay = Mathf.Clamp(_mainTMP.pageToDisplay, 1, int.MaxValue);
        }
        else
        {
            // Timer for delayed start.
            _updateIndex++;
            if (_updateIndex == 10)
            {
                DelayedStart();
            }
        }
    }

    private void IncrementPageInput()
    {
        // Desktop input for turning page left or right.
        if (Input.GetKeyDown("[3]") || Input.GetKey("[*]") || (Input.GetAxis("Mouse ScrollWheel") < 0f && _isScrollWheelActive))
        {
            _mainTMP.pageToDisplay++;
        }
        else if (Input.GetKeyDown("[1]") || Input.GetKey("[/]") || (Input.GetAxis("Mouse ScrollWheel") > 0f && _isScrollWheelActive))
        {
            _mainTMP.pageToDisplay--;
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

        // Logistics for identifying a character's world position.
        else if (Input.GetKeyDown("[9]"))
        {
            _charTransformIndex++;
            _charTransformElement = _mainTMP.text[_charTransformIndex];
            CharWorldPosition();
        }
        else if (Input.GetKeyDown("[7]"))
        {
            _charTransformIndex--;
            _charTransformIndex = Mathf.Max(_charTransformIndex, 0);
            _charTransformElement = _mainTMP.text[_charTransformIndex];
            CharWorldPosition();
        }
    }

    public void CharWorldPosition()
    {
        // Get center of position for character.
        Vector3 _vectorRaw1 = _mainTMP.textInfo.characterInfo[_charTransformIndex].bottomRight;
        Vector3 _vectorRaw2 = _mainTMP.textInfo.characterInfo[_charTransformIndex].bottomLeft;
        Vector3 _vectorRaw3 = _mainTMP.textInfo.characterInfo[_charTransformIndex].topLeft;
        Vector3 _vectorRaw4 = _mainTMP.textInfo.characterInfo[_charTransformIndex].topRight;
        Vector3 _vectorRaw = (_vectorRaw1 + _vectorRaw2 + _vectorRaw3 + _vectorRaw4) / 4f;

        // Move debug cube to character position.
        Vector3 _vectorProcessed = _mainTMP.gameObject.transform.TransformPoint(_vectorRaw);
        _debugCubeGameObject.SetActive(true);
        _debugCubeGameObject.transform.position = _vectorProcessed;

        // Debug.Log($"Character Index 0 VectorRaw: {_vectorRaw} | VectorProcessed: {_vectorProcessed} | Char Transform Element: {_charTransformElement} | Char Transform Index: {_charTransformIndex}");
    }

    private void RotatePageInput()  // Parallel Code.
    {
        if (_isRejectNumPadInputs) return;

        // Rotate the tablet orientation. 
        if (Input.GetKeyDown("[0]"))
        {
            RectTransform _rectTransform = _mainTMP.GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.height, _rectTransform.rect.width);
            if (_tabletOrientation == "V")
            {
                _tabletOrientation = "H";
                _wickGameObject.transform.Rotate(new Vector3(0f, 0f, 90f));
                _rectTransform.Rotate(new Vector3(0f, 0f, -90f));
                _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.y, _rectTransform.localPosition.x + 5, 0);
            }
            else if (_tabletOrientation == "H")
            {
                _tabletOrientation = "V";
                _wickGameObject.transform.Rotate(new Vector3(0f, 0f, -90f));
                _rectTransform.Rotate(new Vector3(0f, 0f, 90f));
                _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.y - 5, _rectTransform.localPosition.x, 0);
            }
        }
    }

    private void CompileBook()
    {
        // Get the book text paths of book index.
        RectTransform _rectTransform = _mainTMP.GetComponent<RectTransform>();
        var _directoryInfo = new DirectoryInfo(_textFilePath[_defaultBookIndex]);
        var _fileInfo = _directoryInfo.GetFiles();

        // Debug.Log($"File Info: {_fileInfo} | File Info Length: {_fileInfo.Length} | Is null: {_fileInfo == null} | Count of non-meta text files: {_fileInfo.Length / 2}");

        // Define block paths.
        string[] _textPathList = new string[_fileInfo.Length / 2];
        string[] _blockPathList = new string[_fileInfo.Length / 2];
        for (int _index = 0; _index < _fileInfo.Length; _index += 2)
        {
            int _halved = _index / 2;
            _textPathList[_halved] = _fileInfo[_index].ToString();
            _blockPathList[_halved] = _textPathList[_halved].Replace("Texts", "Blocks");
            _blockPathList[_halved] = _blockPathList[_halved].Replace(".txt", " (B).txt");

            // Debug.Log($"Block Path Index {_index / 2}: {_blockPathList[_index / 2]}");
        }

        // Identify how many total characters are in the book.
        for (int _textPathIndex = 0; _textPathIndex < _textPathList.Length; _textPathIndex++)
        {
            StreamReader _streamReader = new StreamReader(_textPathList[_textPathIndex]);
            string _textContent = _streamReader.ReadToEnd();
            _maxHeight += _textContent.Length - 1;
        }

        // Loop to populate block paths.
        for (int _textPathIndex = 0; _textPathIndex < _textPathList.Length; _textPathIndex++)
        {
            if ((_isOneBlockOnly && _textPathIndex != _fixedBlock) || _textPathIndex > _maxBlockRestrict) continue;

            // Reset contents of text. Get text content from path.
            _wickTMP.text = "";
            File.WriteAllText(_blockPathList[_textPathIndex], string.Empty);
            StreamReader _streamReader = new StreamReader(_textPathList[_textPathIndex]);
            string _textContent = _streamReader.ReadToEnd();
            _streamReader.Close();

            // Record the loc and percentage height.
            StreamWriter _streamWriter = new StreamWriter(_blockPathList[_textPathIndex], true);
            _blockHeight = _textContent.Length - 1;
            _streamWriter.WriteLine($"{_bookTitleList[_defaultBookIndex]}");
            _streamWriter.WriteLine($"LH:{_locationHeight},BH:{_blockHeight},MH:{_maxHeight}");
            _locationHeight += _textContent.Length - 1;
            _streamWriter.Close();

            // Loops over tablet orientation.
            foreach (string _tabletOrientation in _tabletOrientationList)
            {
                // Ignores processing tablet orientation if disabled.
                if (_isSkipOrientation && _tabletOrientation == "H")
                {
                    _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.height, _rectTransform.rect.width);
                    continue;
                }

                // Loops over font size.
                for (int _fontSizeIndex = 0; _fontSizeIndex < _fontSizeList.Length; _fontSizeIndex++)
                {
                    if (_fontSizeList[_fontSizeIndex] == 0) continue;

                    // Loops over font type.
                    for (int _fontTypeIndex = 0; _fontTypeIndex < _fontTypeAssetList.Length; _fontTypeIndex++)
                    {
                        if (!_fontTypeAssetList[_fontTypeIndex]) continue;

                        PopulateBlock(_blockPathList, _textContent, _fontSizeIndex, _fontTypeIndex, _textPathIndex, _fontSizeList[_fontSizeIndex], _tabletOrientation, _fontTypeAssetList[_fontTypeIndex]);
                    }
                }

                // Flips tablet orientation.
                _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.height, _rectTransform.rect.width);
            }
        }

        // Once we get here, Unity should be unfrozen. All code executed.
    }

    private void PopulateBlock(string[] blockPathList, string textContent, int fontSizeIndex, int fontTypeIndex, int textPathIndex, int fontSize, string tabletOrientation, TMP_FontAsset fontTypeAsset)
    {
        if ((_isFixedFontSizeOnly && fontSizeIndex != _fixedFontSize) ||
            (_isFixedFontTypeOnly && fontTypeIndex != _fixedFontType)) return;

        // Debug.Log($"blockPathList: {blockPathList} | textContent: {textContent} | fontSizeIndex: {fontSizeIndex} | fontTypeIndex: {fontTypeIndex} | textPathIndex: {textPathIndex} | fontSize: {fontSize} | tabletOrientation: {tabletOrientation} | fontTypeAsset: {fontTypeAsset}");

        // Define variables.
        StreamWriter _streamWriter = new StreamWriter(blockPathList[textPathIndex], true);
        bool _isRichText = false;
        int _pageCount = 1;
        int _richTextCount = 0;
        int _richLastCharacterIndex = 0;

        // Populate the text.
        _mainTMP.text = textContent;

        // Process the text.
        DecompressRichText();
        ReplaceRichText(fontSizeIndex);
        RemoveImageRichText();  // Sets all <image="test.jpg"> in CANDLE_TMP to "".

        // Set font type and font size.
        _mainTMP.font = fontTypeAsset;
        _mainTMP.text = _mainTMP.text.Insert(0, $"<size={fontSize}>");
        _mainTMP.ForceMeshUpdate();

        // Get information of vector for character.  Experimental Code for images.
        /*
        RemoveAllRichText();
        Debug.Log($"Index Of '##': {DEBUG_TMP.text.IndexOf("##")}");
        Debug.Log($"Index Of '\n​​' (2 hairlines): {DEBUG_TMP.text.IndexOf("\n​​") + 1}");
        charTransformIndex = DEBUG_TMP.text.IndexOf("\n​​") + 1;
        DebugCubeCharTransform();
        CANDLE_TMP.ForceMeshUpdate();
        */

        // Add identifier for the block text.
        _streamWriter.WriteLine($"TO:{tabletOrientation},FS:{fontSizeIndex},FT:{fontTypeIndex},PC:{_mainTMP.textInfo.pageCount}");
        _streamWriter.WriteLine("0");

        // Loop to populate text block indexes. 
        foreach (var _page in _mainTMP.textInfo.pageInfo)
        {
            if (_page.lastCharacterIndex != 0 && _pageCount != _mainTMP.textInfo.pageCount)
            {
                for (int _index = _richLastCharacterIndex; _index < _page.lastCharacterIndex + _richTextCount + 1 || _isRichText; _index++)
                {
                    // Debug.Log($"Last Char Index: {page.lastCharacterIndex} | End-Loop: {page.lastCharacterIndex + _richTextCount + 1 } | CharIndex: {i} | Char: {textContent[i]}");

                    // Locates small text errors in code.
                    if (_isVerySlowDebug)
                    {
                        try
                        {
                            if (textContent[_index] == 0) { };
                        }
                        catch
                        {
                            Debug.Log($"Index Out Of Range | PageCount: {_pageCount} | Int i: {_index} | Page: {_page}");
                        }
                    }

                    // Count rich texts.
                    // If you see an error here, that means a '<' or '>' symbol was not matched in the text.
                    // Double check the text to ensure that this symbol only appears for rich-text commands.
                    // Double check the text to ensure that this symobol isn't accidently missing for one of the rich-text commands.
                    if (textContent[_index] == '<' || textContent[_index] == '>')
                    {
                        _isRichText = !_isRichText;
                        _richTextCount++;
                    }
                    else if (_isRichText)
                    {
                        _richTextCount++;
                    }
                }

                // Add text block index.
                _richLastCharacterIndex = _page.lastCharacterIndex == 0 ? 0 : _page.lastCharacterIndex + _richTextCount; ;
                _streamWriter.WriteLine(_richLastCharacterIndex);
                _richLastCharacterIndex++;
                _pageCount++;
            }
            else
            {
                // Exit text block loop.
                _streamWriter.WriteLine(textContent.Length - 1);
                _streamWriter.WriteLine("0");
                break;
            }
        }
        _streamWriter.Close();
    }

    private void RemoveAllRichText()  // Parallel Code.
    {
        // Experimental imaging script. Ignore this if the book does not have any images.
        _wickTMP.text = _mainTMP.text;
        _wickTMP.ForceMeshUpdate();
        for (int _index = 0; _index < 100000; _index++)
        {
            int _start = _wickTMP.text.IndexOf("<");
            if (_start == -1)
            {
                // Debug.Log($"LOOP TERMINATED. REMOVED ({_index}) RICH-TEXTS.");
                break;
            }
            int _end = _wickTMP.text.IndexOf(">", _start) + 1;
            _wickTMP.text = _wickTMP.text.Remove(_start, _end - _start);

            // Debug.Log($"REMOVED({_start}, {_end - _start}) | START: {_start} | END: {_end} | SUCCESSFULLY REMOVED");
        }
    }

    public void RemoveImageRichText()  // Parallel Code.
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

    private void ReplaceRichText(int fontSizeIndex)  // Parallel Code.
    {
        // Replaces rich-text with custom variable settings.
        int _fontSize = _fontSizeList[fontSizeIndex];
        _mainTMP.text = _mainTMP.text.Replace("##", $"<align=\"center\"><size=125><sprite name=\"Image_4\"></size><align=\"justified\">\n​​");  // There are 2 hairlines at the end of \n.
        _mainTMP.text = _mainTMP.text.Replace("~", "");
        _mainTMP.text = _mainTMP.text.Replace("<size=h1>", $"<size={(_fontSize * _header1FontSizePercentList[fontSizeIndex]) / 100f}>");
        _mainTMP.text = _mainTMP.text.Replace("<size=h2>", $"<size={(_fontSize * _header2FontSizePercentList[fontSizeIndex]) / 100f}>");
        _mainTMP.text = _mainTMP.text.Replace("<size=h3>", $"<size={(_fontSize * _header3FontSizePercentList[fontSizeIndex]) / 100f}>");
        _mainTMP.text = _mainTMP.text.Replace("</size>", $"<size={_fontSize}>");
    }

    private void DecompressRichText()  // Parallel code.
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

        // Ignore as overflows are handled automatically.
        // char _firstCharacterThisPage = _mainTMP.text[0];
        // char _firstCharacterNextPage = _mainText.text[_lastCharSliceList[_mainPageIndex + 1]];
        // char _lastCharacterLastPage = _mainText.text[_lastCharSliceList[_mainPageIndex]];

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

        // Ignore as overflows are handled automatically.
        /*
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
        */

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
}