using System;
using System.Collections.Generic;

namespace TPW.Data;

public class OnPositionChangeEventArgs : EventArgs
{
   public readonly IList<IBall> Balls;
   public readonly IBall SenderBall;

   public OnPositionChangeEventArgs(IBall senderBall, IList<IBall> balls)
   {
      this.Balls = balls;
      this.SenderBall = senderBall;
   }
}