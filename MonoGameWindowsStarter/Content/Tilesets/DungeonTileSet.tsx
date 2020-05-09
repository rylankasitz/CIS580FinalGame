<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="DungeonTileSet" tilewidth="8" tileheight="8" tilecount="128" columns="8">
 <image source="../Sprites/MapTileSet.png" trans="ff00ff" width="64" height="128"/>
 <tile id="1">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="6" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="6" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index" id="2">
   <object id="2" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="4">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="5">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="11">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="12">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="13">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="14">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="28">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="6" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="29">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="6" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="30">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="6" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="31">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="6" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="32">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="37">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="40">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="41">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="42">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="43">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="49">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="50">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="2"/>
  </objectgroup>
 </tile>
 <tile id="61">
  <properties>
   <property name="Animation" value="AttackBlackGhoul"/>
  </properties>
 </tile>
 <tile id="62">
  <properties>
   <property name="Animation" value="AttackDefault"/>
  </properties>
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
 <tile id="120">
  <properties>
   <property name="Name" value="DoorL"/>
   <property name="Trigger" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="121">
  <properties>
   <property name="Name" value="DoorR"/>
   <property name="Trigger" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="122">
  <properties>
   <property name="Name" value="DoorU"/>
   <property name="Trigger" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="123">
  <properties>
   <property name="Name" value="DoorD"/>
   <property name="Trigger" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
</tileset>
