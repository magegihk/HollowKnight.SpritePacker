# HollowKnight.SpritePacker
[English](./README.md) | 简体中文

将用GODump mod导出的《空洞骑士》中游戏对象的精灵图（sprite）重新包装回图集（atlas）。

## 使用
1. 使用GODump mod （v1.2或以上），将参数dumpAtlasOnce 和 dumpSpriteInfo设为true，导出精灵图。
2. 编辑精灵图
3. 打开精灵图打包鸡，选择需要包装的原始图集
4. 如果你导出的时候在GODump mod中设置了**SpriteSizeFix**，你需要选择**已修**模式
5. 按**检查**键查看是否有本应相同却只编辑了一个的重复精灵图
6. 选择你要的精灵图，按**替换**键替换应该相同的精灵图为你选的那个
7. 重复4.和5.直到没有error信息
8. 按**打包**键然后新生成的图集将出现在0.Atlases文件夹。

## 更新
* v1.3 新增备份还原；新增监视被修改文件一键全替换；全面调整逻辑结构；新增了一些bug。
 

## 感谢
* [Team Cherry](https://teamcherry.com.au/) - 好！

## License
[GPL-3.0](https://choosealicense.com/licenses/gpl-3.0/)