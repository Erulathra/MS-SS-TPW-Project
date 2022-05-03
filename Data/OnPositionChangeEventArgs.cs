using System;

namespace TPW.Data;

public class OnPositionChangeEventArgs : EventArgs
{
   public readonly IBall Ball;

   public OnPositionChangeEventArgs(IBall ball)
   {
      this.Ball = ball;
   }
}