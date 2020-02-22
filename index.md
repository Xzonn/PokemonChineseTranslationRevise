---
date: 2020-02-22 23:12
layout: default
title: 《精灵宝可梦》第四世代汉化修正
---
## 说明
2016年对于《精灵宝可梦》系列的中文玩家来说是非常重要的一年，在这一年的2月26日，官方首次公布了《精灵宝可梦》系列对应简体中文和繁体中文的消息。随着首个官方中文游戏《精灵宝可梦 太阳／月亮》的发布，“口袋妖怪”世代成为了过去，<strong lang="ja">ポケットモンスター</strong>的正式名称被确定为“**精灵宝可梦**”。

由于官方中文化确定了宝可梦、招式、道具、特性等许多名词的译名，因此，为使官方译名更加深入人心，同时为中文玩家在游玩“口袋妖怪”时代的爱好者汉化作品时能够方便地查找资料，本人对<span lang="ja">『ポケットモンスター プラチナ』</span>（参考译名：《精灵宝可梦 白金》）原汉化版的译名进行了修正，并发布了出来。

《白金》的汉化修正版发布后得到了一些反馈，有朋友希望能够统一第四世代的码表。因此我重写了使用工具的代码，并将代码和编译后的程序发布出来，供有兴趣的朋友研究。

## 工具发布
发布链接：[GitHub：1.0.1版本](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/1.0.1) &#124; 源代码：[GitHub](https://github.com/Xzonn/PokemonChineseTranslationRevise/)

本人所使用的工具由C#编写，按照功能区分为文本处理工具和字库处理工具，均为命令行程序，无图形化界面。命令行参数可通过`--help`指令查看。

文本处理工具为`PokemonCTRText.exe`，目前实现的功能包括：从narc文件导出txt文本、将txt文本导入并创建新的narc文件。

字库处理工具为`PokemonCTRFont.exe`，目前实现的功能仅有：从narc文件根据字库创建新的narc文件。

详细说明请见：**[汉化工具说明](./Tools.html)**。

## 补丁发布
由于版权原因，本人仅发布修正补丁，使用方式已在压缩包中，可自行查看。发布链接：

- 《精灵宝可梦 白金》：[GitHub：0.9.3版本](https://github.com/Xzonn/PokemonChineseTranslationRevise/releases/tag/0.9.3) &#124; [百度网盘：0.9.3版本](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ)（提取密码：`pkmn`）

修改内容如下：

- 对宝可梦、招式、道具、特性的名字、介绍等修改为官方文本。其中部分特性介绍的官方文本较长，本人对其进行了缩减。
- 对一部分人物的名字修改为官方译名，其中未在游戏中出现的，以动画中的翻译代替。
- 对标题图标进行更改。

由于本人技术所限，未能对游戏中的一些图片进行更改。此外，由于文本更改为批量替换，可能存在替换错误的情况，如有错误，请及时向我反馈。

## 预览
<table class="table">
    <thead>
        <tr><th>原版</th><th>修正版</th></tr>
    </thead>
    <tbody>
        <tr><td><img src="https://file.moetu.org/images/2020/02/20/e23ee94c4705bb8188cde6ee2ba684f2370e44e045b982e4.png" alt="预览图" data-disp="auto" /></td><td><img src="https://file.moetu.org/images/2020/02/20/89d999dd37bc6baf509483cda87e1b48c4f0d13c86dd37ed.png" alt="预览图" data-disp="auto" /></td></tr>
        <tr><td><img src="https://file.moetu.org/images/2020/02/20/711af3dd3ffa60ce599ba9464a9aca6b3b39aa5fe3bc0d41.png" alt="预览图" data-disp="auto" /></td><td><img src="https://file.moetu.org/images/2020/02/20/963793c79538e2cd529d57d3604f9a5487894f6bde7ed3d3.png" alt="预览图" data-disp="auto" /></td></tr>
        <tr><td><img src="https://file.moetu.org/images/2020/02/20/d15725931787f5b1a11f0b075fd397f7a9ef59c9e0cb3fab.png" alt="预览图" data-disp="auto" /></td><td><img src="https://file.moetu.org/images/2020/02/20/ea402e21fcae399bdf8a1bd137cfed093cf0fef6a8e71bb8.png" alt="预览图" data-disp="auto" /></td></tr>
    </tbody>
</table>