using System;
using System.IO;
using System.Collections.Generic;

namespace Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            //Информация о файле с заданиями.
            FileInfo tasksFile = new FileInfo("tasks.txt");

            //Если файла не существует, то сообщить об ошибке.
            if (!tasksFile.Exists) Console.WriteLine("There is no file with tasks in application's directory. Its name must be tasks.txt");
            else
            {
                //В другом случае создать StreamReader.
                using (StreamReader streamReader = new StreamReader(tasksFile.OpenRead()))
                {
                    //Создать экземпляр TaskDataFromStreamProvider.
                    ITaskDataProvider taskDataProvider = new TaskDataFromStreamProvider(streamReader);
                    //Создать экземпляр SolutionToConsoleExporter.
                    ISolutionExporter solutionExporter = new SolutionToConsoleExporter();

                    try
                    {
                        //Попытаться решить задачу. При решении могут возникнуть ошибки, если, например, были указаны невозможные маршруты
                        //или указаны петли в транспортной сети, отрицательные расстояния между станциями и т.д.
                        SolveTasks(taskDataProvider, solutionExporter);
                    }
                    catch (Exception e)
                    {
                        //Если при решении задачи возникли ошибки, то сообщить о них.
                        Console.WriteLine("Error occured while solving task: " + e.Message);
                    }
                }
            }

            //Пользователь может смотреть результаты решения до тех пор, пока не нажмет какую-либо клавишу.
            Console.ReadKey();
        }

        //Используя переданные ITaskDataProvider, ISolutionExporter, решить задачу и вывести результат.
        static void SolveTasks(ITaskDataProvider taskDataProvider, ISolutionExporter solutionExporter)
        {
            //Пока в ITaskDataProvider есть входные данные
            while (taskDataProvider.HasNextTaskData())
            {
                //Получить входные данные.
                Tuple<ITransportNetwork, IEnumerable<IRoute>> taskData = taskDataProvider.GetTaskData();

                ITransportNetwork transportNetwork = taskData.Item1;
                IEnumerable<IRoute> routes = taskData.Item2;

                //Найти столкновения на станциях.
                IEnumerable<CollisionAtStationCase> collisionAtStationCases = CollisionsFinder.FindCollisionsAtStations(transportNetwork, routes);
                //Найти столкновения на путях.
                IEnumerable<CollisionOnConnectionBetweenStationsCase> collisionOnConnectionBetweenStationsCases = CollisionsFinder.FindCollisionsOnConnectionsBetweenStations(transportNetwork, routes);

                //Передать результаты решения ISolutionExporter.
                solutionExporter.Export(transportNetwork, routes, collisionAtStationCases, collisionOnConnectionBetweenStationsCases);
            }
        }
    }
}
