# HollowKnight.SpritePacker
English | [简体中文](./README_cn.md)

Pack sprites that are dumped by GODump mod back into atlas.

## Usage
1. Dump sprites using GODump mod (v1.2 or above) with dumpAtlasOnce and dumpSpriteInfo settings set to true
2. Edit your sprites
3. Open SpritePacker and choose a collection you want to be packed into
4. If you dump sprites with **SpriteSizeFix**,you need to choose **v1.3** mode first
5. Press Check button to see if there are sprites that are supposed to be the same but are not
6. Choose the one sprite you want of the sprites in 4. and press Replace button to replace the rest with the one you choose
7. Repeat 4. and 5. until there are no error messages
8. Press Pack button and the atlas will be generated in 0.Atlases folder.

* Sprites dumped by mod are needed for this app to run correctly.
* Replace will create backup files.You can uncheck this backup behaver.
* You can manually add files to Changed Files or turn watcher on to automatically add those you edited and saved to Changed Files while this app is running.
* You can replace all files in Changed Files or restore selected files in Backup Files at once.
* Double click to remove file from list.

## Update
* v1.3 Add Backup Files and Changed Files.Change code logic.Add new bugs.
* v1.3.1  Fix a logic glinch that causes border sprites not being fully packed.
* v1.3.2  Png files in sprites folder that are not created by mod no longer crash this app.
* v1.3.3  Set default settings to Language: zh-CN, GODump: v1.3, Backup: false, Watch: On.

## Credits
* [Team Cherry](https://teamcherry.com.au/) - Without which, we would not have Hollow Knight.

## License
[GPL-3.0](https://choosealicense.com/licenses/gpl-3.0/)