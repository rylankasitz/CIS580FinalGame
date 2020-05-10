<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="ProjectileFire" tilewidth="8" tileheight="31" spacing="5" tilecount="7" columns="7">
 <image source="../Sprites/ProjectileFire.png" width="86" height="31"/>
 <tile id="0">
  <properties>
   <property name="Animation" value="ProjectileFire"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="3" y="27" width="5" height="4"/>
  </objectgroup>
  <animation>
   <frame tileid="0" duration="100"/>
   <frame tileid="1" duration="100"/>
   <frame tileid="2" duration="100"/>
   <frame tileid="3" duration="100"/>
   <frame tileid="4" duration="100"/>
   <frame tileid="5" duration="100"/>
   <frame tileid="6" duration="2000"/>
  </animation>
 </tile>
 <tile id="1">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="22" width="7" height="9"/>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="21" width="8" height="10"/>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="15" width="7" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="13" width="8" height="18"/>
  </objectgroup>
 </tile>
 <tile id="5">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="7" width="7" height="24"/>
  </objectgroup>
 </tile>
 <tile id="6">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="5" width="8" height="26"/>
  </objectgroup>
 </tile>
</tileset>
