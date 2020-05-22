<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="ProjectileStar" tilewidth="17" tileheight="17" spacing="4" tilecount="4" columns="4">
 <image source="../Sprites/ProjectileStar.png" width="80" height="17"/>
 <tile id="0">
  <properties>
   <property name="Animation" value="ProjectileStar"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="4" y="2" width="13" height="13">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="0" duration="100"/>
   <frame tileid="1" duration="100"/>
   <frame tileid="2" duration="100"/>
   <frame tileid="3" duration="100"/>
  </animation>
 </tile>
 <tile id="1">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="0" width="17" height="17">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="0" width="17" height="17">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="0" width="17" height="17">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
</tileset>
