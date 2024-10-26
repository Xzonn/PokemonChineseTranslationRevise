---
is_patch: true
last_modified_at: 2024-10-26 23:08
title: 《宝可梦 心金／魂银》汉化修正补丁
---
<div class="alert alert-info" role="alert">
<p>如果您发现了本补丁的错误，或是对本补丁有任何意见建议，请在 <strong><a href="#pg-content-comment" class="alert-link">留言区</a></strong> 留言。</p>
</div>

<div class="alert alert-success" role="alert">
<p>本补丁欢迎转载，转载时请留下本页面链接以方便反馈。</p>
</div>

<div class="alert alert-danger" role="alert">
<p><strong>本补丁严禁商用，如因商业使用造成法律纠纷概与本人无关。</strong></p>
</div>

{% include figure.html src="https://images-na.ssl-images-amazon.com/images/I/91zZqHcE78L.jpg" width="200" height="200" alt="《心金》封面" class="pull-right" %}

{% include figure.html src="https://images-na.ssl-images-amazon.com/images/I/917ewchZrjL.jpg" width="200" height="200" alt="《魂银》封面" class="pull-right" %}

## 发布链接
考虑到便利性，本人仅发布修正补丁，使用方式已在压缩包中，可自行查看。发布链接：

- GitHub：[2.1.0](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/v2.1.0)。
- 百度网盘：[2.1.0](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ?pwd=pkmn)。

日文版 ROM 的 MD5 校验码：

- [《心金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4168)：`e3f7933aee8cc2694629293f16c1c0a8`
- [《魂银》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4169)：`a1211b2d44e551197712177e3d50b491`

## 修改内容
- 标题图标改为“精灵宝可梦 心灵之金”“精灵宝可梦 灵魂之银”。（由 **[大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作）
- 整合了宝可计步器汉化文本及汉化补丁（由 **OS** 翻译、**圈叉汉化组** 制作，**[发布链接](https://bbs.oldmanemu.net/thread-18167.htm)**）。
- 导入了一部分《宝可梦 晶灿钻石／明亮珍珠》的官方文本。
- 对宝可梦、招式、道具、特性的名字、介绍等修改为官方文本。其中部分特性介绍的官方文本较长，本人对其进行了缩减。
- 对一部分人物、地点的名字修改为官方译名。
- 对原文相同的长文本统一了译文文本。
- 其他零碎的修改内容。

本修正补丁所用的字库基于原《白金》汉化版的扩充，同时与本人修正的其他第四世代游戏的字库通用，因此在这些游戏之间可以直接交换宝可梦而不出现乱码。

如需修改宝可梦、主角、劲敌的名字，请使用[本人修改的PKHeX]({{ "/PKHeX.html" | relative_url }})。

对于新版本汉化修正补丁读取旧版本存档时可能存在的乱码问题，参见 **[各汉化版本的码表兼容性]({{ "/CharTable.html" | relative_url }})**。

<div class="alert alert-warning" role="alert">
<p><strong>注意</strong>：本修正补丁并不与原《心金／魂银》汉化版字库通用，如需将原汉化版游戏存档应用于本人修正版，请使用上述版本PKHeX修正宝可梦昵称。</p>
<p>同时，本修正补丁并不与第三、第五世代字库通用，因此如需跨世代传输，请先将宝可梦的昵称改为日文昵称。</p>
</div>

## 更新日志
- 2.1.0（2024-10-27）：
  - 整合了第三、四世代汉化版中文字符转换程序（感谢 **圈叉汉化组** 制作并允许本项目整合加入，**[发布链接](https://bbs.oldmantvg.net/thread-44009.htm)**）。
  - 整合了Wi-Fi Connection设置界面的汉化程序（感谢 **天涯**、**F君** 汉化制作并允许本项目整合加入，**[发布链接](https://github.com/R-YaTian/DS-Internet-CHS)**）。
  - 修复部分招式文本控制符错误的问题。
  - 修复训练家卡无法显示星星的问题。
  - 额外提供了保留日文宝可梦名字的补丁（对于大多数人来说并不需要此补丁）。如需使用，需要先对ROM使用通常版本的补丁，再使用此补丁。
- 2.0.0（2024-02-04）：
  - **应用补丁所需要的原始 ROM 由原汉化版本修改为日本版**。请注意，《钻石／珍珠》需要 Rev 6 版本，以其他版本作为原始 ROM 会导致生成的 ROM 无法运行。补丁应用工具会自动计算原始 ROM 的 MD5 校验码，如果没有提示“原始 ROM 的 MD5 校验失败”，则说明原始 ROM 的版本正确。
  - 采用了更细致的匹配算法，导入了更多与《晶灿钻石／明亮珍珠》等官方中文化游戏匹配的文本。
  - 版本号修改为删除存档界面显示，可在标题界面按`↑ + SELECT + B`查看。
  - 修复招式文本与实际效果不一致的问题。此问题自从导入官方招式文本后就一直存在，此前未能引起注意。
  - 修复《心金》图鉴文本实际为《魂银》图鉴文本的问题。（感谢 **Konyaka** 反馈）
  - 修复几处小图标未更新至官译的问题。
  - 修复了多处翻译不准确、不恰当的地方。（非常感谢 **[爱儿aiko](https://space.bilibili.com/101749351)** 细心反馈并提出了很多修改建议）
- 1.5.0（2022-06-03）：
  - 导入部分《晶灿钻石／明亮珍珠》的译名。
  - 整合了宝可计步器汉化文本及汉化补丁（感谢 **OS** 翻译、**圈叉汉化组** 制作并允许本项目整合加入，**[发布链接](https://bbs.oldmanemu.net/thread-18167.htm)**）。
  - 版本号修改到初始菜单的神秘礼物中。
  - 其他部分修正。
- 1.4.1（2021-06-28）：
  - 改用[自制的汉化补丁应用工具]({{ "/Tools.html" | relative_url }})。
  - 部分译名和控制符修正。
  - 本次更新未对《钻石／珍珠》进行修改。
- 1.4.0（2021-02-27）：
  - 部分译名和控制符修正。
  - 本次更新未对《钻石／珍珠》进行修改。
- 1.3.0（2020-04-22）：
  - 将道具说明修正为与第四世代相符。（感谢 **[刀刀是个ky](https://space.bilibili.com/313754647)** 反馈）
  - 将部分名词修正为官方译名（如捕虫大赛、超级华丽大赛等）。
  - 将部分地理名词修正为常用译名。
  - 其他部分修正（感谢 **[TONE-TONG](https://space.bilibili.com/32451014)**、**k90单反**、**MrVilla**、**yggdra**等反馈）
- 1.2.0（2020-03-24）：
  - 更新标题图标。（由 **[大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作）
- 1.1.0（2020-03-05）：
  - 根据《精灵宝可梦 究极之日》的文本修改了部分用语。
- 1.0.0（2020-02-28）：
  - 最初发布版本。

## 预览
{% include figure.html src="ed366964bfaf73e862883782181a9ec1.png" alt="《宝可梦 心金》标题" %}

{% include figure.html src="4664ff3f027ff954c174ed5d1ea2fd4e.png" alt="《宝可梦 魂银》标题" %}

{% include figure.html src="f5bb8f665bcf1d57bdbe8bc191ac7b60.png" alt="宝可梦图鉴" %}

## 致谢
感谢 **[大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作并授权本人使用标题图标，原始发布地址：<https://tieba.baidu.com/p/4518179164?see_lz=1>。

感谢开发商：Game Freak以及发行商：The Pokémon Company、任天堂，正是他们的辛勤工作才让我们在丰富的宝可梦游戏世界中探索。

感谢原汉化组：ACG汉化组，正是他们的不懈努力，让玩家能在没有官方中文的条件下仍能够没有语言障碍地游玩宝可梦游戏。

ACG汉化组发布页面：[【ACG】0060-NDS-ACG汉化组作品《口袋妖怪：心灵之金》](http://zt.tgbus.com/acghh/Work/2010/07/02/18593618170.shtml)／[【ACG】0061-NDS-ACG汉化组作品《口袋妖怪：灵魂之银》](http://zt.tgbus.com/acghh/Work/2010/07/09/23061618359.shtml)

汉化名单：

- 破解：Joyce，EW
- 翻译：囧囧的小霏，悠幻翼逝，xiaolincryst，猫姫やんこ，海客，水天兰，WGWG，倒霉孩子，takutaku，零，Aquariusの群，小侠，Our Story℃
- 润色：圣月祭司，Our Story℃
- 美工：OPERA，骏狼不凡
- 名词：pk迪、fb1324
- 测试：Rambutan、宝石海星、圣幻光线、ganzihe，河马，无冰南极，bless1001

感谢为宝可梦官方中文化努力的所有玩家，正是他们的不断请愿才让官方发现了华语圈的广阔市场，并让官方自《太阳／月亮》以来发售了越来越多的官方中文游戏。

译文文本参考了任天堂官方游戏文本，记载于神奇宝贝百科，按照[“署名-非商业性使用-相同方式共享 3.0”](https://creativecommons.org/licenses/by-nc-sa/3.0/deed.zh)授权。
