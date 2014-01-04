/*
 * Classes
 * 	Warrior
 * 		Samurai
 * 		Solider
 * 	Mage
 * 	Rogue
 * 	Cleric
 * 
 * 
 * 
 * Strength
 * Intellec
 * Agility
 * Weapons:
 * 	Durability
	Penetration
	Imbued with elemental qualities
	Weight
	


Attack
Defense

Speed
Dodge
Crit

Fire
Water 
Earth 
Air 

Elemental Modifiers
  Flaming
  Wet
  Muddy
  Windy
  
  Blazing
  Murky
  

Status
	Bleed
	Blind
	Confuse
	Curse
	Poison
	Sleep
	Slow
	Soften
	Stun
	Weaken
	Invigorate



  Color[] tilegraphics(int tileid){
        int numTilesPerRow = terrainTiles.width / tileRes;
        int numRows = terrainTiles.height / tileRes;
        Color[][] tiles = new Color[numTilesPerRow * numRows][];
        for(int b=0; b < numRows; b++) {
        for(int a=0; a < numTilesPerRow; a++){
          tiles[b*numTilesPerRow + a] = terrainTiles.GetPixels( a*tileRes, b*tileRes, tileRes, tileRes);
        }
}
Color[] c = tiles[tileid];
return c;
}

*/