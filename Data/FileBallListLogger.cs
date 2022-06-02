using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TPW.Data;

internal class FileBallListLogger : IBallListLogger
{
   private readonly string logFilePath;

   private Task? loggingTask;

   private ConcurrentQueue<IBall> ballQueue = new ConcurrentQueue<IBall>();

   private readonly Mutex queueMutex = new Mutex();

   public FileBallListLogger()
   {
      string tempPath = Path.GetTempPath();
      logFilePath = tempPath + "balls.json";
   }

   public void AddToLogQueue(IBall ball)
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
      
      if (loggingTask != null && !loggingTask.IsCompleted)
         return;

      loggingTask = Task.Factory.StartNew(this.LogToFile);
   }

   private async Task LogToFile()
   {
      //If file doesnt exists create new one.
      JArray array;
      if (File.Exists(logFilePath))
      {
         try
         {
            string input = await File.ReadAllTextAsync(logFilePath);
            array = JArray.Parse(input);
         }
         catch (JsonReaderException)
         {
            array = new JArray();
         }
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
      await File.WriteAllTextAsync(logFilePath ,output);
   }
  
}