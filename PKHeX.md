---
last_modified_at: 2022-03-02 10:07
title: 可用于《宝可梦》第四世代汉化修正版的PKHeX版本
---
{% include video.html aid="96227144" page="2" %}

{% include figure.html src="a7b4b821e754b775055372bb0380bc0d.png" width="763" height="385" alt="可用于《宝可梦》第四世代汉化修正版的PKHeX版本" %}

## 发布链接
- GitHub：[20220301](https://github.com/Xzonn/PKHeX/releases/latest)。
- 百度网盘：[20220301](https://pan.baidu.com/s/1tLhRCJjMfZJuxZSvD4I1GQ)（`pkmn`）。

## 说明
PKHeX项目网站：<https://projectpokemon.org/home/files/file/1-pkhex/>。

PKHeX项目GitHub地址：<https://github.com/kwsch/PKHeX>。

由于汉化版使用的码表在官方游戏中是不存在的，因此原版的PKHeX读取汉化版的宝可梦名字会出现错误。好在PKHeX是基于GPL v3的开源软件，因此我修改了码表和日文宝可梦名字并编译，得到了这一版本。

由于技术限制，不建议将此版本用于修改除《白金／心金／魂银》汉化修正版之外的游戏存档。

另外，由于我修改了日文宝可梦名字，因此汉化修正版中的中文名宝可梦不会被本版本视为非法宝可梦，但实际上该宝可梦仍为非法宝可梦。请不要将中文名宝可梦用于联网操作或是跨世代传输。

（2020/02/29更新：将日文宝可梦名字修改范围扩大为全部宝可梦，可以供第五、六世代官方译名版游戏使用。）

{% include figure.html src="62ab991310328dbb044a66eb3693610c.png" width="396" height="273" alt="利用PKHeX批量修改宝可梦昵称" %}

## 批量修改宝可梦昵称
PKHeX具有批量操作的功能。具体操作方法为（以下说明均基于中文界面）：

- 打开存档。
- 在工具栏中选择“工具”→“数据整理”→“批量编辑器”。
- 在新窗口中，选择“盒子”或“队伍”。
- 在输入框中输入：

```
.IsNicknamed=False
```

- 点击“运行”。