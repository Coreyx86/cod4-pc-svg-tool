# cod4-pc-svg-tool
 A tool to view and modify debug savegame files for Call of Duty 4 for PC & Xbox.

# This only supports singleplayer savegame files.

# You must use a developer savegame file in order to use this tool properly!

# PC Instructions For Obtaining SaveFile

  - Load any mission in campaign
  - As soon as the mission is finished loading, open the console (~) and enable 'developer' ('/developer 2') and then save & quit.
  - The save file (located in the players/profiles/[account]/save/ subdirectory of the game folder) should be significantly larger than the other savegame files. (~3000KB)
  - If you're not sure if your savegame is a proper developer savegame, open the file up in a hex editor and search for the string "main()" or "#include", if any results return then you have a proper file. 

# Xbox Instructions For Obtaining SaveFile

  - Load any mission in campaign
  - As soon as the mission is finished loading, save and quit.
  - Transfer Save file to USB and use a 3rd party Xbox SaveTool to enable the 'developer' DVAR on the save file.
  - Transfer modified save file to Xbox and load the save game and save and quit again.
  - Transfer the new save game file to USB
  - If you're not sure if your savegame is a proper developer savegame, open the file up in a hex editor and search for the string "main()" or "#include", if any results return then you have a proper file. 