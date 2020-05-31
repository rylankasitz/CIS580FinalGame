<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="DungeonTileSet" tilewidth="8" tileheight="8" tilecount="128" columns="8">
 <image source="../Sprites/DungeonTileSet.png" trans="ff00ff" width="64" height="128"/>
 <tile id="1">
  <properties>
   <property name="Name" value="Ceiling-N"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="2">
  <properties>
   <property name="Name" value="Ceiling-H"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="3">
  <properties>
   <property name="Name" value="Ceiling-V"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="4">
  <properties>
   <property name="Name" value="Wall-Basic"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="5">
  <properties>
   <property name="Name" value="Wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="10">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="11">
  <properties>
   <property name="IsLight" type="bool" value="true"/>
   <property name="Name" value="Wall-Light"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="12">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="13">
  <properties>
   <property name="Name" value="Wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="14">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="15">
  <properties>
   <property name="Name" value="Floor"/>
  </properties>
 </tile>
 <tile id="16">
  <properties>
   <property name="Name" value="Floor"/>
  </properties>
 </tile>
 <tile id="17">
  <properties>
   <property name="Name" value="Floor"/>
  </properties>
 </tile>
 <tile id="21">
  <properties>
   <property name="Name" value="Door"/>
  </properties>
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
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="39">
  <properties>
   <property name="Name" value="BlockedDoor"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
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
 <tile id="45">
  <properties>
   <property name="Name" value=""/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="49">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="50">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="53">
  <properties>
   <property name="Name" value=""/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="8"/>
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
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="2" y="7" width="4" height="1"/>
   <object id="2" type="Collision" x="1" y="1" width="7" height="7">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="76" duration="1000"/>
  </animation>
 </tile>
 <tile id="77">
  <properties>
   <property name="Animation" value="WalkBlackGhoul"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="2" y="7" width="4" height="1"/>
   <object id="2" type="Collision" x="1" y="1" width="7" height="7">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="76" duration="250"/>
   <frame tileid="77" duration="250"/>
  </animation>
 </tile>
 <tile id="78">
  <properties>
   <property name="Animation" value="IdleBlackSlime"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="7" width="8" height="1"/>
   <object id="2" type="Collision" x="0" y="1" width="8" height="7">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="78" duration="200"/>
  </animation>
 </tile>
 <tile id="79">
  <properties>
   <property name="Animation" value="WalkBlackSlime"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="0" y="7" width="8" height="1"/>
   <object id="2" type="Collision" x="0" y="1" width="8" height="7">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="78" duration="500"/>
   <frame tileid="79" duration="500"/>
  </animation>
 </tile>
 <tile id="80">
  <properties>
   <property name="Animation" value="IdleBlackEye"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="7" width="6" height="1"/>
   <object id="2" type="Collision" x="1" y="1" width="7" height="7">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="80" duration="1000"/>
  </animation>
 </tile>
 <tile id="81">
  <properties>
   <property name="Animation" value="WalkBlackEye"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="7" width="6" height="1"/>
   <object id="2" type="Collision" x="1" y="1" width="7" height="7">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="80" duration="200"/>
   <frame tileid="81" duration="200"/>
  </animation>
 </tile>
 <tile id="82">
  <properties>
   <property name="Animation" value="AttackBlackGhoul"/>
  </properties>
  <animation>
   <frame tileid="77" duration="500"/>
  </animation>
 </tile>
 <tile id="83">
  <properties>
   <property name="Animation" value="AttackBlackSlime"/>
  </properties>
  <animation>
   <frame tileid="79" duration="1000"/>
  </animation>
 </tile>
 <tile id="84">
  <properties>
   <property name="Animation" value="AttackBlackEye"/>
  </properties>
  <animation>
   <frame tileid="81" duration="400"/>
  </animation>
 </tile>
 <tile id="85">
  <properties>
   <property name="Animation" value="AttackFireSummoner"/>
  </properties>
  <animation>
   <frame tileid="102" duration="1000"/>
  </animation>
 </tile>
 <tile id="101">
  <properties>
   <property name="Animation" value="IdleFireSummoner"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="7" width="5" height="1"/>
   <object id="2" type="Collision" x="0" y="0" width="8" height="8">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="101" duration="1000"/>
  </animation>
 </tile>
 <tile id="102">
  <properties>
   <property name="Animation" value="WalkFireSummoner"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="1" y="7" width="5" height="1"/>
   <object id="2" type="Collision" x="0" y="0" width="8" height="8">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="101" duration="100"/>
   <frame tileid="102" duration="100"/>
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
 <tile id="124">
  <properties>
   <property name="Name" value="KeySpawn"/>
  </properties>
 </tile>
</tileset>
