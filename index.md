---
last_modified_at: 2025-08-24 13:28
title: 《宝可梦》第四世代汉化修正
---
<div class="alert alert-info text-center" role="alert" markdown="1" style="font-size: 2rem;">
**[太长不看，跳转到链接](#补丁发布)**
</div>

## 说明

近年来，官方中文版的《宝可梦》系列游戏所用的译名已经深入人心。对于旧世代的《宝可梦》游戏而言，使用官方译名无疑将更有利于玩家间的交流。

本项目是对Nintendo DS平台的第四世代《宝可梦》游戏（《钻石／珍珠／白金／心金／魂银》）的简体中文汉化，不仅采用了《宝可梦》系列最新官方译名，还对原有的汉化文本进行了完全修订，提升了翻译质量，此外还添加了一些额外功能。

本汉化以补丁形式提供。<strong style="color: var(--color-warm);">如需转载，请保留压缩包中的说明文件以方便获取进一步更新。</strong>

{% include video.html bvid="BV135e7ziE4y" %}

## 汉化名单

- **初始版本汉化**：YYJoy汉化组（《[钻石／珍珠](https://xzonn.top/PokemonChineseTranslationRevise/DP.html)》）、ACG汉化组（《[白金](https://xzonn.top/PokemonChineseTranslationRevise/Pt.html)／[心金／魂银](https://xzonn.top/PokemonChineseTranslationRevise/HGSS.html)》）
- **文本修正、本地化改动**：Xzonn
- **二进制代码构建**：Xzonn（基于 [devkitPro](https://devkitpro.org/wiki/Getting_Started) 编译，参考了Jonko编写的 [NitroPacker](https://github.com/haroohie-club/NitroPacker)）
- **[标题图标制作](https://tieba.baidu.com/p/4518179164?see_lz=1)**：大神丶橡皮
- **[宝可计步器汉化](https://bbs.oldmantvg.net/thread-18167.htm)**：OS（翻译）、圈叉汉化组（制作）
- **[第三、四世代汉化版中文字符转换程序](https://bbs.oldmantvg.net/thread-44009.htm)**：圈叉汉化组
- **[Wi-Fi Connection设置界面汉化](https://github.com/R-YaTian/DS-Internet-CHS)**：天涯、F君

## 使用方式

<div class="alert alert-info text-center" role="alert" markdown="1" style="font-size: 2rem;">
**[GitHub](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/latest/)·[百度网盘](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ?pwd=pkmn)**
</div>

请下载压缩包并解压，按照补丁应用工具“NitroPatcher”的说明进行操作。补丁压缩包中包含了Microsoft Windows平台的补丁应用工具，其他平台的工具可通过下方下载地址获取。

软件使用视频教程：<https://www.bilibili.com/video/BV1oH1xYXEdb/?t=69>

原始ROM可以为日本版或汉化修正版2.0.0及以后的版本。其中日本版的MD5校验码：

- [《钻石》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=6641)：`c1d66b8d4fbdbfa57ff4868970fe19d2`
- [《珍珠》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4929)：`751d4a0524e4ef38b153ccfb5d21271f`
- [《白金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=2641)：`8271f64f5c7fb299adf937a8be6d8c88`
- [《心金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4168)：`e3f7933aee8cc2694629293f16c1c0a8`
- [《魂银》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4169)：`a1211b2d44e551197712177e3d50b491`

补丁压缩包下载地址：

- GitHub：<https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/latest/>
- 百度网盘：<https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ?pwd=pkmn>

补丁应用工具下载地址（Windows/Linux/macOS/Android）：

- GitHub：<https://github.com/Xzonn/NitroPatcher/releases/latest/>
- 百度网盘：<https://pan.baidu.com/s/1vXynSX1WauU3FeGHDnrDfg?pwd=ntro>

各版本详细说明：

<table class="figure-table"><tbody><tr>
<td>{% include figure.html link="/DP.html" src="9938f0f795848274294631bb0d8fa323.jpg" alt="《宝可梦 钻石／珍珠》" width="588" height="331" %}</td>
<td>{% include figure.html link="/Pt.html" src="efdd474ffac175997868fa704bdc063e.jpg" alt="《宝可梦 白金》" width="588" height="331" %}</td>
</tr><tr>
<td>{% include figure.html link="/HGSS.html" src="3e6f40d11d826cc1f4babd1c5b2147b0.jpg" alt="《宝可梦 心金／魂银》" width="588" height="331" %}</td>
<td>{% include figure.html link="/PKHeX.html" src="a7b4b821e754b775055372bb0380bc0d.png" alt="可用于《宝可梦》第四世代汉化修正版的PKHeX版本" width="588" height="331" %}</td>
</tr></tbody></table>

## 功能介绍

##### 连接交换

自《白金／心金／魂银》1.5.0版、《钻石／珍珠》2.0.0版起，第四世代所有游戏之间均采用了完全相同的码表，因此可以实现不同版本之间的连接交换，而不会出现乱码问题。

对于新版本汉化修正补丁读取旧版本存档时可能存在的乱码问题，请参见[各汉化版本的码表兼容性]({{ "CharTable.html" | relative_url }}#%E5%90%84%E6%B1%89%E5%8C%96%E7%89%88%E6%9C%AC%E7%9A%84%E7%A0%81%E8%A1%A8%E5%85%BC%E5%AE%B9%E6%80%A7)。

##### 宝可计步器汉化

（1.5.0版加入，OS翻译、圈叉汉化组制作）

详情：<https://bbs.oldmantvg.net/thread-18167.htm>

《心金／魂银》发售时捆绑了一个宝可计步器。在拥有《心金／魂银／黑／白／黑2／白2》正版卡带和宝可计步器的情况下，可以通过TWiLight Menu++ 运行汉化版游戏，将宝可计步器的汉化程序发送到宝可计步器中。

##### 通过“简单会话”解锁神秘礼物

（2.0.0版加入）

在第四世代游戏中，可以通过输入特定的短语，解锁神秘礼物，或获得特殊壁纸、宝可梦的蛋等。在《钻石／珍珠／白金》中，需要对祝庆电视台三楼的制作人输入。在《心金／魂银》中，需要对桔梗市宝可梦中心的阿始输入。

在《钻石／珍珠／白金》中，输入特定的短语后可以解锁神秘礼物界面。神秘礼物的开启密语为“大家 快乐 Wi-Fi 连接”。

此外，特殊壁纸、宝可梦的蛋密语的四个词语是根据训练家ID（ID No.）确定的，可以使用[密语计算器]({{ "/Aikotoba.html" | relative_url }})计算适用于汉化版的密语。

##### 第三、四世代汉化版联动

（2.1.0版加入）

详情：<https://bbs.oldmantvg.net/thread-44009.htm>

第三、四世代《宝可梦》游戏可通过伙伴公园联动，从而将第三世代的宝可梦传送到第四世代。自2.1.0版本起，加入了一个中文字符转换程序，允许将口袋群星SP汉化组制作的《火红／叶绿／绿宝石》汉化版中的宝可梦传送到第四世代而不会出现乱码的问题。

##### 日文宝可梦名字补丁

（2.1.0版加入）

考虑到部分玩家的联机、向后世代传送的需求，特别提供了一个日文宝可梦名字补丁。使用这个补丁后，游戏中新捕捉的宝可梦的名字将会使用日文名（不影响已有的宝可梦）。

如需使用，需要先对ROM使用通常版本的补丁，再使用此补丁。

## 常见问题

##### 1. 提示“原始ROM的MD5校验失败”是怎么回事？

原始ROM为日本版ROM，也可使用已经打好2.0.0及以后版本的补丁的ROM。

如果你使用过日文宝可梦名字补丁，那么应用更新补丁时也会提示“原始ROM的MD5校验失败”，这是正常现象。

##### 2. 如何获取ROM？

如果你有正版卡带，请参照[这篇文章](https://haroohie.club/zh-hans/chokuretsu/guide/dumping-the-rom)提取ROM。

##### 3. 正版卡带能否使用补丁？

目前还没有能够给正版卡带打补丁的方式。你仍然需要提取ROM后对ROM打补丁，然后使用烧录卡或[TWiLight Menu++](https://github.com/DS-Homebrew/TWiLightMenu) 运行。采用TWiLight Menu++ 时，通过特定操作可以读写正版卡带的存档，请参照[这篇文章](https://bbs.oldmantvg.net/thread-22894.htm)。

##### 4. 如何确定汉化修正版的版本？

在标题屏幕上按 <kbd>↑ + SELECT + B</kbd>，即可查看汉化修正版的版本信息。注意这个组合按键实际上是删除保存数据的按键，如果你只是想查看版本信息，请务必不要误删存档。

## 截图预览

<div style="display: flex; flex-wrap: wrap;">
{% include figure.html src="title-D.png" alt="《宝可梦 钻石》标题" %}
{% include figure.html src="title-P.png" alt="《宝可梦 珍珠》标题" %}
{% include figure.html src="title-Pt.png" alt="《宝可梦 白金》标题" %}
{% include figure.html src="title-HG.png" alt="《宝可梦 心金》标题" %}
{% include figure.html src="title-SS.png" alt="《宝可梦 魂银》标题" %}
</div>

## 进阶说明

如果你有任何问题反馈、意见建议，请在网站评论区留言，或通过[Bilibili](https://space.bilibili.com/16114399)私信联系我。

如果你想获取最新的汉化版本，可以访问[GitHub上的项目仓库](https://github.com/Xzonn/PokemonChineseTranslationRevise)。

如果你希望自己处理文本、字库、替换narc中的文件，可以使用[我编写的命令行工具]({{ "Tools.html" | relative_url }})，或是使用[字库扩容补丁](https://xzonn.top/posts/Pokemon-Gen-4-Font-Patch.html)。
