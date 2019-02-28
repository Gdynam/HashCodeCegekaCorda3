using GoogleHashCode2019;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GoogleHashCode2019.Program;

namespace GoogleHashCode2019
{
    public class Line
    {
        public int Id { get; set; }

        public string Orientation { get; set; }

        public int TagCount { get; set; }

        public List<string> TagList { get; set; }
    }

    public class VerticalLine
    {
        public Line Line1 { get; set; }

        public Line Line2 { get; set; }

        public int TagCount { get; set; }

        public List<string> TagList { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var inputFiles = Directory.GetFiles($"{Environment.CurrentDirectory}\\Input");

            foreach (var file in inputFiles)
            {
                HandleInput(file);
            }

            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }



        private static void HandleInput(string input)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var fileName = Path.GetFileNameWithoutExtension(input);
            Console.WriteLine("-----------------------");
            Console.WriteLine($"Handeling: \t {fileName}");

            var allLines = File.ReadAllLines(input);

            var amountOfPhotos = int.Parse(allLines.First());

            var outputBuilder = new StringBuilder();

            allLines = allLines.Skip(1).ToArray();
            List<Line> photoLines = new List<Line>();
            var count = 0;
            var totalLinesInOutputFile = 0;
            foreach (var inputLine in allLines)
            {
                var argumentsInLine = inputLine.Split(' ');
                var i = 0;

                var position = argumentsInLine[i++];
                var amountOfTags = int.Parse(argumentsInLine[i++]);

                var line = new Line
                {
                    Id = count,
                    Orientation = position,
                    TagCount = amountOfTags,
                    TagList = new List<string>()
                };

                for (int j = 2; j < argumentsInLine.Length; j++)
                {
                    line.TagList.Add(argumentsInLine[j]);
                }
                photoLines.Add(line);
                count++;
            }

            totalLinesInOutputFile = SolutionNiels(photoLines, outputBuilder, totalLinesInOutputFile);


            var result = $"{totalLinesInOutputFile}\n{outputBuilder}";

            WriteResult(result, Path.GetFileName(fileName));

            watch.Stop();
            Console.WriteLine($"Time elapsed: \t {watch.ElapsedMilliseconds}");

            Console.WriteLine("-----------------------");
        }

        private static int SolutionOne(List<Line> photoLines, StringBuilder outputBuilder, int totalLinesInOutputFile)
        {
            var orderByTagCountList = photoLines.OrderBy(x => x.TagCount);


            foreach (var toAppend in orderByTagCountList.Where(x => x.Orientation == "H"))
            {
                outputBuilder.AppendLine($"{toAppend.Id}");
                totalLinesInOutputFile++;
            }

            var even = true;
            foreach (var photoLine in orderByTagCountList.Where(x => x.Orientation.Equals("V")))
            {
                if (even)
                {
                    outputBuilder.Append($"{photoLine.Id}");
                }
                else
                {
                    outputBuilder.Append($" {photoLine.Id}\n");
                    totalLinesInOutputFile++;
                }

                even = !even;
            }

            return totalLinesInOutputFile;
        }

         private static int SuperiorSolution(List<Line> photoLines, StringBuilder outputBuilder, int totalLinesInOutputFile)
        {
            
            var orderByTagCountList = photoLines.OrderBy(x => x.TagCount);            
            var verticalList = orderByTagCountList.Where(x => x.Orientation == "V").ToList();
            var horizontalList = orderByTagCountList.Where(x => x.Orientation == "H").ToList();

            foreach (var toAppend in horizontalList)
            {
                
                outputBuilder.AppendLine($"{toAppend.Id}");
                totalLinesInOutputFile++;
            }

            
 /*           foreach (var photoLine in verticalList)
            {
                if (even)
                {
                    outputBuilder.Append($"{photoLine.Id}");
                }
                else
                {
                    outputBuilder.Append($" {photoLine.Id}\n");
                    totalLinesInOutputFile++;
                }
                even = !even;
            }*/

            var listVerticalAsHorizontal = new List<VerticalLine>();
            while (verticalList.Count() >0)
            {
             var verticalLine = new VerticalLine
             {
                 TagList = new List<string>(),
                 TagCount = 0
             };
             bool first = true;
                foreach (var photoLine in verticalList.Take(2))
                {
                    if (first)
                        verticalLine.Line1 = photoLine;
                    else
                        verticalLine.Line2 = photoLine;

                    verticalLine.TagCount += photoLine.TagCount;
                    verticalLine.TagList.AddRange(photoLine.TagList);

                    first = !first;
                }
            }
    
                    var vertLine = new VerticalLine();
                    outputBuilder.Append($"{vertLine.Line1.Id} {vertLine.Line2.Id}\n");
                    totalLinesInOutputFile++;

            return totalLinesInOutputFile;
        }

        private static void WriteResult(string output, string fileName)
        {
            File.WriteAllText($"Output\\{fileName}.out", output);
            Console.WriteLine($"Done with: \t {fileName}");
        }



        private static int SolutionNiels(List<Line> photoLines, StringBuilder outputBuilder, int totalLinesInOutputFile)
        {
            var orderByTagCountList = photoLines.OrderBy(x => x.TagCount);

            var horizontalList = orderByTagCountList.Where(x => x.Orientation == "H");

            var tagAmountsHorizontal = horizontalList.Select(x => x.TagCount).Distinct();

            foreach (var tagAmount in tagAmountsHorizontal)
            {

                var horizontalListOfTagAmount = horizontalList.Where(x => x.TagCount.Equals(tagAmount)).ToList();


                var nextLine = horizontalListOfTagAmount.First();

                outputBuilder.AppendLine($"{nextLine.Id}");
                totalLinesInOutputFile++;



                while (horizontalListOfTagAmount.Count() > 1)
                {

                    var points = new Dictionary<Line, int>();
                    horizontalListOfTagAmount.Remove(nextLine);

                    foreach (var toCompareLine in horizontalListOfTagAmount.Take(100))
                    {
                        points.Add(toCompareLine, PointCalculator.GetPoint(nextLine.TagList, toCompareLine.TagList));
                    }

                    var maxPoints = points.Values.Max(x => x);

                    var winnerLine = points.First(x => x.Value.Equals(maxPoints)).Key;
                    outputBuilder.AppendLine($"{winnerLine.Id}");
                    totalLinesInOutputFile++;
                    horizontalListOfTagAmount.Remove(winnerLine);

                    nextLine = winnerLine;
                }

            }

            var listVerticalAsHorizontal = new List<VerticalLine>();
            var verticalList = orderByTagCountList.Where(x => x.Orientation == "V").ToList();

          

            while (verticalList.Count() > 0)
            {
                var verticalLine = new VerticalLine
                {
                    TagList = new List<string>(),
                    TagCount = 0
                };
                bool first = true;
                foreach (var photoLine in verticalList.Take(2))
                {
                    if (first)
                        verticalLine.Line1 = photoLine;
                    else
                        verticalLine.Line2 = photoLine;

                    verticalLine.TagCount += photoLine.TagCount;
                    verticalLine.TagList.AddRange(photoLine.TagList);

                    first = !first;

                    
                }

                verticalList.Remove(verticalLine.Line1);
                verticalList.Remove(verticalLine.Line2);
                listVerticalAsHorizontal.Add(verticalLine);
            }

            var tagAmountsVertical = listVerticalAsHorizontal.Select(x => x.TagCount).Distinct().ToList();

            foreach (var tagAmount in tagAmountsVertical)
            {

                var verticalListOfTagAmount = listVerticalAsHorizontal.Where(x => x.TagCount.Equals(tagAmount)).ToList();


                var nextLine = verticalListOfTagAmount.First();

                outputBuilder.AppendLine($"{nextLine.Line1.Id} {nextLine.Line2.Id}");
                verticalListOfTagAmount.Remove(nextLine);
                totalLinesInOutputFile++;



                while (verticalListOfTagAmount.Count() > 1)
                {

                    var points = new Dictionary<VerticalLine, int>();
                    listVerticalAsHorizontal.Remove(nextLine);

                    foreach (var toCompareLine in verticalListOfTagAmount.Take(10))
                    {
                        points.Add(toCompareLine, PointCalculator.GetPoint(nextLine.TagList, toCompareLine.TagList));
                    }
                    var maxPoints = points.Values.Max(x => x);

                    var winnerLine = points.First(x => x.Value.Equals(maxPoints)).Key;
                    outputBuilder.AppendLine($"{winnerLine.Line1.Id} {winnerLine.Line2.Id}");
                    totalLinesInOutputFile++;
                    verticalListOfTagAmount.Remove(winnerLine);

                    nextLine = winnerLine;
                }
            }

            return totalLinesInOutputFile;
        }


    }

}
