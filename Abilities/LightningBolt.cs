﻿using RogueSharp;
using RogueSharpSadConsoleSamples.Core;
using RogueSharpSadConsoleSamples.Interfaces;

namespace RogueSharpSadConsoleSamples.Abilities
{
   public class LightningBolt : Ability, ITargetable
   {
      private readonly int _attack;
      private readonly int _attackChance;

      public LightningBolt( int attack, int attackChance )
      {
         Name = "Lightning Bolt";
         TurnsToRefresh = 40;
         TurnsUntilRefreshed = 0;
         _attack = attack;
         _attackChance = attackChance;
      }

      protected override bool PerformAbility()
      {
         return RogueGame.TargetingSystem.SelectLine( this );
      }

      public void SelectTarget( Point target )
      {
         DungeonMap map = RogueGame.DungeonMap;
         Player player = RogueGame.Player;
         RogueGame.MessageLog.Add( $"{player.Name} casts a {Name}" );

         Actor lightningBoltActor = new Actor {
            Attack = _attack,
            AttackChance = _attackChance,
            Name = Name
         };
         foreach ( Cell cell in map.GetCellsAlongLine( player.X, player.Y, target.X, target.Y ) )
         {
            if ( cell.IsWalkable )
            {
               continue;
            }

            if ( cell.X == player.X && cell.Y == player.Y )
            {
               continue;
            }

            Monster monster = map.GetMonsterAt( cell.X, cell.Y );
            if ( monster != null )
            {
               RogueGame.CommandSystem.Attack( lightningBoltActor, monster );
            }
            else
            {
               // We hit a wall so stop the bolt
               // TODO: consider having bolts and fireballs destroy walls and leave rubble
               return;
            }
         }
      }
   }
}
