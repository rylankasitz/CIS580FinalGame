<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="ProjectileBubble" tilewidth="16" tileheight="16" tilecount="4" columns="4">
 <image source="../Sprites/ProjectileBubble.png" width="64" height="16"/>
 <tile id="0">
  <properties>
   <property name="Animation" value="ProjectileBubble"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" type="Collision" x="2" y="5" width="7" height="6"/>
  </objectgroup>
  <animation>
   <frame tileid="0" duration="400"/>
   <frame tileid="1" duration="400"/>
   <frame tileid="2" duration="400"/>
   <frame tileid="3" duration="400"/>
  </animation>
 </tile>
 <tile id="1">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="2" y="4" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="2" width="12" height="12"/>
  </objectgroup>
 </tile>
</tileset>
