using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

public class ViewModelBallDecorator : INotifyPropertyChanged
{
   private Vector2 position;
   private float radius;

   public ViewModelBallDecorator(Vector2 position, float radius)
   {
      this.radius = radius;
      Position = position;
   }

   public ViewModelBallDecorator()
   {
      radius = 0;
      Position = Vector2.Zero;
   }

   public Vector2 Position
   {
      get
      {
         return position;
      }
      set
      {
         this.X = value.X;
         this.Y = value.Y;
         this.OnPropertyChanged();
      }
   }

   public float X
   {
      get
      {
         return position.X;
      }
      set
      {
         position.X = value;
         this.OnPropertyChanged();
      }
   }

   public float Y
   {
      get
      {
         return position.Y;
      }
      set
      {
         position.Y = value;
         this.OnPropertyChanged();
      }
   }

   public float Radius
   {
      get
      {
         return radius;
      }
      set
      {
         radius = value;
         this.OnPropertyChanged();
      }
   }

   public event PropertyChangedEventHandler PropertyChanged;

   private void OnPropertyChanged([CallerMemberName] string caller = "")
   {
      var args = new PropertyChangedEventArgs(caller);
      PropertyChanged?.Invoke(this, args);
   }
}