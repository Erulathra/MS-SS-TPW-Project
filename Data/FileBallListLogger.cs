using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TPW.Data;

internal class FileBallListLogger : IBallListLogger
{
   private readonly string logFilePath;

   private Task? loggingTask;

   private readonly ConcurrentQueue<JObject> ballQueue = new ConcurrentQueue<JObject>();

   private readonly Mutex queueMutex = new Mutex();
   private readonly JArray fileDataArray;

   public FileBallListLogger()
   {
      string tempPath = Path.GetTempPath();
      logFilePath = tempPath + "balls.json";
      
      //If file doesnt exists create new one.
      if (File.Exists(logFilePath))
      {
         try
         {
            string input = File.ReadAllText(logFilePath);
            fileDataArray = JArray.Parse(input);
            return;
         }
         catch (JsonReaderException)
         {
            fileDataArray = new JArray();
         }
      }

      fileDataArray = new JArray();
      File.Create(logFilePath);
   }

   public void AddToLogQueue(IBall ball)
   {
      queueMutex.WaitOne();
      try
      {
         JObject itemToAdd = JObject.FromObject(ball);
         ballQueue.Enqueue(itemToAdd);


         if (loggingTask == null || loggingTask.IsCompleted)
         {
            loggingTask = Task.Factory.StartNew(this.LogToFile);
         }
      }
      finally
      {
         queueMutex.ReleaseMutex();
      }
   }

   private Mutex fileMutex = new Mutex();
   
   private async Task LogToFile()
   {
      //Append logs until queue empty
      while (ballQueue.TryDequeue(out JObject ball))
      {
         fileDataArray.Add(ball);
      }
      
      // Convert data to string and save it
      string output = JsonConvert.SerializeObject(fileDataArray);
      
      fileMutex.WaitOne();
      try
      {
         await File.WriteAllTextAsync(logFilePath, output);
      }
      finally
      {
         fileMutex.ReleaseMutex();
      }
   }

   ~FileBallListLogger()
   {
      fileMutex.WaitOne();
      fileMutex.ReleaseMutex();
   }
}