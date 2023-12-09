
using System.Xml.Linq;

namespace Day08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                LR

                BNR = (CTN, QNM)
                QNM = (CSB, ZZZ)
                ZZZ = (ZZZ, ZZZ)
             */

            var lines = File.ReadAllLines("input.txt");
            var steps = lines[0];
            var nodes = lines.Skip(2).Select(Node.Parse).ToList();
            var nodeDict = nodes.ToDictionary(x => x.Name);

            Part1(steps, nodeDict);
            Part2(steps, nodeDict);
        }

        private static void Part1(string steps, Dictionary<string, Node> nodeDict)
        {
            var node = nodeDict["AAA"];
            var stepIdx = 0;
            var stepsTaken = 0;
            while (node.Name != "ZZZ")
            {
                node = steps[stepIdx] switch
                {
                    'L' => nodeDict[node.Left],
                    _ => nodeDict[node.Right]
                };

                stepsTaken++;
                stepIdx = (stepIdx + 1) % steps.Length;
            }

            Console.WriteLine($"Part 1: {stepsTaken}");
        }

        private static void Part2(string steps, Dictionary<string, Node> nodeDict)
        {
            var nodes = nodeDict.Values.Where(x => x.Name[2] == 'A').ToList();
            var stepsUntilZForNodes = nodes.Select(x => StepsUntilZ(steps, x, nodeDict)).ToList();
            var minSteps = stepsUntilZForNodes.Min();

            var stepsTaken = (long) minSteps;
            while (stepsUntilZForNodes.Any(x => stepsTaken % x != 0))
            {
                stepsTaken += minSteps;
            }

            Console.WriteLine($"Part 2: {stepsTaken}");
        }
        
        private static int StepsUntilZ(string steps,Node node, Dictionary<string, Node> nodeDict)
        {
            var stepIdx = 0;
            var stepsTaken = 0;
            while (node.Name[2] != 'Z')
            {
                node = steps[stepIdx] switch
                {
                    'L' => nodeDict[node.Left],
                    _ => nodeDict[node.Right]
                };

                stepsTaken++;
                stepIdx = (stepIdx + 1) % steps.Length;
            }

            return stepsTaken;
        }

        record Node(string Name, string Left, string Right)
        {
            public static Node Parse(string s)
            {
                // BNR = (CTN, QNM)
                var name = s.Substring(0, 3);
                var left = s.Substring(7, 3);
                var right = s.Substring(12, 3);
                return new Node(name, left, right);
            }
        };
    }
}
