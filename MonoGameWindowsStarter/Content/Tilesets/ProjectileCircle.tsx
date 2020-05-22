<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="ProjectileCircle" tilewidth="16" tileheight="16" spacing="8" tilecount="4" columns="4">
 <image source="../Sprites/ProjectileCircle.png" width="88" height="16"/>
 <tile id="0">
  <properties>
   <property name="Animation" value="ProjectileCircle"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="1" width="14" height="14">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="0" duration="200"/>
   <frame tileid="1" duration="200"/>
   <frame tileid="2" duration="200"/>
   <frame tileid="3" duration="200"/>
  </animation>
 </tile>
 <tile id="1">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="0" width="16" height="16">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="0" width="16" height="16">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="0" width="16" height="16">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
</tileset>
