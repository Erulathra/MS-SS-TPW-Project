using System;
using System.Collections.Generic;

namespace TPW.Presentation.Model;

public class OnPositionChangeEventArgs : EventArgs
{
   public readonly IModelBall Ball;

   public OnPositionChangeEventArgs(IModelBall ball)
   {
      Ball = ball;
   }
}