Console.WriteLine("");
// using System.Collections.Immutable;
// using System.Reflection;
// using AXExpansion;
// using UltimateForum.CliClient;
//
// Console.Write("Site root: ");
// UltimateForumApi forum = new(Console.ReadLine() ?? throw new InvalidOperationException("Invalid site root. "));
//
// var boardLevel = async () =>
// {
//     var boards = (await forum.GetAllBoardAsync()).Index().Select(i=>(Index: (int?)i.Index, i.Item)).ToList();
//     ROOT_OF_BOARD_LEVEL:
//     foreach (var valueTuple in boards)
//     {
//         Console.WriteLine("{0}. {1}", valueTuple.Index, valueTuple.Item);
//     }
//
//     var i= Console.ReadLine()??throw new InvalidOperationException("INVALID INPUT");
//     var parsedI = i.TryParseOrDefault<int>();
//     if (boards.FirstOrDefault(i=>i.Index == parsedI).Item is null)
//     {
//         Console.Beep();
//         goto ROOT_OF_BOARD_LEVEL;
//     }
//
// };
//
// Console.WriteLine($"UltimateForum Commandline Client v{Assembly.GetExecutingAssembly().GetName().Version}");
// await boardLevel(); 
// Console.Write("#");
//
