---
last_modified_at: 2023-10-14 11:37
title: 汉化工具说明
---
## 发布链接
- GitHub：[4.0.0](https://github.com/Xzonn/PCTRTools/releases/tag/v4.0.0)。

## 更新日志
- 1.0.1（2020-02-22）：
  - 最初发布版本。
- 1.1.0（2020-02-28）：
  - 将标点符号所用字体改为 MS Gothic。
  - 将中文字体与英文字体和数字下基线基本对齐。
  - 根据《心金／魂银》的字体进行修正。
  - 其他零碎的修正。
- 1.2.0（2020-03-05）：
  - 修正了无法读取《钻石／珍珠》字库的问题。
  - 修改了控制符的处理方式，兼容前一版本已导出的文本。
- 2.0.0（2020-03-27）：由于本次更新后的工具可能存在一些问题，请详细阅读使用说明。
  - 增加了对第五世代《黑／白》的支持（文本、字库）。
- 3.0.0（2021-06-22）：
  - 修改了第四世代《钻石／珍珠／白金／心金／魂银》的控制符。详请见下。
- 3.1.0（2021-06-27）：
  - 增加了自制的汉化补丁应用工具。详请见下。
- 3.1.1（2021-06-28）：
  - 修正补丁包文件占用问题。
- 3.1.2（2021-06-29）：
  - 修改`ndstool.exe`。
- 3.2.0（2023-02-11）：
  - 部分细节修正。
- 3.2.1（2023-09-11）：
  - 部分细节修正。
- 4.0.0（2023-10-08）：
  - 将全角空格、引号（`　“”‘’`）均视为标点。
  - 将中文字宽修改为 12。
  - 将两个工具合并为同一个。
  - 删除了补丁应用工具（`PokemonCTRPatch`），被 [NitroPatcher](https://github.com/Xzonn/NitroPatcher) 代替。
  - 增加了修改 NARC 文件包含的文件的功能。

## 总体说明
本人所使用的工具由C#编写，包含文本处理功能、字库处理功能、narc文件替换功能，为命令行程序，无图形化界面，命令行参数可通过`--help`指令查看。

文本处理功能具体包括：从narc文件导出txt文本、将txt文本导入并创建新的narc文件。

字库处理功能具体包括：从narc文件根据码表创建字体并创建新的narc文件。

narc文件替换功能具体包括：将narc文件中包含的文件替换为其他文件并创建新的narc文件。

1.2.0版本之前仅支持第四世代的文件，2.0.0版本加入对第五世代文件的初步支持。

在使用之前，请先从ROM中提取包含了文本和字库的narc文件，以及一份码表。详情见以下部分说明。本人不以任何形式提供完整ROM文件。

本工具为开源软件，源代码发布于[GitHub](https://github.com/Xzonn/PCTRTools/)，按照GPL-3.0协议授权。

## 码表
参见：[码表及控制符说明]({{ "/CharTable.html" | relative_url }})。

## narc文件
本工具直接读取和输出的文件即为narc文件。该格式实际是多个文件打包的格式。

narc解包代码参考了“narctool 0.1-p”，由 **@natrium42** 制作，**[@Pipian](https://github.com/pipian)** 修改，原始发布地址已不可考，但我从[“delguoqing/LMDumper”](https://github.com/delguoqing/LMDumper/tree/master/tools/narctool-0.1-p)项目的源代码中发现了该程序的源代码。原始程序为C++，我编写了C#下对打包和解包的实现，未编写压缩相关内容（与本项目无关）。

narc文件需要对原始ROM进行解包，我使用的是[“Tinke 0.9.2”](https://github.com/pleonex/tinke)，由 **[@pleoNeX](https://github.com/pleonex)** 制作并发布于GitHub，开源。该软件为图形化界面，方便操作。

## 文本
### 工具说明
在第四、第五世代中文本均以narc格式保存，路径有所不同：

- 《钻石／珍珠》：`/msgdata/msg.narc`
- 《白金》：`/msgdata/pl_msg.narc`
- 《心金／魂银》：`/a/0/2/7`
- 《黑／白／黑２／白２》：`a/0/0/2`、`a/0/0/3`

文本处理工具为`PokemonCTRText.exe`。第四世代代码参考了[“DS Text Editor”](https://github.com/JackHack96/DS-Text-Editor)，由 **[@JackHack96](https://github.com/JackHack96)** 制作并发布于GitHub，开源，授权协议为GPL-3.0。原始程序为Java，我编写了C#下的实现。第五世代代码参考了[“pptxt”](https://projectpokemon.org/home/forums/topic/10583-pptxt-text-editing-tool/)，由 **[@SCV](https://projectpokemon.org/home/profile/2-scv/)** 制作并发布于Project Pokemon Forums，开源。原始程序为C++，我编写了C#下的实现。

对于第四世代的文本，1.x - 2.x版本中均把控制符`0x25BC`转义为`\r`，`0x25BD`转义为`\f`，但这种转义方法与第五世代有冲突，因此自3.0.0版本开始调换了两个控制符的转义方式。目前，`\r`表示对话移动到下一行，`\f`表示对话翻页。为了兼容之前的版本，使用新版本导出的文本第一行会加入`#3`，表明为新版本导出的文本。

对于第五世代的文本，由于缺乏研究，在某些情况下导入文本可能会出错，原因不明。为了稳定起见，本工具会以原文本占用的最大长度 - 1作为导入文本的长度，长度不足者以`0xFFFF`补齐，长度超过者直接截断并在控制台输出信息。需要注意的是原文本占用的最大长度并非文本长度，而是文本长度 × 2，例如“フシギダネ”占用的长度为10而非5，同时考虑到末尾必须有`0xFFFF`作为控制符，因此允许导入的最大长度为9。后续版本中可能会根据对文件结构的分析解除限制。

### 使用方法
#### 从narc文件导出txt文本
示例：

``` bash
PCTRTools.exe text-export -c CharTable/Original_DP.txt -i DP/Original/msgdata/msg.narc -o DP/Original/Messages.txt
```

此操作以`CharTable/Original_DP.txt`为码表，`DP/Original/msgdata/msg.narc`为文本所在的narc文件，`DP/Original/Messages.txt`为导出后保存为的txt文件。

#### 将txt文本导入并创建新的narc文件
示例：

``` bash
PCTRTools.exe text-import -c CharTable/Expanded.txt -i DP/Original/msgdata/msg.narc -t DP/Revised/Messages.txt -o DP/Revised/msgdata/msg.narc
```

此操作以`CharTable/Expanded.txt`为码表，`DP/Original/msgdata/msg.narc`为文本所在的narc文件，`DP/Revised/Messages.txt`为需要导入的txt文件，`DP/Revised/msgdata/msg.narc`为导入文本后保存为的narc文件。

如果文本中存在码表未收录的字符，则在导入时程序会有输出的提示。

## 字库
### 工具说明
在第四、第五世代中字库均以narc格式保存，路径有所不同：

- 《钻石／珍珠》：`/graphic/font.narc`
- 《白金》：`/graphic/pl_font.narc`
- 《心金／魂银》：`/a/0/1/6`
- 《黑／白／黑２／白２》：`a/0/2/3`

字库处理工具为`PokemonCTRFont.exe`。代码参考了[“PokeFontDS”](https://github.com/TheFearsomeDzeraora/PokeFontDS)，由 **TheFearsomeDzeraora** 制作并发布于 GitHub，开源；以及“NARCFileReadingDLL”，由 **@evco1** 制作，原始发布地址已不可考，但我从“PokeFontDS”项目的源代码中发现了该程序的反编译源代码。

由于可选的中文点阵字体较少，在比较了多种字体后我选择了Windows自带的中易宋体作为字体。如需更换为其他字体，请自行修改源代码并编译。

使用前请确保已安装以下字体：

- 中易宋体（Simsun）
- MS Gothic
- Zfull-GB（仅第五世代使用）

本人不以任何方式提供上述字体的可安装版本。

本工具先读取日文字库，然后对字库包含的字符数量扩容到所需的大小，最后将中文字符用上述字体重新生成。字号大小为12px（正常大小，中易宋体或MS Gothic）或9px（小号字体，Zfull-GB，仅第五世代使用）。在上述字体中均包含了对应字号下的点阵字库，因此无需抗锯齿即可取得较好的显示效果。

由于非汉化版游戏并没有对中文字库进行额外处理，因此汉化版均对arm9.bin进行了修改，以适配大字库。修改方式可参考[本文](https://xzonn.top/posts/Pokemon-DP-Chinese-Localization-Based-on-Pret-Project.html)。

### 使用方法
#### 从narc文件根据码表创建新的narc文件
示例：

``` bash
PCTRTools.exe font -c CharTable/Expanded.txt -i DP/Original/graphic/font.narc -o DP/Revised/graphic/font.narc
```

此操作以`CharTable/Expanded.txt`为码表，`DP/Original/graphic/font.narc`为字库所在的narc文件，`DP/Revised/graphic/font.narc`为新字库保存为的narc文件。

## narc文件替换
主要用于替换图片形式的文本，无需手动打包，方便自动化操作。

### 使用方法
#### 将narc文件中包含的文件替换为其他文件并创建新的narc文件
示例：

``` bash
PCTRTools.exe replace-narc -i files/DP/data/ -n textures/DP/ -o out/D/data/
```

此操作将遍历`files/DP/data/`目录下的所有文件，将其中的narc文件中包含的文件替换为`textures/DP/`目录下的同名文件，并将替换后的narc文件保存到`out/D/data/`目录下。

## 汉化补丁应用工具
{% include figure.html src="0d73b0d02719ca21fb9bbac2c8362a1b.png" width="469" height="324" alt="汉化补丁应用工具" %}

为了解决之前很多人提到的“打补丁打不上”“校验值不通过”的问题，我自行编写了汉化补丁应用工具，可以搭配[自动构建项目](https://github.com/Xzonn/PokemonChineseTranslationRevise)一起使用。

目前该工具已重写，改名为[“NitroPatcher”](https://github.com/Xzonn/NitroPatcher)。由于采用的是新的打包方式，因此不再需要使用`ndstool`，但与旧版本工具生成的ROM校验码可能不一致。对于1.5.0版本及以前版本的补丁，请使用旧版本工具。

本工具为图形化界面，如有使用上的问题请及时反馈。