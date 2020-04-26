<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="DungeonTileSet" tilewidth="8" tileheight="8" tilecount="120" columns="8">
 <image source="../Sprites/MapTileSet.png" trans="ff00ff" width="64" height="120"/>
 <tile id="1">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="61">
  <properties>
   <property name="Animation" value="AttackBlackGhoul"/>
  </properties>
  <animation>
   <frame tileid="77" duration="100"/>
  </animation>
 </tile>
 <tile id="62">
  <properties>
   <property name="Animation" value="AttackDefault"/>
  </properties>
  <animation>
   <frame tileid="63" duration="200"/>
  </animation>
 </tile>
 <tile id="63">
  <properties>
   <property name="Animation" value="WalkDefault"/>
  </properties>
  <animation>
   <frame tileid="63" duration="200"/>
   <frame tileid="64" duration="200"/>
  </animation>
 </tile>
 <tile id="64">
  <properties>
   <property name="Animation" value="IdleDefault"/>
  </properties>
  <animation>
   <frame tileid="64" duration="1000"/>
  </animation>
 </tile>
 <tile id="71">
  <properties>
   <property name="Animation" value="CharacterPickup"/>
  </properties>
  <animation>
   <frame tileid="71" duration="650"/>
   <frame tileid="70" duration="650"/>
  </animation>
 </tile>
 <tile id="76">
  <properties>
   <property name="Animation" value="IdleBlackGhoul"/>
  </properties>
  <animation>
   <frame tileid="76" duration="1000"/>
  </animation>
 </tile>
 <tile id="77">
  <properties>
   <property name="Animation" value="WalkBlackGhoul"/>
  </properties>
  <animation>
   <frame tileid="76" duration="250"/>
   <frame tileid="77" duration="250"/>
  </animation>
 </tile>
</tileset>
