---
last_modified_at: 2024-05-03 12:07
title: 码表及控制符说明
---
## 更新日志
- 2021-06-22：
  - 修改了第四世代中控制符`\r`和`\f`的表现方法，与第五世代保持一致。
- 2021-08-21：
  - 修改了`00F1`对应的字符，原本为全角连字符`－`（U+FF0D），修改为了日语长音符`ー`（U+30FC）。
  - 交换了`00F5`和`0DD9`对应的字符，原本为`00F5` = `〜`（U+301C），`0DD9` = `～`（U+FF5E）。
- 2023-02-11：
  - 添加了更多字符。
- 2023-09-12：
  - 添加了更多字符。

## 码表
码表文件实际为txt文本文件，每个字符为一行，每行以`\t`（U+0009）分隔符隔开，前为编码（无前缀16进制），后为字符。

### 第四世代
在第四世代中，游戏存储字符的编码与Unicode编码并非一一对应，因此需要使用码表标识字符。对于使用不同码表的游戏，同一字符对应的编码并不相同，同一编码对应的字符也不相同，因此两者间连接交换会出现字符不匹配，即“乱码”。

本人提供了《[钻石／珍珠](https://github.com/Xzonn/PokemonChineseTranslationRevise/raw/master/files/CharTable_DP_YYJoy.txt)》《[白金](https://github.com/Xzonn/PokemonChineseTranslationRevise/raw/master/files/CharTable_Pt_ACG.txt)》《[心金／魂银](https://github.com/Xzonn/PokemonChineseTranslationRevise/raw/master/files/CharTable_Pt_HGSS.txt)》原始汉化版所用的码表，同时发售的双版本游戏使用了同样的码表和文本，因此合并发布，下同。

《珍珠／钻石》和《白金》的码表信息来自于“PokeSav字符转换器”（文件名：`WordChanger.exe`），由 **Easy_World** 制作，原始发布地址已不可考。其中，该软件提供的《珍珠／钻石》码表最大编号为`0x1C70`（7280），但由于字库文件限制，实际最大编号为`0x119D`（4509），但这个大小还是比《白金》和《心金／魂银》的字库大出不少，常用字基本已经包括在内。《白金》码表最大编号为`0x0DD9`（3545）。

《心金／魂银》的码表信息为我个人校对而成，最大编号为`0x0E81`（3713）。

由于本人发布的修正补丁使用了同一码表，因此可以互相连接交换。同时本人建议其他使用本工具者也使用本人的码表，以方便连接交换，缺失的字符可以在最后追加并提交Pull Request。

**[> 修正补丁码表链接 <](https://github.com/Xzonn/PokemonChineseTranslationRevise/raw/master/files/CharTable.txt)**

### 第五世代
在第五世代中，游戏存储字符的编码为Unicode，因此在任意版本的游戏中字符与编码都是一一对应的，连接交换不会出现问题。因此，对第五世代来说“码表”仅用于标识字库中需要包含哪些字符。由于一些原因，本工具仅会读取Unicode编码为`0x4E00` ~ `0xE000`之间的字符，其他字符会被直接忽略。

由于不明原因，本人建议第五世代的码表按照Unicode顺序排序，否则可能会出现字符变为“？”的情况。

### PKHeX
[PKHeX]({{ "/PKHeX.html" | relative_url }})使用的码表与上述码表不完全一致，主要体现在非中文文本部分，对使用无较大影响。

## 控制符
对于第四世代的文本，1.x - 2.x版本的[汉化工具]({{ "/Tools.html" | relative_url }})中均把控制符`0x25BC`转义为`\r`，`0x25BD`转义为`\f`，但这种转义方法与第五世代有冲突，因此自3.0.0版本开始调换了两个控制符的转义方式。

目前，`\r`表示对话移动到下一行，`\f`表示对话翻页。为了兼容之前的版本，使用新版本导出的文本第一行会加入`#3`，表明为新版本导出的文本。

## 各汉化版本的码表兼容性
<table class="table table-bordered">
<thead>
<tr><th rowspan="2">语言</th><th rowspan="2">游戏</th><th rowspan="2">来源</th><th colspan="2">钻石／珍珠</th><th>白金／心金／魂银</th></tr>
<tr><th>2.0.0 版</th><th>0.9.0 版</th><th>所有版本</th></tr>
</thead>
<tbody>
<tr><td>日文</td><td colspan="2">任意游戏</td><td class="success">正常</td><td class="success">正常</td><td class="success">正常</td></tr>
<tr><td>英文</td><td colspan="2">任意游戏</td><td class="success">正常</td><td class="success">正常</td><td class="success">正常</td></tr>
<tr><td rowspan="6">中文</td><td rowspan="2">钻石／珍珠</td><td>YYJoy版</td><td class="danger">不正常，显示为乱码</td><td class="success">正常</td><td class="danger">不正常，显示为乱码</td></tr>
<tr><td>Xzonn 修正版，0.9.0 版</td><td class="danger">不正常，显示为乱码</td><td class="success">正常</td><td class="danger">不正常，显示为乱码</td></tr>
<tr><td rowspan="2">白金</td><td>ACG 版</td><td class="success">正常</td><td class="danger">不正常，显示为乱码</td><td class="success">正常</td></tr>
<tr><td>Xzonn 修正版，1.5.0 版</td><td class="success">正常</td><td class="danger">不正常，显示为乱码</td><td class="success">正常</td></tr>
<tr><td rowspan="2">心金／魂银</td><td>ACG 版</td><td class="danger">不正常，显示为乱码</td><td class="danger">不正常，显示为乱码</td><td class="danger">不正常，显示为乱码</td></tr>
<tr><td>Xzonn 修正版，1.5.0 版</td><td class="success">正常</td><td class="danger">不正常，显示为乱码</td><td class="success">正常</td></tr>
<tr><td>韩文</td><td colspan="2">非汉化版游戏</td><td class="danger">不正常，显示为中文</td><td class="danger">不正常，显示为中文</td><td class="danger">不正常，显示为中文</td></tr>
</tbody>
</table>
