# Candle

Author: Ryan Kruse

VRChat: Clearly

Discord: Clearly#3238

GitHub: https://github.com/RyanKruse/Candle

Prefab: https://clearly.booth.pm/items/3258223



## Introduction

VRChat is a virtual reality social platform that has introduced multiple media channels for communicating information to its users. One channel of communication missing in virtual reality is the channel of reading books. Textual mediums of communication have existed for thousands of years. They have evolved from wooden sticks, to parchment paper, to printed paper, to personal computers, to mobile devices, and now to virtual reality. The goal of Candle is to help bridge the gap between the user and the interface in reading books within a virtual reality platform.

This document outlines the steps to publish a book onto Candle. There is no coding involved to add a book. Candle works in the following execution order:

1) Populate text files.
2) Format text files.
3) Process text files.
4) Compile text files.
5) Read text files.

First time adding a book to Candle? Let's walk through each step:

YouTube Tutorial: https://www.youtube.com/watch?v=bPbtYkrA3-M

Discord Assistance: Clearly#3238



## Populate Text Files

1) Open this folder: Candle / Assets / Books / 02 - Blank Book Template / Texts
2) Populate the files with the book contents.
3) Add, remove, or rename any files.
4) Save and close.

Hint: The first two digits of the file name contain the order Candle will read the files.

Hint: The file names appear on the lower-left hand corner of the Candle display when reading.



## Format Text Files

1) Open this folder: Candle / Assets / Books / 03 - Rich Text Commands / Texts
2) Format your book text with the rich-text you'd like to add.
3) Open "06 - Acknowledgements.txt" to see an example of proper formatting.
4) Save and close.



## Process Text Files

1) Click on "Wax" in Unity's Hierarchy Window.
2) Open this folder: Candle / Assets / Books / 02 - Blank Book Template / Texts
3) Populate "02_Text" with the text files.
4) Save the scene.



## Compile Text Files

1) Click on "Wick" in Unity's Hierarchy Window.
2) Change the variable "Default Book Index" to 2.
3) Check the variable "Is Execute".
4) Click on the Play Button and let compiler execute.
5) Once complete, click on the Exit Button.
6) Uncheck the variable "Is Execute".
7) Click on "Wax" in Unity's Hierarchy Window.
8) Open this folder: Candle / Assets / Books / 02 - Blank Book Template / Blocks
9) Populate "02_Block" with the block files.
10) Save the scene.



## Read Text Files

1) Click on the Play Button.
2) Click on the book cover "Your Book Here".
3) Read the book.
4) Validating no errors, exit and build the scene for VRChat.
5) Read the book.

Hint: Once everything works, feel free to change the book cover and add multiple books.



## Conclusion

After finishing the above execution orders, the book that you have added into the text files should now be readable in VRChat.



## Numpad Commands

[1] - Increment Page Down.

[3] - Increment Page Up.

[4] - Change Font Size.

[5] - Toggle Mouse Scrolling.

[6] - Change Font Type.

[7] - Decrease Tablet Size.

[9] - Increase Tablet Size.

[0] - Rotate Tablet Display.

[/] - Fast Increment Page Down.

[*] - Fast Increment Page Up.

[-] - Bump Tablet Position Up.

[+] - Bump Tablet Position Down.

