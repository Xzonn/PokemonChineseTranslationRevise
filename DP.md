---
is_patch: true
last_modified_at: 2023-11-21 22:40
title: 《宝可梦 钻石／珍珠》汉化修正补丁
---
<div class="alert alert-info" role="alert" style="margin-top: 15px;">
<p>如果您发现了本补丁的错误，或是对本补丁有任何意见建议，请在 <strong><a href="#xz-content-comment" class="alert-link">留言区</a></strong> 留言。</p>
</div>

<div class="alert alert-success" role="alert">
<p>本补丁欢迎转载，转载时请留下本页面链接以方便反馈。</p>
</div>

<div class="alert alert-danger" role="alert">
<p><strong>本补丁严禁商用，如因商业使用造成法律纠纷概与本人无关。</strong></p>
</div>

{% include figure.html src="https://images-na.ssl-images-amazon.com/images/I/51lt+86frXL.jpg" width="200" height="200" alt="《钻石》封面" class="pull-right" %}

{% include figure.html src="https://images-na.ssl-images-amazon.com/images/I/51TWlKkescL.jpg" width="200" height="200" alt="《珍珠》封面" class="pull-right" %}

## 发布链接
考虑到便利性，本人仅发布修正补丁，使用方式已在压缩包中，可自行查看。发布链接：

- GitHub：[2.0.0](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/v2.0.0)。
- 百度网盘：[2.0.0](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ?pwd=pkmn)。

日文版 ROM 的 MD5 校验码：

- [《钻石》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=6641)：`c1d66b8d4fbdbfa57ff4868970fe19d2`
- [《珍珠》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4929)：`751d4a0524e4ef38b153ccfb5d21271f`

## 修改内容
- 标题图标改为“精灵宝可梦 钻石”“精灵宝可梦 珍珠”。（由 **[@大神丶橡皮](https://tieba.baidu.com/home/main?un=%E5%A4%A7%E7%A5%9E%E4%B8%B6%E6%A9%A1%E7%9A%AE&ie=utf-8)** 制作）
- 导入了一部分《宝可梦 晶灿钻石／明亮珍珠》的官方文本。
- 对宝可梦、招式、道具、特性的名字、介绍等修改为官方文本。其中部分特性介绍的官方文本较长，本人对其进行了缩减。
- 对一部分人物、地点的名字修改为官方译名。
- 对原文相同的长文本统一了译文文本。
- 其他零碎的修改内容。

如需修改宝可梦、主角、劲敌的名字，请使用[本人修改的PKHeX]({{ "/PKHeX.html" | relative_url }})。

<div class="alert alert-warning" role="alert">
<p><strong>注意</strong>：本修正补丁并不与第五世代字库通用，因此如需跨世代传输，请先将宝可梦的昵称改为日文昵称。</p>
</div>

## 更新日志
- 2.0.0（2024-02-04）：
  - **应用补丁所需要的原始 ROM 由原汉化版本修改为日本版**。请注意，《钻石／珍珠》需要 Rev 6 版本，以其他版本作为原始 ROM 会导致生成的 ROM 无法运行。补丁应用工具会自动计算原始 ROM 的 MD5 校验码，如果没有提示“原始 ROM 的 MD5 校验失败”，则说明原始 ROM 的版本正确。
  - **同步更新《钻石／珍珠》补丁**，并修改《钻石／珍珠》的图标。（感谢 **[ppllouf](https://github.com/ppllouf)**）
  - 采用了更细致的匹配算法，导入了更多与《晶灿钻石／明亮珍珠》等官方中文化游戏匹配的文本。
  - 版本号修改为删除存档界面显示，可在标题界面按`↑ + SELECT + B`查看。
  - 修复招式文本与实际效果不一致的问题。此问题自从导入官方招式文本后就一直存在，此前未能引起注意。
  - 修复《钻石／珍珠／白金》无法开启神秘礼物的问题。神秘礼物的开启密语为“大家 快乐 Wi-Fi 连接”。（感谢 **[兔隐](https://tieba.baidu.com/home/main?id=tb.1.b078b4c8.5EUyhmk8zkrkK__di08swQ)** 等人反馈，[链接](https://tieba.baidu.com/p/7213514184)）
  - 修复几处小图标未更新至官译的问题。
  - 修改《钻石／珍珠／白金》简单会话的词语排序。**注意，这一修改会导致日版《钻石／珍珠》壁纸密语不适用于汉化版**。请参考[“密语计算器”]({{ "/Aikotoba.html" | relative_html }})页面的说明。
  - 修复了多处翻译不准确、不恰当的地方。（非常感谢 **[爱儿aiko](https://space.bilibili.com/101749351)** 细心反馈并提出了很多修改建议）
- 0.9.0（2020-04-22）：
  - 最初发布版本。

## 预览
{% include figure.html src="040c99ebeb494d5b5bfe10fc4bdb1d52.png" alt="《宝可梦 钻石》标题" %}

{% include figure.html src="7cf85ec45232b9930b9359268c6bd767.png" alt="《宝可梦 珍珠》标题" %}

## 致谢
感谢开发商：Game Freak以及发行商：The Pokémon Company、任天堂，正是他们的辛勤工作才让我们在丰富的宝可梦游戏世界中探索。

感谢原汉化组：YYJoy汉化组，正是他们的不懈努力，让玩家能在没有官方中文的条件下仍能够没有语言障碍地游玩宝可梦游戏。

YYJoy汉化组发布页面：[「口袋妖怪 珍珠 & 钻石」简体中文补丁！](http://bbs.yyjoy.com/thread-54130-1-1.html)（链接已失效）

汉化名单：

- 破解：DNA
- 协力：沙滩凉鞋
- 翻译：非典型性废言、猫猫爱雪、Jiofu、口袋茶叶、Tristan、初二的夏天、天使、Li9s 等……
- 润色：非典型性废言
- 美工：Deapho、月下风铃
- 监制：幺幺的任天堂
- 打杂：AK47
- 测试：Obi-Wan、月下雪影、泉此方、YYJoy 汉化组成员

感谢为宝可梦官方中文化努力的所有玩家，正是他们的不断请愿才让官方发现了华语圈的广阔市场，并让官方自《太阳／月亮》以来发售了越来越多的官方中文游戏。

译文文本参考了任天堂官方游戏文本，记载于神奇宝贝百科，按照[“署名-非商业性使用-相同方式共享 3.0”](https://creativecommons.org/licenses/by-nc-sa/3.0/deed.zh)授权。