package com.ankamagames.dofus.network.messages.game.actions.fight
{
   import com.ankamagames.dofus.network.messages.game.actions.AbstractGameActionMessage;
   import com.ankamagames.jerakine.network.CustomDataWrapper;
   import com.ankamagames.jerakine.network.ICustomDataInput;
   import com.ankamagames.jerakine.network.ICustomDataOutput;
   import com.ankamagames.jerakine.network.INetworkMessage;
   import com.ankamagames.jerakine.network.utils.FuncTree;
   import flash.utils.ByteArray;
   
   [Trusted]
   public class GameActionFightInvisibilityMessage extends AbstractGameActionMessage implements INetworkMessage
   {
      
      public static const protocolId:uint = 5821;
       
      
      private var _isInitialized:Boolean = false;
      
      public var targetId:Number = 0;
      
      public var state:uint = 0;
      
      public function GameActionFightInvisibilityMessage()
      {
         super();
      }
      
      override public function get isInitialized() : Boolean
      {
         return super.isInitialized && this._isInitialized;
      }
      
      override public function getMessageId() : uint
      {
         return 5821;
      }
      
      public function initGameActionFightInvisibilityMessage(param1:uint = 0, param2:Number = 0, param3:Number = 0, param4:uint = 0) : GameActionFightInvisibilityMessage
      {
         super.initAbstractGameActionMessage(param1,param2);
         this.targetId = param3;
         this.state = param4;
         this._isInitialized = true;
         return this;
      }
      
      override public function reset() : void
      {
         super.reset();
         this.targetId = 0;
         this.state = 0;
         this._isInitialized = false;
      }
      
      override public function pack(param1:ICustomDataOutput) : void
      {
         var _loc2_:ByteArray = new ByteArray();
         this.serialize(new CustomDataWrapper(_loc2_));
         writePacket(param1,this.getMessageId(),_loc2_);
      }
      
      override public function unpack(param1:ICustomDataInput, param2:uint) : void
      {
         this.deserialize(param1);
      }
      
      override public function unpackAsync(param1:ICustomDataInput, param2:uint) : FuncTree
      {
         var _loc3_:FuncTree = new FuncTree();
         _loc3_.setRoot(param1);
         this.deserializeAsync(_loc3_);
         return _loc3_;
      }
      
      override public function serialize(param1:ICustomDataOutput) : void
      {
         this.serializeAs_GameActionFightInvisibilityMessage(param1);
      }
      
      public function serializeAs_GameActionFightInvisibilityMessage(param1:ICustomDataOutput) : void
      {
         super.serializeAs_AbstractGameActionMessage(param1);
         if(this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
         {
            throw new Error("Forbidden value (" + this.targetId + ") on element targetId.");
         }
         param1.writeDouble(this.targetId);
         param1.writeByte(this.state);
      }
      
      override public function deserialize(param1:ICustomDataInput) : void
      {
         this.deserializeAs_GameActionFightInvisibilityMessage(param1);
      }
      
      public function deserializeAs_GameActionFightInvisibilityMessage(param1:ICustomDataInput) : void
      {
         super.deserialize(param1);
         this._targetIdFunc(param1);
         this._stateFunc(param1);
      }
      
      override public function deserializeAsync(param1:FuncTree) : void
      {
         this.deserializeAsyncAs_GameActionFightInvisibilityMessage(param1);
      }
      
      public function deserializeAsyncAs_GameActionFightInvisibilityMessage(param1:FuncTree) : void
      {
         super.deserializeAsync(param1);
         param1.addChild(this._targetIdFunc);
         param1.addChild(this._stateFunc);
      }
      
      private function _targetIdFunc(param1:ICustomDataInput) : void
      {
         this.targetId = param1.readDouble();
         if(this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
         {
            throw new Error("Forbidden value (" + this.targetId + ") on element of GameActionFightInvisibilityMessage.targetId.");
         }
      }
      
      private function _stateFunc(param1:ICustomDataInput) : void
      {
         this.state = param1.readByte();
         if(this.state < 0)
         {
            throw new Error("Forbidden value (" + this.state + ") on element of GameActionFightInvisibilityMessage.state.");
         }
      }
   }
}
