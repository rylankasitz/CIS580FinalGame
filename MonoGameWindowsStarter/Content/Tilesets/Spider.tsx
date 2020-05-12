<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="Spider" tilewidth="72" tileheight="8" tilecount="30" columns="15">
 <image source="../Sprites/SpiderCharacter.png" width="1080" height="16"/>
 <tile id="0">
  <properties>
   <property name="Animation" value="WalkSpider"/>
  </properties>
  <animation>
   <frame tileid="0" duration="100"/>
   <frame tileid="1" duration="100"/>
  </animation>
 </tile>
 <tile id="1">
  <properties>
   <property name="Animation" value="IdleSpider"/>
  </properties>
  <animation>
   <frame tileid="0" duration="1000"/>
  </animation>
 </tile>
 <tile id="15">
  <properties>
   <property name="Animation" value="AttackSpider"/>
  </properties>
  <animation>
   <frame tileid="15" duration="50"/>
   <frame tileid="16" duration="50"/>
   <frame tileid="17" duration="50"/>
   <frame tileid="18" duration="50"/>
   <frame tileid="19" duration="50"/>
   <frame tileid="20" duration="50"/>
   <frame tileid="21" duration="50"/>
   <frame tileid="22" duration="50"/>
   <frame tileid="23" duration="50"/>
   <frame tileid="24" duration="50"/>
  </animation>
 </tile>
 <tile id="19">
  <objectgroup draworder="index" id="2">
   <object id="2" type="Attack" x="38" y="1" width="18" height="1"/>
  </objectgroup>
 </tile>
 <tile id="20">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="38" y="1" width="34" height="1"/>
  </objectgroup>
 </tile>
 <tile id="21">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="38" y="1" width="34" height="2"/>
  </objectgroup>
 </tile>
 <tile id="22">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="38" y="1" width="33" height="3"/>
  </objectgroup>
 </tile>
 <tile id="23">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="38" y="1" width="31" height="6"/>
  </objectgroup>
 </tile>
 <tile id="24">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="39" y="2" width="29" height="6"/>
  </objectgroup>
 </tile>
 <tile id="25">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="39" y="4" width="28" height="4"/>
  </objectgroup>
 </tile>
</tileset>
