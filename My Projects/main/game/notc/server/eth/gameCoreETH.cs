// Copyright information can be found in the file named COPYING
// located in the root directory of this distribution.

function GameCoreETH::onMissionLoaded(%game)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::onMissionLoaded");

   $Server::MissionType = "ETH";
   ETH::createTeams(%game);
   Parent::onMissionLoaded(%game);
}

function GameCoreETH::initGameVars(%game)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::initGameVars");

   //-----------------------------------------------------------------------------
   // What kind of "player" is spawned is either controlled directly by the
   // SpawnSphere or it defaults back to the values set here. This also controls
   // which SimGroups to attempt to select the spawn sphere's from by walking down
   // the list of SpawnGroups till it finds a valid spawn object.
   // These override the values set in core/scripts/server/spawn.cs
   //-----------------------------------------------------------------------------
   
   // Leave $Game::defaultPlayerClass and $Game::defaultPlayerDataBlock as empty strings ("")
   // to spawn a the $Game::defaultCameraClass as the control object.
   $Game::defaultPlayerClass = "Etherform";
   $Game::defaultPlayerDataBlock = "FrmEtherform";
   $Game::defaultPlayerSpawnGroups = "PlayerSpawnPoints PlayerDropPoints";

   //-----------------------------------------------------------------------------
   // What kind of "camera" is spawned is either controlled directly by the
   // SpawnSphere or it defaults back to the values set here. This also controls
   // which SimGroups to attempt to select the spawn sphere's from by walking down
   // the list of SpawnGroups till it finds a valid spawn object.
   // These override the values set in core/scripts/server/spawn.cs
   //-----------------------------------------------------------------------------
   $Game::defaultCameraClass = "Camera";
   $Game::defaultCameraDataBlock = "Observer";
   $Game::defaultCameraSpawnGroups = "CameraSpawnPoints PlayerSpawnPoints PlayerDropPoints";

   // Set the gameplay parameters
   %game.duration = 0;
   %game.endgameScore = 0;
   %game.endgamePause = 10;
   %game.allowCycling = true;   // Is mission cycling allowed?
}

function GameCoreETH::startGame(%game)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::startGame");

   Parent::startGame(%game);
   ETH::startNewRound();
}

function GameCoreETH::endGame(%game)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::endGame");

   parent::endGame(%game);
}

function GameCoreETH::onGameDurationEnd(%game)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::onGameDurationEnd");

   parent::onGameDurationEnd(%game);
}

function GameCoreETH::prepareClient(%game, %client)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::prepareClient");

   Parent::prepareClient(%game, %client);
   
   %files = "xa/notc/core/client/audio/Descriptions/v1/exec.cs" TAB
            "xa/notc/core/client/audio/Hearing/v1/exec.cs" TAB
            "xa/notc/core/client/audio/HitSound/v1/exec.cs" TAB
            "xa/notc/core/client/gui/CatGui/v1/exec.cs" TAB
            "xa/notc/core/client/gui/EtherformGui/v1/exec.cs" TAB
            "xa/notc/core/client/gui/ChatHud/v1/exec.cs" TAB
            "xa/notc/core/client/gui/GuiChanger/v1/exec.cs" TAB
            "xa/notc/core/client/gui/LoadoutHud/v1/exec.cs" TAB
            "xa/notc/core/client/gui/MiscHud/v1/exec.cs" TAB
            "xa/notc/core/client/misc/Commands/v1/exec.cs" TAB
            "xa/notc/core/client/postfx/ChromaticLens/v1/exec.cs" TAB
            "xa/notc/deathmatch/client/gui/EndGameGui/v1/exec.cs" TAB
            "xa/notc/deathmatch/client/gui/PlayerList/v1/exec.cs";

   %fieldCount = getFieldCount(%files);
   for(%i = 0; %i < %fieldCount; %i++)
   {
      %file = getField(%files, %i);
      commandToClient(%client, 'ExecContentScript', %file);
   }
}

function GameCoreETH::onClientEnterGame(%game, %client)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::onClientEnterGame");
   
   // Setup loadouts
   %client.zActiveLoadout = 0;
   for(%i = 0; %i < 9; %i++)
   {
      %client.zLoadoutProgress[%i] = 1.0;
      %client.zLoadoutProgressDt[%i] = 0.0;
   }
   
   // Setup loadout HUD
   %client.LoadoutHud_UpdateSlot(0, true,
      "content/xa/notc/core/icons/p1/smg1.32x32.png", 1.0);
   %client.LoadoutHud_UpdateSlot(1, true,
      "content/xa/notc/core/icons/p1/mgl1.32x32.png", 1.0);
   %client.LoadoutHud_UpdateSlot(2, true,
      "content/xa/notc/core/icons/p1/sr1.32x32.png", 1.0);
   %client.LoadoutHud_UpdateSlot(3, true,
      "content/xa/notc/core/icons/p1/mg1.32x32.png", 1.0);
   %client.LoadoutHud_UpdateSlot(4, false);
   %client.LoadoutHud_UpdateSlot(5, false);
   %client.LoadoutHud_SelectSlot(0);

	// Join team with less players.
	if(Game.team1.numPlayers > Game.team2.numPlayers)
   	ETH::joinTeam(%client, 2);
   else
      ETH::joinTeam(%client, 1);

   Parent::onClientEnterGame(%game, %client);
   
   if($Game::Duration)
   {
      %timeLeft = ($Game::StartTime + $Game::Duration) - $Sim::Time;
      commandToClient(%client, 'GameTimer', %timeLeft);
   }
}

function GameCoreETH::onClientLeaveGame(%game, %client)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::onClientLeaveGame");

   parent::onClientLeaveGame(%game, %client);

}

function GameCoreETH::queryClientSettings(%game, %client, %settings)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::queryClientSettings");

   Parent::queryClientSettings(%game, %client, %settings);
   
   commandToClient(%client, 'XaNotcSettings1_Query', "PlayerColor0");
   commandToClient(%client, 'XaNotcSettings1_Query', "PlayerColor1");
}

function GameCoreETH::processClientSettingsReply(%game, %client, %setting, %value)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::processClientSettingsReply");
   
   %status = "Ignored";

   if(%setting $= "PlayerColor0")
   {
      if(isValidPlayerColor(%value))
      {
         %client.paletteColors[0] = %value SPC "255";
         %status = "Ok";
      }
      else
         %status = "Invalid";

   }
   else if(%setting $= "PlayerColor1")
   {
      if(isValidPlayerColor(%value))
      {
         %client.paletteColors[1] = %value SPC "255";
         %status = "Ok";
      }
      else
         %status = "Invalid";
   }

   commandToClient(%client, 'XaNotcSettings1_Confirmation', %setting, %status);
}

function GameCoreETH::loadOut(%game, %player)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::loadOut");
   
   Parent::loadOut(%game, %player);
   
   %team = %player.client.team;
   %player.setTeamId(%team.teamId);
   %teamColorF = %team.color;
   %teamColorI = getWord(%teamColorF, 0)*255 SPC
                 getWord(%teamColorF, 1)*255 SPC
                 getWord(%teamColorF, 2)*255 SPC
                 255;

   %player.paletteColors[0] = %teamColorI;
   %player.paletteColors[1] = %teamColorI;
   
   if(isObject(%player.light))
   {
      %colorI = %player.paletteColors[0];
      %colorF = getWord(%colorI, 0) / 255 SPC
                getWord(%colorI, 1) / 255 SPC
                getWord(%colorI, 2) / 255 SPC
                1;
      %player.light.color = %colorF;
   }

   return;

   %player.clearWeaponCycle();
   %player.addToWeaponCycle(WpnSMG1);
   %player.addToWeaponCycle(WpnMGL1);
   %player.addToWeaponCycle(WpnSG1);
   %player.addToWeaponCycle(WpnSR1);
   %player.addToWeaponCycle(WpnMG1);
   //%player.addToWeaponCycle(WpnML1);

   %player.setInventory(ItemImpShield, 1);
   %player.setInventory(ItemEtherboard, 1);
   %player.setInventory(ItemLauncher, 1);
   
   %player.setInventory(WpnSMG1, 1);
   %player.setInventory(WpnMGL1, 1);
   %player.setInventory(WpnMGL1Ammo, 9999);

   if (%player.getDatablock().mainWeapon.image !$= "")
      %player.mountImage(%player.getDatablock().mainWeapon.image, 0);
   else
      %player.mountImage(WpnBadgerImage, 0);
}

function GameCoreETH::onUnitDestroyed(%game, %obj)
{
   //echo(%game @"\c4 -> "@ %game.class @" -> GameCoreETH::onUnitDestroyed");
   
   Parent::onUnitDestroyed(%game, %obj);
   
   %client = %obj.client;
   if(isObject(%client) && %client.player == %obj)
   {
      ETH::switchToEtherform(%client);
   }
}

function GameCoreETH::clientAction(%game, %client, %nr)
{
   echo(%game @"\c4 -> "@ %game.class @" -> GameCoreETH::clientAction");

   %obj = %client.getControlObject();
   if(!isObject(%obj))
      return;

   %obj.getDataBlock().clientAction(%obj, %nr);
}

function GameCoreETH::etherformManifest(%game, %obj)
{
   echo(%game @"\c4 -> "@ %game.class @" -> GameCoreETH::etherformManifest");
   
   %client = %obj.client;
   
   if(!isObject(%client))
      return;
      
   %percent = %client.zLoadoutProgress[%client.zActiveLoadout];

   if(%percent < 0.5)
   {
      error("Class needs at least 50% health to manifest!");
      return;
   }

   if(%client.player.getEnergyLevel() < 50)
   {
      error("You need at least 50% energy to manifest!");
      return;
   }

   %ownTeamId = %client.player.getTeamId();

   %inOwnZone = false;
   %inOwnTerritory = false;
   %inEnemyZone = false;

   %pos = %obj.getPosition();
   InitContainerRadiusSearch(%pos, 0.0001, $TypeMasks::TacticalZoneObjectType);
   while((%srchObj = containerSearchNext()) != 0)
   {
      // object actually in this zone?
      %inSrchZone = false;
      for(%i = 0; %i < %srchObj.getNumObjects(); %i++)
      {
         if(%srchObj.getObject(%i) == %client.player)
         {
            %inSrchZone = true;
            break;
         }
      }
      if(!%inSrchZone)
         continue;

      %zoneTeamId = %srchObj.getTeamId();
      %zoneBlocked = %srchObj.zBlocked;

      if(%zoneTeamId != %ownTeamId && %zoneTeamId != 0)
      {
         %inEnemyZone = true;
         break;
      }
      else if(%zoneTeamId == %ownTeamId)
      {
         %inOwnZone = true;
         if(%srchObj.getDataBlock().getName() $= "TerritoryZone"
         || %srchObj.getDataBlock().isTerritoryZone)
            %inOwnTerritory = true;
      }
   }

   if(%inEnemyZone)
   {
      error("You can not manifest in an enemy zone!");
      return;
   }
   else if(%inOwnZone && !%inOwnTerritory)
   {
      error("This is not a territory zone!");
      return;
   }
   else if(!%inOwnZone)
   {
      error("You can only manifest in your team's territory zones!");
      return;
   }
   else if(%zoneBlocked)
   {
      error("This zone is currently blocked!");
      return;
   }
   
   %data = FrmStandardcat;
   switch(%client.zActiveLoadout)
   {
      case 2:
         %data = FrmSnipercat;
   }

   %player = new Player() {
      dataBlock = %data;
      client = %client;
      teamId = %client.team.teamId;
      isCAT = true;
   };
   MissionCleanup.add(%player);
   copyPalette(%obj, %player);

   %player.setInventory(ItemVAMP, 1);
   %player.setInventory(ItemImpShield, 1);
   %player.setInventory(ItemLauncher, 1);
   %player.setInventory(ItemBounce, 1);

   %player.clearWeaponCycle();
   switch(%client.zActiveLoadout)
   {
      case 0:
         %player.setInventory(ItemEtherboard, 1);
         %player.setInventory(WpnSMG1, 1);
         %player.setInventory(WpnSG1, 1);
         %player.setInventory(WpnSG1Ammo, 9999);
         %player.addToWeaponCycle(WpnSMG1);
         %player.addToWeaponCycle(WpnSG1);
         %player.mountImage(WpnSMG1Image, 0);
      case 1:
         %player.setInventory(ItemEtherboard, 1);
         %player.setInventory(WpnMGL1, 1);
         %player.setInventory(WpnMGL1Ammo, 9999);
         %player.setInventory(WpnSG2, 1);
         %player.setInventory(WpnSG2Ammo, 9999);
         %player.addToWeaponCycle(WpnMGL1);
         %player.addToWeaponCycle(WpnSG2);
         %player.mountImage(WpnMGL1Image, 0);
      case 2:
         %player.setInventory(WpnSR1, 1);
         %player.setInventory(WpnSR1Ammo, 9999);
         %player.addToWeaponCycle(WpnSR1);
         %player.addToWeaponCycle(WpnMG1);
         %player.mountImage(WpnSR1Image, 0);
      case 3:
         %player.setInventory(ItemEtherboard, 1);
         %player.setInventory(WpnMG1, 1);
         %player.setInventory(WpnMG1Ammo, 9999);
         %player.addToWeaponCycle(WpnSR1);
         %player.addToWeaponCycle(WpnMG1);
         %player.mountImage(WpnMG1Image, 0);
   }

   %mat = %obj.getTransform();
   %dmg = %obj.getDamageLevel();
   %nrg = %obj.getEnergyLevel();
   %buf = %obj.getDamageBufferLevel();
   %vel = %obj.getVelocity();

   %player.setTransform(%mat);
   %player.setTransform(%pos);
   %player.setDamageLevel(%player.getDataBlock().maxDamage * (1-%percent));
   //%player.setShieldLevel(%buf);

	//if(%tagged || $Server::Game.tagMode == $Server::Game.alwaystag)
	//	%player.setTagged();

   %client.control(%player);

   // Remove any z-velocity.
   %vel = getWord(%vel, 0) SPC getWord(%vel, 1) SPC "0";

   %player.setEnergyLevel(%nrg);
   %player.setVelocity(VectorScale(%vel, 0.25));

   %player.startFade(1000,0,false);
   %player.playAudio(0, CatSpawnSound);

   %client.player.schedule(9, "delete");
	%client.player = %player;
}

function GameCoreETH::suicide(%game, %client)
{
   //echo (%game @"\c4 -> "@ %game.class @" -> GameCoreETH::suicide");
   ETH::switchToEtherform(%client);
}

