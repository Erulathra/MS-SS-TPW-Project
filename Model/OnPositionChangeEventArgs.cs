using System;
using System.Collections.Generic;

namespace TPW.Presentation.Model;

public class OnPositionChangeEventArgs : EventArgs
{
   public readonly IList<IModelBall> Balls;
   public readonly IModelBall SenderBall;

   public OnPositionChangeEventArgs(IModelBall senderBall, IList<IModelBall> balls)
   {
      SenderBall = senderBall;
      Balls = balls;
   }
}