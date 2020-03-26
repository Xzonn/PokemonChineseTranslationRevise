---
date: 2020-02-22 23:12
info: 利用C#编写的命令行工具，可以对《精灵宝可梦 钻石／珍珠／白金／心金／魂银》的文本和字库进行修改。
last_modified_at: 2020-03-26 18:07
layout: default
title: 《精灵宝可梦》第四世代汉化修正
---
<div class="bilibiliBox" data-aid="96227144" data-cid="164268057" data-page="1"></div>

## 说明
2016年对于《精灵宝可梦》系列的中文玩家来说是非常重要的一年，在这一年的2月26日，官方首次公布了《精灵宝可梦》系列对应简体中文和繁体中文的消息。随着首个官方中文游戏《精灵宝可梦 太阳／月亮》的发布，“口袋妖怪”世代成为了过去，<strong lang="ja">ポケットモンスター</strong>的正式名称被确定为“**精灵宝可梦**”。

由于官方中文化确定了宝可梦、招式、道具、特性等许多名词的译名，因此，为使官方译名更加深入人心，同时为中文玩家在游玩“口袋妖怪”时代的爱好者汉化作品时能够方便地查找资料，本人对<span lang="ja">『ポケットモンスター プラチナ』</span>（参考译名：《精灵宝可梦 白金》）原汉化版的译名进行了修正，并发布了出来。

《白金》的汉化修正版发布后得到了一些反馈，有朋友希望能够统一第四世代的码表。因此我重写了使用工具的代码，并将代码和编译后的程序发布出来，供有兴趣的朋友研究。

汉化补丁1.2.0版本之后的标题图标由 **[@大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作并授权本人使用，原始发布地址：<https://tieba.baidu.com/p/4518179164?see_lz=1>。

## 工具发布
发布链接：[GitHub：1.2.0版本](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/1.2.0) &#124; 源代码：[GitHub](https://github.com/Xzonn/PokemonChineseTranslationRevise/)

本人所使用的工具由C#编写，按照功能区分为文本处理工具和字库处理工具，均为命令行程序，无图形化界面。命令行参数可通过`--help`指令查看。

文本处理工具为`PokemonCTRText.exe`，目前实现的功能包括：从narc文件导出txt文本、将txt文本导入并创建新的narc文件。

字库处理工具为`PokemonCTRFont.exe`，目前实现的功能仅有：从narc文件根据码表创建新的narc文件。

1.2.0版本之前仅支持第四世代的文件，计划在2.0.0版本加入对第五世代文件的支持。

详细说明请见：**[汉化工具说明](./Tools.html)**。

## 补丁发布
由于版权原因，本人仅发布修正补丁，使用方式已在压缩包中，可自行查看。发布链接：

<div class="row">
<div class="col-md-4 col-md-offset-1">
<a href="./Pt.html" style="display: block"><img src="https://file.moetu.org/images/2020/02/23/efdd474ffac175997868fa704bdc063e1f4ad7cdd56b9c40.jpg" alt="《精灵宝可梦 白金》" data-size="588" data-disp="block" /></a>
<p class="text-center">《精灵宝可梦 白金》</p>
</div>
<div class="col-md-4 col-md-offset-2">
<a href="./HGSS.html" style="display: block"><img src="https://file.moetu.org/images/2020/02/23/3e6f40d11d826cc1f4babd1c5b2147b08f8baaac761f7e65.jpg" alt="《精灵宝可梦 心金／魂银》" data-size="588" data-disp="block" /></a>

<p class="text-center">《精灵宝可梦 心金／魂银》</p>
</div>
</div>
<div class="row">
<div class="col-md-4 col-md-offset-4">
<a href="./PKHeX.html" style="display: block"><img src="https://file.moetu.org/images/2020/02/28/a7b4b821e754b775055372bb0380bc0d801bf278aa99f058.png" alt="可用于《精灵宝可梦》第四世代汉化修正版的PKHeX版本" data-size="588" data-disp="block" /></a>
<p class="text-center">PKHeX</p>
</div>
</div>

总链接：

- GitHub：[1.2.0](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/patches-1.2.0)。
- 百度网盘：[1.2.0](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ)（`pkmn`）。

本系列修正补丁仅适用于ACG汉化组发布的官方汉化版本，其余修改版本均不适用。并且本人无意制作基于其他版本游戏的补丁，如有需求请自行导入。

## 预览
<table class="table">
<thead>
<tr><th>原版</th><th>修正版</th></tr>
</thead>
<tbody>
<tr><td><img src="https://file.moetu.org/images/2020/02/20/e23ee94c4705bb8188cde6ee2ba684f2370e44e045b982e4.png" alt="《口袋妖怪 白金》标题" data-disp="auto" /></td><td><img src="https://file.moetu.org/images/2020/03/24/808b046468c20f4b60a7361413efb8a91fe2d519e6c39d9b.png" alt="《精灵宝可梦 白金》标题" data-disp="auto" /></td></tr>
<tr><td><img src="https://file.moetu.org/images/2020/02/28/a8c17f7f507f7119d1f7caa8ab6458f5e446bc7ca62d2346.png" alt="《口袋妖怪 心灵之金》标题" data-disp="auto" /></td><td><img src="https://file.moetu.org/images/2020/03/24/5545beab7e329331555cfbfe24255b8e95fb55402337176d.png" alt="《精灵宝可梦 心灵之金》标题" data-disp="auto" /></td></tr>
<tr><td><img src="https://file.moetu.org/images/2020/02/28/94baba0691c671fb07233b6d7c4051a3d593e577132de3fe.png" alt="《口袋妖怪 灵魂之银》标题" data-disp="auto" /></td><td><img src="https://file.moetu.org/images/2020/03/24/3f954e319208266b71d62f2ea3cffa45be65c61b3e64c94f.png" alt="《精灵宝可梦 灵魂之银》标题" data-disp="auto" /></td></tr>
</tbody>
</table>