using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TPW.Data;

internal class BallListLogger
{
   public static string logFilePath = "%TEMP%/Balls/ballsLog.json";

   private Task? loggingTask;

   private ConcurrentQueue<IBall> ballQueue = new ConcurrentQueue<IBall>();

   private readonly Mutex queueMutex = new Mutex();
   
   public async void AddToLogQueue(IBall ball)
   {
      queueMutex.WaitOne();
      try
      {
         ballQueue.Enqueue(ball);
      }
      finally
      {
         queueMutex.ReleaseMutex();
      }
      
      if (loggingTask != null || loggingTask.IsCompleted.Equals("false"))
         return;

      loggingTask = Task.Run(this.LogToFile);
   }

   private async Task LogToFile()
   {
      //If file doesnt exists create new one.
      JArray array;
      if (File.Exists(logFilePath))
      {
         array = JArray.Parse(File.ReadAllText(logFilePath));
      }
      else
      {
         array = new JArray();
         File.Create(logFilePath);
      }
      
      //Append logs until queue empty
      IBall ball;
      while (ballQueue.TryDequeue(out ball))
      {
         JObject itemToAdd = JObject.FromObject(ball);
         array.Add(itemToAdd);
      }
      
      // Convert data to string and save it
      string output = JsonConvert.SerializeObject(array);
      File.WriteAllText(logFilePath ,output);
   }
  
}