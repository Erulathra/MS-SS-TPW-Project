using System;
using System.Collections.Generic;

namespace TPW.Logic;

public class OnPositionChangeEventArgs : EventArgs
{
   public ILogicBall Ball;

   public OnPositionChangeEventArgs(ILogicBall ball)
   {
      this.Ball = ball;
   }
}