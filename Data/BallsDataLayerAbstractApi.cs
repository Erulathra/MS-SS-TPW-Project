using System;
using System.Numerics;
using System.Threading;

namespace TPW.Data;

public abstract class BallsDataLayerAbstractApi
{
   protected BallsDataLayerAbstractApi(Vector2 boardSize)
   {
      this.BoardSize = boardSize;
      CancelSimulationSource = new CancellationTokenSource();
   }

   public CancellationTokenSource CancelSimulationSource { get; }

   public Vector2 BoardSize { get; protected set; } 
   public abstract void Add(int howMany);

   public event EventHandler<OnPositionChangeEventArgs>? PositionChange;

   protected void OnPositionChange(OnPositionChangeEventArgs argv)
   {
      PositionChange?.Invoke(this, argv);
   }

   public abstract void StartSimulation();
   public abstract void StopSimulation();

   public static BallsDataLayerAbstractApi? CreateBallsList(Vector2 boardSize)
   {
      return new BallsList(boardSize);
   }

}