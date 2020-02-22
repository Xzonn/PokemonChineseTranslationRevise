---
date: 2020-02-22 23:54
layout: default
title: 汉化工具说明
---
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

## 字库
在《钻石／珍珠／白金／心金／魂银》中字库均以narc格式保存，路径有所不同：

- 《钻石／珍珠》：`/graphic/font.narc`
- 《白金》：`/graphic/pl_font.narc`
- 《心金／魂银》：`/a/0/1/6`