using System;

namespace TPW.Logic;

public class OnPositionChangeEventArgs : EventArgs
{
   public readonly ILogicBall Ball;

   public OnPositionChangeEventArgs(ILogicBall ball)
   {
      this.Ball = ball;
   }
}