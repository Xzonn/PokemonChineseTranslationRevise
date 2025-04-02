---
last_modified_at: 2025-04-02 15:09
title: 《宝可梦》第四世代汉化修正
---
<div class="alert alert-info text-center" role="alert" markdown="1" style="font-size: 2rem;">
**[太长不看，跳转到链接](#补丁发布)**
</div>

## 说明
2016年对于《宝可梦》系列的中文玩家来说是非常重要的一年，在这一年的2月26日，官方首次公布了《宝可梦》系列对应简体中文和繁体中文的消息。随着首个官方中文游戏《精灵宝可梦 太阳／月亮》的发布，“口袋妖怪”世代成为了过去，<strong lang="ja">ポケットモンスター</strong>的正式名称被确定为“**精灵宝可梦**”。此后“精灵宝可梦”又被缩略为“宝可梦”。

由于官方中文化确定了宝可梦、招式、道具、特性等许多名词的译名，因此，为使官方译名更加深入人心，同时为中文玩家在游玩“口袋妖怪”时代的爱好者汉化作品时能够方便地查找资料，本人对<span lang="ja">『ポケットモンスター プラチナ』</span>（参考译名：《宝可梦 白金》）原汉化版的译名进行了修正，并发布了出来。

《白金》的汉化修正版发布后得到了一些反馈，有朋友希望能够统一第四世代的码表。因此我重写了使用工具的代码，并将代码和编译后的程序发布出来，供有兴趣的朋友研究。

汉化补丁1.2.0版本之后的标题图标由 **[大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作并授权本人使用，原始发布地址：<https://tieba.baidu.com/p/4518179164?see_lz=1>。

《钻石／珍珠》的补丁已随1.3.0版本发布，于2.0.0版本及以后的版本同步更新。

《心金／魂银》1.5.0版本补丁整合了宝可计步器汉化文本及汉化补丁，其中文本由 **OS** 翻译，补丁由 **圈叉汉化组** 制作并允许本人整合加入，原始发布地址：<https://bbs.oldmanemu.net/thread-18167.htm>。

2.1.0版本补丁整合了第三、四世代汉化版中文字符转换程序，由 **圈叉汉化组** 制作并允许本人整合加入，原始发布地址：<https://bbs.oldmantvg.net/thread-44009.htm>；同时整合了Wi-Fi Connection设置界面的汉化程序，由 **天涯**、**F君** 汉化制作并允许本人整合加入，原始发布地址：<https://github.com/R-YaTian/DS-Internet-CHS>。

## 工具发布
[GitHub](https://github.com/Xzonn/PCTRTools/releases/tag/v4.0.0)·[百度网盘](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ?pwd=pkmn)

工具由C#编写，包含文本处理功能、字库处理功能、narc文件替换功能，为命令行程序，无图形化界面，命令行参数可通过`--help`指令查看。

详细说明请见：**[汉化工具说明]({{ "/Tools.html" | relative_url }})**。

## 补丁发布

<div class="alert alert-info text-center" role="alert" markdown="1" style="font-size: 2rem;">
**[GitHub](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/latest)·[百度网盘](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ?pwd=pkmn)**
</div>

{% include video.html bvid="BV1oH1xYXEdb" %}

修正补丁使用方式已在压缩包中，也可参照上方的视频。各版本详细说明：

<table class="figure-table"><tbody><tr>
<td>{% include figure.html link="/DP.html" src="9938f0f795848274294631bb0d8fa323.jpg" alt="《宝可梦 钻石／珍珠》" width="588" height="331" %}</td>
<td>{% include figure.html link="/Pt.html" src="efdd474ffac175997868fa704bdc063e.jpg" alt="《宝可梦 白金》" width="588" height="331" %}</td>
</tr><tr>
<td>{% include figure.html link="/HGSS.html" src="3e6f40d11d826cc1f4babd1c5b2147b0.jpg" alt="《宝可梦 心金／魂银》" width="588" height="331" %}</td>
<td>{% include figure.html link="/PKHeX.html" src="a7b4b821e754b775055372bb0380bc0d.png" alt="可用于《宝可梦》第四世代汉化修正版的PKHeX版本" width="588" height="331" %}</td>
</tr></tbody></table>

2.0.0版本及以后版本的补丁仅适用于日文版ROM，其中《钻石／珍珠》需要Rev 6版本；1.5.0版本及以前版本的修正补丁仅适用于YYJoy汉化组／ACG汉化组发布的官方汉化版本，其余修改版本均不适用。

日文版ROM的MD5校验码：

- [《钻石》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=6641)：`c1d66b8d4fbdbfa57ff4868970fe19d2`
- [《珍珠》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4929)：`751d4a0524e4ef38b153ccfb5d21271f`
- [《白金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=2641)：`8271f64f5c7fb299adf937a8be6d8c88`
- [《心金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4168)：`e3f7933aee8cc2694629293f16c1c0a8`
- [《魂银》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4169)：`a1211b2d44e551197712177e3d50b491`

对于新版本汉化修正补丁读取旧版本存档时可能存在的乱码问题，参见 **[各汉化版本的码表兼容性]({{ "/CharTable.html" | relative_url }}#各汉化版本的码表兼容性)**。

## 预览
<table class="table">
<thead>
<tr><th colspan="2">标题界面</th></tr>
</thead>
<tbody>
<tr><td>{% include figure.html src="040c99ebeb494d5b5bfe10fc4bdb1d52.png" alt="《宝可梦 钻石》标题" %}</td><td>{% include figure.html src="7cf85ec45232b9930b9359268c6bd767.png" alt="《宝可梦 珍珠》标题" %}</td></tr>
<tr><td colspan="2">{% include figure.html src="7dc3f1452c6bd5c7779686929f96a6bc.png" alt="《宝可梦 白金》标题" %}</td></tr>
<tr><td>{% include figure.html src="ed366964bfaf73e862883782181a9ec1.png" alt="《宝可梦 心金》标题" %}</td><td>{% include figure.html src="4664ff3f027ff954c174ed5d1ea2fd4e.png" alt="《宝可梦 魂银》标题" %}</td></tr>
</tbody>
</table>

## 常见问题

1. **提示“原始ROM的MD5校验失败”是怎么回事？**
  - 原始ROM为日文版ROM，其中《钻石／珍珠》需要Rev 6版本；也可使用已经打好2.0.0版本及以后版本的补丁的ROM。
2. **如何获取ROM？**
  - 这里不提供ROM下载链接。如果你有正版卡带，也可以参照[这篇文章](https://haroohie.club/zh-hans/chokuretsu/guide/dumping-the-rom)提取ROM。
3. **正版卡带能否使用补丁？**
  - 目前还没有能够给正版卡带打补丁的方式。你仍然需要提取ROM后对ROM打补丁，然后使用烧录卡或[TWiLight Menu++](https://github.com/DS-Homebrew/TWiLightMenu)运行。采用TWiLight Menu++时，通过特定操作可以读写正版卡带的存档，请参照[这篇文章](https://bbs.oldmantvg.net/thread-22894.htm)。
