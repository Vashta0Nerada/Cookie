package com.ankamagames.dofus.network.types.game.context.fight
{
   import com.ankamagames.dofus.network.types.game.context.EntityDispositionInformations;
   import com.ankamagames.dofus.network.types.game.look.EntityLook;
   import com.ankamagames.jerakine.network.ICustomDataInput;
   import com.ankamagames.jerakine.network.ICustomDataOutput;
   import com.ankamagames.jerakine.network.INetworkType;
   import com.ankamagames.jerakine.network.utils.FuncTree;
   
   public class GameFightAIInformations extends GameFightFighterInformations implements INetworkType
   {
      
      public static const protocolId:uint = 151;
       
      
      public function GameFightAIInformations()
      {
         super();
      }
      
      override public function getTypeId() : uint
      {
         return 151;
      }
      
      public function initGameFightAIInformations(param1:Number = 0, param2:EntityLook = null, param3:EntityDispositionInformations = null, param4:uint = 2, param5:uint = 0, param6:Boolean = false, param7:GameFightMinimalStats = null, param8:Vector.<uint> = null) : GameFightAIInformations
      {
         super.initGameFightFighterInformations(param1,param2,param3,param4,param5,param6,param7,param8);
         return this;
      }
      
      override public function reset() : void
      {
         super.reset();
      }
      
      override public function serialize(param1:ICustomDataOutput) : void
      {
         this.serializeAs_GameFightAIInformations(param1);
      }
      
      public function serializeAs_GameFightAIInformations(param1:ICustomDataOutput) : void
      {
         super.serializeAs_GameFightFighterInformations(param1);
      }
      
      override public function deserialize(param1:ICustomDataInput) : void
      {
         this.deserializeAs_GameFightAIInformations(param1);
      }
      
      public function deserializeAs_GameFightAIInformations(param1:ICustomDataInput) : void
      {
         super.deserialize(param1);
      }
      
      override public function deserializeAsync(param1:FuncTree) : void
      {
         this.deserializeAsyncAs_GameFightAIInformations(param1);
      }
      
      public function deserializeAsyncAs_GameFightAIInformations(param1:FuncTree) : void
      {
         super.deserializeAsync(param1);
      }
   }
}
