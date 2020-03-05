---
date: 2020-02-22 23:54
last_modified_at: 2020-02-23 15:18
layout: default
title: 汉化工具说明
---
## 更新日志
- 1.0.1（2020/02/22）：
  - 最初发布版本。
- 1.1.0（2020/02/28）：
  - 将标点符号所用字体改为 MS Gothic。
  - 将中文字体与英文字体和数字下基线基本对齐。
  - 根据《心金／魂银》的字体进行修正。
  - 其他零碎的修正。
- 1.2.0（2020/03/05）：
  - 修正了无法读取《钻石／珍珠》字库的问题。
  - 修改了控制符的处理方式，兼容前一版本已导出的文本。

## 码表
码表文件实际为txt文本文件，每个字符为一行，每行以`\t`分隔符隔开，前为编码（无前缀16进制），后为字符。

本人提供了《[钻石／珍珠](https://github.com/Xzonn/PokemonChineseTranslationRevise/raw/master/Files/CharTable/Original_DP.txt)》《[白金](https://github.com/Xzonn/PokemonChineseTranslationRevise/raw/master/Files/CharTable/Original_Pt.txt)》《[心金／魂银](https://github.com/Xzonn/PokemonChineseTranslationRevise/raw/master/Files/CharTable/Original_HGSS.txt)》原始汉化版所用的码表，同时发售的双版本游戏使用了同样的码表和文本，因此合并发布，下同。

《珍珠／钻石》和《白金》的码表信息来自于“PokeSav字符转换器”（文件名：`WordChanger.exe`），由 **@Easy_World** 制作，原始发布地址已不可考。其中，该软件提供的《珍珠／钻石》码表最大编号为`0x1C70`（7280），但由于字库文件限制，实际最大编号为`0x119D`（4509），但这个大小还是比《白金》和《心金／魂银》的字库大出不少，常用字基本已经包括在内。《白金》码表最大编号为`0x0DD9`（3545）。

《心金／魂银》的码表信息为我个人校对而成，最大编号为`0x0E81`（3713）。

## narc文件
本工具直接读取和输出的文件即为narc文件。该格式实际是多个文件打包的格式。

narc解包代码参考了“narctool 0.1-p”，由 **@natrium42** 制作，**[@Pipian](https://github.com/pipian)** 修改，原始发布地址已不可考，但我从“[delguoqing/LMDumper](https://github.com/delguoqing/LMDumper/tree/master/tools/narctool-0.1-p)”项目的源代码中发现了该程序的源代码。原始程序为C++，我编写了C#下对打包和解包的实现，未编写压缩相关内容（与本项目无关）。

narc文件需要对原始ROM进行解包，我使用的是“[Tinke 0.9.2](https://github.com/pleonex/tinke)”，由 **[@pleoNeX](https://github.com/pleonex)** 制作并发布于GitHub，开源。该软件为图形化界面，方便操作。

## 文本
在《钻石／珍珠／白金／心金／魂银》中文本均以narc格式保存，路径有所不同：

- 《钻石／珍珠》：`/msgdata/msg.narc`
- 《白金》：`/msgdata/pl_msg.narc`
- 《心金／魂银》：`/a/0/2/7`

文本处理工具为`PokemonCTRText.exe`。代码参考了“[DS Text Editor](https://github.com/JackHack96/DS-Text-Editor)”，由 **[@JackHack96](https://github.com/JackHack96)** 制作并发布于GitHub，开源，授权协议为GPL-3.0。原始程序为Java，我编写了C#下的实现。

使用方法：

### 从narc文件导出txt文本
示例：

```
PokemonCTRText.exe -c CharTable/Original_DP.txt -m DP/Original/msgdata/msg.narc -e DP/Original/Messages.txt
```

此操作以`CharTable/Original_DP.txt`为码表，`DP/Original/msgdata/msg.narc`为文本所在的narc文件，`DP/Original/Messages.txt`为导出后保存为的txt文件。

### 将txt文本导入并创建新的narc文件
示例：

```
PokemonCTRText.exe -c CharTable/Expanded.txt -m DP/Original/msgdata/msg.narc -i DP/Revised/Messages.txt -o DP/Revised/msgdata/msg.narc
```

此操作以`CharTable/Expanded.txt`为码表，`DP/Original/msgdata/msg.narc`为文本所在的narc文件，`DP/Revised/Messages.txt`为需要导入的txt文件，`DP/Revised/msgdata/msg.narc`为导入文本后保存为的narc文件。

如果文本中存在码表未收录的字符，则在导入时程序会有输出的提示。

## 字库
在《钻石／珍珠／白金／心金／魂银》中字库均以narc格式保存，路径有所不同：

- 《钻石／珍珠》：`/graphic/font.narc`
- 《白金》：`/graphic/pl_font.narc`
- 《心金／魂银》：`/a/0/1/6`

文本处理工具为`PokemonCTRFont.exe`。代码参考了“NARCFileReadingDLL”，由 **@evco1** 制作，原始发布地址已不可考，但我从“[TheFearsomeDzeraora/PokeFontDS](https://github.com/TheFearsomeDzeraora/PokeFontDS)”项目的源代码中发现了该程序的反编译源代码。

由于可选的中文点阵字体较少，在比较了多种字体后我选择了Windows自带的中易宋体作为字体。如需更换为其他字体，请自行修改源代码并编译。

使用方法：

### 从narc文件根据码表创建新的narc文件
示例：

```
PokemonCTRFont.exe -c CharTable/Expanded.txt -f DP/Original/graphic/font.narc -o DP/Revised/graphic/font.narc
```

此操作以`CharTable/Expanded.txt`为码表，`DP/Original/graphic/font.narc`为字库所在的narc文件，`DP/Revised/graphic/font.narc`为新字库保存为的narc文件。

### 从narc文件导出字库为png文件（实验性）
本功能为实验性功能，为本人在调试过程中使用，故无法直接调用，如需调用请自行修改源代码并编译。