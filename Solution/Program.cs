using System;
using System.IO;
using System.Collections.Generic;

namespace Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo tasksFile = new FileInfo("tasks.txt");

            if (!tasksFile.Exists) Console.WriteLine("There is no file with tasks in application's directory. Its name must be tasks.txt");
            else
            {
                using (StreamReader streamReader = new StreamReader(tasksFile.OpenRead()))
                {
                    ITaskDataProvider taskDataProvider = new TaskDataFromStreamProvider(streamReader);
                    ISolutionExporter solutionExporter = new SolutionToConsoleExporter();

                    try
                    {
                        SolveTasks(taskDataProvider, solutionExporter);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error occured while solving task: " + e.Message);
                    }
                }
            }

            Console.ReadKey();
        }

        static void SolveTasks(ITaskDataProvider taskDataProvider, ISolutionExporter solutionExporter)
        {
            while (taskDataProvider.HasNextTaskData())
            {
                Tuple<ITransportNetwork, IEnumerable<IRoute>> taskData = taskDataProvider.GetTaskData();

                ITransportNetwork transportNetwork = taskData.Item1;
                IEnumerable<IRoute> routes = taskData.Item2;

                IEnumerable<CollisionAtStationCase> collisionAtStationCases = CollisionsFinder.FindCollisionsAtStations(transportNetwork, routes);
                IEnumerable<CollisionOnConnectionBetweenStationsCase> collisionOnConnectionBetweenStationsCases = CollisionsFinder.FindCollisionsOnConnectionsBetweenStations(transportNetwork, routes);

                solutionExporter.Export(transportNetwork, routes, collisionAtStationCases, collisionOnConnectionBetweenStationsCases);
            }
        }
    }
}
