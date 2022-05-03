using System;
using System.Collections.Generic;

namespace TPW.Logic;

public class OnPositionChangeEventArgs : EventArgs
{
   public readonly IList<ILogicBall> Balls;
   public ILogicBall SenderBall;

   public OnPositionChangeEventArgs(ILogicBall senderBall, IList<ILogicBall> balls)
   {
      this.SenderBall = senderBall;
      this.Balls = balls;
   }
}