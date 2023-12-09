---
is_patch: true
last_modified_at: 2023-12-09 22:39
title: 开发版本补丁概述
---
<div class="alert alert-info" role="alert" style="margin-top: 15px;">
<p>如果您发现了本补丁的错误，或是对本补丁有任何意见建议，请在 <strong><a href="#xz-content-comment" class="alert-link">留言区</a></strong> 留言。</p>
</div>

<div class="alert alert-danger" role="alert">
<p><strong>为了防止误解，请勿转载开发版本的补丁，如需转载，请转载正式版本的补丁。</strong></p>
</div>

## 发布链接
开发版本补丁仅提供GitHub链接，补丁内容会随着每次提交而更新。

- GitHub：[开发版本](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/dev-pub)。

## 说明
尽管1.5.0版本的补丁对于译名的替换已经比较充分，但仍然存在各种疏漏，如翻译不恰当、控制符错误、未替换的旧译名、新译名需要更新等，而这些小问题又不值得发布一个新的版本，因此需要一个开发版本的补丁及时修正错误，等到修改的内容足够多时一并发布。另外，一些小测试（例如字体优化等）也会在开发版本中进行。

另外，[目前《钻石／珍珠》原汉化版本无法正常打包的问题已经解决了](/posts/Pokemon-DP-Chinese-Localization-Based-on-Pret-Project.html)，使得《钻石／珍珠》可以通过和《白金／心金／魂银》同样的构建方式处理。因此，开发版本补丁也会包含《钻石／珍珠》的内容。

## 更新内容
开发补丁相较于1.5.0版本有如下更新（将会作为2.0.0版本更新的内容）：

- 应用补丁所需要的原始ROM由原汉化版本修改为日本版。请注意，《钻石／珍珠》需要Rev 6版本，以其他版本作为原始ROM会导致生成的ROM无法运行。各版本MD5校验码如下，补丁应用工具会自动计算原始ROM的MD5校验码，如果没有提示“原始 ROM的MD5校验失败”，则说明原始ROM的版本正确。
  - [《钻石》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=6641)：`c1d66b8d4fbdbfa57ff4868970fe19d2`
  - [《珍珠》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4929)：`751d4a0524e4ef38b153ccfb5d21271f`
  - [《白金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=2641)：`8271f64f5c7fb299adf937a8be6d8c88`
  - [《心金》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4168)：`e3f7933aee8cc2694629293f16c1c0a8`
  - [《魂银》](https://datomatic.no-intro.org/index.php?page=show_record&s=28&n=4169)：`a1211b2d44e551197712177e3d50b491`
- 同步更新《钻石／珍珠》补丁，并修改《钻石／珍珠》的图标。（感谢 **[@ppllouf](https://github.com/ppllouf)**）
- 采用了更细致的匹配算法，导入了更多与《晶灿钻石／明亮珍珠》等官方中文化游戏匹配的文本。
- 版本号修改为删除存档界面显示，可在标题界面按`↑ + SELECT + B`查看。
- 修复招式文本与实际效果不一致的问题。此问题自从导入官方招式文本后就一直存在，此前未能引起注意。
- 修复《心金》图鉴文本实际为《魂银》图鉴文本的问题。（感谢 **Konyaka** 反馈）
- 修复《钻石／珍珠／白金》无法开启神秘礼物的问题。神秘礼物的开启密语为“大家 快乐 Wi-Fi 连接”。（感谢 **[兔隐](https://tieba.baidu.com/home/main?id=tb.1.b078b4c8.5EUyhmk8zkrkK__di08swQ)** 等人反馈，[链接](https://tieba.baidu.com/p/7213514184)）
- 修复《白金》标题动画显示错误的问题。（感谢 **[@ppllouf](https://github.com/ppllouf)**）
- 修改《钻石／珍珠／白金》简单会话的词语排序。**注意，这一修改会导致日版《钻石／珍珠》壁纸密语不适用于汉化版。**请参考[“密语计算器”]({{ "/Aikotoba.html" | relative_html }})页面的说明。
- 修复几处小图标未更新至官译的问题。

## 预览
{% include figure.html src="040c99ebeb494d5b5bfe10fc4bdb1d52.png" alt="《宝可梦 钻石》标题" width="256" height="384" %}

{% include figure.html src="7cf85ec45232b9930b9359268c6bd767.png" alt="《宝可梦 珍珠》标题" width="256" height="384" %}