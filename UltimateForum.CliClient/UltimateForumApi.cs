// using System.Net.Http.Json;
// using UltimateForum.Api;
//
// namespace UltimateForum.CliClient;
//
// public class UltimateForumApi
// {
//     private readonly HttpClient _client;
//
//     public UltimateForumApi(string baseAddr)
//     {
//         _client = new HttpClient();
//         _client.BaseAddress = new Uri(baseAddr); 
//     }
//
//     public async Task<IEnumerable<BoardBody>> GetAllBoardAsync()
//     {
//         var res = await _client.GetFromJsonAsync<IEnumerable<BoardBody>>("/api/v1/board/all-board");
//         if (res is null)
//         {
//             throw new NullReferenceException("It shouldn't be null.");
//         }
//         return res; 
//     }
// }