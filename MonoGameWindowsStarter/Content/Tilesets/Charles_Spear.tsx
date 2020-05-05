<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.2" name="Charles_Spear" tilewidth="40" tileheight="24" tilecount="18" columns="6">
 <image source="../Sprites/Charles_Spear.png" width="240" height="72"/>
 <tile id="0">
  <properties>
   <property name="Animation" value="AttackCharles"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" type="Attack" x="24" y="11" width="5" height="5"/>
   <object id="3" type="Collision" x="17" y="7" width="7" height="9"/>
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
   <object id="1" type="Attack" x="22.125" y="10" width="17.875" height="5"/>
   <object id="2" type="Collision" x="14" y="7" width="8" height="9"/>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="23" y="10" width="16" height="6"/>
   <object id="2" type="Collision" x="15" y="7" width="8" height="9"/>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Attack" x="23.125" y="11" width="12.125" height="5"/>
   <object id="2" type="Collision" x="15" y="7" width="8" height="9"/>
  </objectgroup>
 </tile>
 <tile id="6">
  <properties>
   <property name="Animation" value="WalkCharles"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="17" y="7" width="7" height="9"/>
  </objectgroup>
  <animation>
   <frame tileid="6" duration="200"/>
   <frame tileid="7" duration="200"/>
  </animation>
 </tile>
 <tile id="7">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="17" y="8" width="7" height="8"/>
  </objectgroup>
 </tile>
 <tile id="12">
  <properties>
   <property name="Animation" value="IdleCharles"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="15" y="7" width="9" height="9"/>
  </objectgroup>
  <animation>
   <frame tileid="12" duration="200"/>
   <frame tileid="13" duration="200"/>
   <frame tileid="14" duration="200"/>
   <frame tileid="15" duration="200"/>
   <frame tileid="16" duration="200"/>
   <frame tileid="17" duration="200"/>
  </animation>
 </tile>
 <tile id="13">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="15" y="7" width="9" height="9"/>
  </objectgroup>
 </tile>
 <tile id="14">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="15" y="7" width="9" height="9"/>
  </objectgroup>
 </tile>
 <tile id="15">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="15" y="7" width="9" height="9"/>
  </objectgroup>
 </tile>
 <tile id="16">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="15" y="7" width="9" height="9"/>
  </objectgroup>
 </tile>
 <tile id="17">
  <objectgroup draworder="index" id="2">
   <object id="1" type="Collision" x="15" y="7" width="9" height="9"/>
  </objectgroup>
 </tile>
</tileset>
