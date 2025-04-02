---
is_patch: true
last_modified_at: 2025-04-02 15:01
title: 《宝可梦 白金》汉化修正补丁
---
{% include figure.html src="efdd474ffac175997868fa704bdc063e.jpg" width="588" height="331" %}

## 发布链接
<div class="alert alert-info text-center" role="alert" markdown="1" style="font-size: 2rem;">
**[GitHub](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/latest)·[百度网盘](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ?pwd=pkmn)**
</div>

修正补丁使用方式已在压缩包中，也可参照[此视频](https://www.bilibili.com/video/BV1oH1xYXEdb/)。

日文版ROM的MD5校验码：

- [《白金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=2641)：`8271f64f5c7fb299adf937a8be6d8c88`

## 修改内容
- 标题图标改为“精灵宝可梦 白金”（由 **[大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作）。
- 整合了第三、四世代汉化版中文字符转换程序（由 **圈叉汉化组** 制作，**[发布链接](https://bbs.oldmantvg.net/thread-44009.htm)**）。
- 整合了Wi-Fi Connection设置界面的汉化程序（由 **天涯**、**F君** 制作，**[发布链接](https://github.com/R-YaTian/DS-Internet-CHS)**）。
- 导入了一部分《宝可梦 晶灿钻石／明亮珍珠》的官方文本。
- 对宝可梦、招式、道具、特性的名字、介绍等修改为官方文本。其中部分特性介绍的官方文本较长，本人对其进行了缩减。
- 对一部分人物、地点的名字修改为官方译名。
- 对原文相同的长文本统一了译文文本。
- 其他零碎的修改内容。

本修正补丁所用的字库基于原《白金》汉化版的扩充，同时与本人修正的其他第四世代游戏的字库通用，因此在这些游戏之间可以直接交换宝可梦而不出现乱码。

如需修改宝可梦、主角、劲敌的名字，请使用[本人修改的PKHeX]({{ "/PKHeX.html" | relative_url }})。

对于新版本汉化修正补丁读取旧版本存档时可能存在的乱码问题，参见 **[各汉化版本的码表兼容性]({{ "/CharTable.html" | relative_url }}#各汉化版本的码表兼容性)**。

<div class="alert alert-warning" role="alert">
<p><strong>注意</strong>：本修正补丁并不与第五世代汉化版字库通用，因此如需跨世代传输，请先将宝可梦的昵称改为日文昵称。</p>
</div>

## 更新日志
- 2.1.0（2024-10-27）：
  - 整合了第三、四世代汉化版中文字符转换程序（感谢 **圈叉汉化组** 制作并允许本项目整合加入，**[发布链接](https://bbs.oldmantvg.net/thread-44009.htm)**）。
  - 整合了Wi-Fi Connection设置界面的汉化程序（感谢 **天涯**、**F君** 汉化制作并允许本项目整合加入，**[发布链接](https://github.com/R-YaTian/DS-Internet-CHS)**）。
  - 修复部分招式文本控制符错误的问题。
  - 修复《白金》神奥图鉴在部分情况下仅显示编号小于等于151的宝可梦的问题（感谢 **[Rainzard](https://github.com/Rainzard)** 反馈）。
  - 修复训练家卡无法显示星星的问题。
  - 额外提供了保留日文宝可梦名字的补丁（对于大多数人来说并不需要此补丁）。如需使用，需要先对ROM使用通常版本的补丁，再使用此补丁。
- 2.0.0（2024-02-04）：
  - **应用补丁所需要的原始ROM由原汉化版本修改为日本版**。请注意，《钻石／珍珠》需要Rev 6版本，以其他版本作为原始ROM会导致生成的ROM无法运行。补丁应用工具会自动计算原始ROM的MD5校验码，如果没有提示“原始ROM的MD5校验失败”，则说明原始ROM的版本正确。
  - 采用了更细致的匹配算法，导入了更多与《晶灿钻石／明亮珍珠》等官方中文化游戏匹配的文本。
  - 版本号修改为删除存档界面显示，可在标题界面按`↑ + SELECT + B`查看。
  - 修复招式文本与实际效果不一致的问题。此问题自从导入官方招式文本后就一直存在，此前未能引起注意。
  - 修复《钻石／珍珠／白金》无法开启神秘礼物的问题。神秘礼物的开启密语为“大家 快乐 Wi-Fi 连接”（感谢 **[兔隐](https://tieba.baidu.com/home/main?id=tb.1.b078b4c8.5EUyhmk8zkrkK__di08swQ)** 等人反馈，[链接](https://tieba.baidu.com/p/7213514184)）。
  - 修复几处小图标未更新至官译的问题。
  - 修改《钻石／珍珠／白金》简单会话的词语排序。
  - 修复了多处翻译不准确、不恰当的地方（非常感谢 **[爱儿aiko](https://space.bilibili.com/101749351)** 细心反馈并提出了很多修改建议）。
- 1.5.0（2022-06-03）：
  - 导入部分《晶灿钻石／明亮珍珠》的译名。
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
  - 将道具说明修正为与第四世代相符（感谢 **[刀刀是个ky](https://space.bilibili.com/313754647)** 反馈）。
  - 将部分名词修正为官方译名（如捕虫大赛、超级华丽大赛等）。
  - 将部分地理名词修正为常用译名。
  - 其他部分修正（感谢 **[TONE-TONG](https://space.bilibili.com/32451014)**、**k90单反**、**MrVilla**、**yggdra**等反馈）
- 1.2.0（2020-03-24）：
  - 更新标题图标（由 **[大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作）。
- 1.1.0（2020-03-05）：
  - 根据《精灵宝可梦 究极之日》的文本修改了部分用语。
- 1.0.0（2020-02-28）：
  - 最初发布版本。

## 预览
{% include figure.html src="7dc3f1452c6bd5c7779686929f96a6bc.png" alt="《宝可梦 白金》标题" %}

{% include figure.html src="963793c79538e2cd529d57d3604f9a54.png" alt="宝可梦图鉴列表" %}

{% include figure.html src="ea402e21fcae399bdf8a1bd137cfed09.png" alt="宝可梦图鉴介绍" %}

## 致谢
感谢 **[大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作并授权本人使用标题图标，原始发布地址：<https://tieba.baidu.com/p/4518179164?see_lz=1>。

感谢开发商：Game Freak以及发行商：The Pokémon Company、任天堂，正是他们的辛勤工作才让我们在丰富的宝可梦游戏世界中探索。

感谢原汉化组：ACG汉化组，正是他们的不懈努力，让玩家能在没有官方中文的条件下仍能够没有语言障碍地游玩宝可梦游戏。

ACG汉化组发布页面：[0038-NDS-口袋妖怪:白金](http://zt.tgbus.com/acghh/Work/2009/08/01/1057257087.shtml)

汉化名单：

- 破解：
  - 技术顾问：flyeyes
  - 文本解密：chyt
  - 资源拆包：阿德
  - 文本破解：Joyce
  - 字库破解：Joyce
  - 图片破解：easyWorld
- 美工：LOGO制作：骏狼不凡  字模制作：hippopo
- 翻译：由基拉，海客，oddboy，降谷建志，東瀛流水，无恨生，忘却巡回，S-R-X，小白羊，chunjie，真红命，阿牛，-273℃，佐仓蜜柑，OurStory℃
- 润色：ahlai，wjy1987，zmmc，brain，小心点，马里，宝石海星123，deathkira，winepanda1234，Fb1324，圣月祭司，pk迪
- 测试：溪云山雨，禾口言皆，LeeMingYu，樱染，lovemusic 
- 打杂：Pluto
- 友情支援：幻化绝影

感谢为宝可梦官方中文化努力的所有玩家，正是他们的不断请愿才让官方发现了华语圈的广阔市场，并让官方自《太阳／月亮》以来发售了越来越多的官方中文游戏。

译文文本参考了任天堂官方游戏文本，记载于神奇宝贝百科，按照[“署名-非商业性使用-相同方式共享 3.0”](https://creativecommons.org/licenses/by-nc-sa/3.0/deed.zh)授权。
