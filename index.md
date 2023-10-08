---
last_modified_at: 2023-10-08 20:01
title: 《宝可梦》第四世代汉化修正
---
{% include video.html aid="886985239" %}

## 说明
2016年对于《宝可梦》系列的中文玩家来说是非常重要的一年，在这一年的2月26日，官方首次公布了《宝可梦》系列对应简体中文和繁体中文的消息。随着首个官方中文游戏《精灵宝可梦 太阳／月亮》的发布，“口袋妖怪”世代成为了过去，<strong lang="ja">ポケットモンスター</strong>的正式名称被确定为“**精灵宝可梦**”。此后“精灵宝可梦”又被缩略为“宝可梦”。

由于官方中文化确定了宝可梦、招式、道具、特性等许多名词的译名，因此，为使官方译名更加深入人心，同时为中文玩家在游玩“口袋妖怪”时代的爱好者汉化作品时能够方便地查找资料，本人对<span lang="ja">『ポケットモンスター プラチナ』</span>（参考译名：《宝可梦 白金》）原汉化版的译名进行了修正，并发布了出来。

《白金》的汉化修正版发布后得到了一些反馈，有朋友希望能够统一第四世代的码表。因此我重写了使用工具的代码，并将代码和编译后的程序发布出来，供有兴趣的朋友研究。

汉化补丁1.2.0版本之后的标题图标由 **[@大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作并授权本人使用，原始发布地址：<https://tieba.baidu.com/p/4518179164?see_lz=1>。

《钻石／珍珠》的补丁已随1.3.0版本发布，预计于1.6.0及以后的版本同步更新。

《心金／魂银》1.5.0版本补丁整合了宝可计步器汉化文本及汉化补丁，其中文本由 **OS** 翻译，补丁由 **圈叉汉化组** 制作并允许本人整合加入，原始发布地址：<https://bbs.oldmanemu.net/thread-18167.htm>。

## 工具发布
发布链接：[GitHub：3.2.1版本](https://github.com/Xzonn/PCTRTools/releases/tag/3.2.1) &#124; 源代码：[GitHub](https://github.com/Xzonn/PokemonChineseTranslationRevise/)

本人所使用的工具由C#编写，按照功能区分为文本处理工具、字库处理工具，为命令行程序，无图形化界面，命令行参数可通过`--help`指令查看。

详细说明请见：**[汉化工具说明]({{ "/Tools.html" | relative_url }})**。

## 补丁发布
考虑到便利性，本人仅发布修正补丁，使用方式已在压缩包中，可自行查看。发布链接：

<table class="figure-table"><tbody><tr>
<td>{% include figure.html link="/DP.html" src="9938f0f795848274294631bb0d8fa323.jpg" alt="《宝可梦 钻石／珍珠》" width="588" height="331" %}</td>
<td>{% include figure.html link="/Pt.html" src="efdd474ffac175997868fa704bdc063e.jpg" alt="《宝可梦 白金》" width="588" height="331" %}</td>
</tr><tr>
<td>{% include figure.html link="/HGSS.html" src="3e6f40d11d826cc1f4babd1c5b2147b0.jpg" alt="《宝可梦 心金／魂银》" width="588" height="331" %}</td>
<td>{% include figure.html link="/PKHeX.html" src="a7b4b821e754b775055372bb0380bc0d.png" alt="可用于《宝可梦》第四世代汉化修正版的PKHeX版本" width="588" height="331" %}</td>
</tr></tbody></table>

总链接：

- GitHub：[1.5.0](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/patches-1.5.0)。
- 百度网盘：[1.5.0](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ)（`pkmn`）。

本系列修正补丁仅适用于YYJoy汉化组／ACG汉化组发布的官方汉化版本，其余修改版本均不适用。并且本人无意制作基于其他版本游戏的补丁，如有需求请自行导入。

## 预览
<table class="table">
<thead>
<tr><th>原版</th><th>修正版</th></tr>
</thead>
<tbody>
<tr><td>{% include figure.html src="e23ee94c4705bb8188cde6ee2ba684f2.png" alt="《口袋妖怪 白金》标题" width="256" height="384" %}</td><td>{% include figure.html src="808b046468c20f4b60a7361413efb8a9.png" alt="《宝可梦 白金》标题" width="256" height="384" %}</td></tr>
<tr><td>{% include figure.html src="a8c17f7f507f7119d1f7caa8ab6458f5.png" alt="《口袋妖怪 心灵之金》标题" width="256" height="384" %}</td><td>{% include figure.html src="5545beab7e329331555cfbfe24255b8e.png" alt="《宝可梦 心金》标题" width="256" height="384" %}</td></tr>
<tr><td>{% include figure.html src="94baba0691c671fb07233b6d7c4051a3.png" alt="《口袋妖怪 灵魂之银》标题" width="256" height="384" %}</td><td>{% include figure.html src="3f954e319208266b71d62f2ea3cffa45.png" alt="《宝可梦 魂银》标题" width="256" height="384" %}</td></tr>
</tbody>
</table>