using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using KLGproject.Repositories.AppObjects;

namespace KLGproject.Resources.Libraries.API.Lib
{
    public class API
    {

        //GET https://www.freetogame.com/api/games
        //GET https://www.freetogame.com/api/games?platform=pc
        //GET https://www.freetogame.com/api/games?category=shooter
        //GET https://www.freetogame.com/api/games?sort-by=alphabetical
        //GET https://www.freetogame.com/api/games?platform=browser&category=mmorpg&sort-by=release-date
        //GET https://www.freetogame.com/api/filter?tag=3d.mmorpg.fantasy.pvp&platform=pc
        //GET https://www.freetogame.com/api/game?id=452

        /**
         * Categories for Games 
         * mmorpg, shooter, strategy, moba, racing, sports, social, sandbox, open-world, survival, pvp, pve, pixel, voxel, zombie, turn-based, first-person, third-Person, top-down, tank, space, sailing, side-scroller, superhero, permadeath, card, battle-royale, mmo, mmofps, mmotps, 3d, 2d, anime, fantasy, sci-fi, fighting, action-rpg, action, military, martial-arts, flight, low-spec, tower-defense, horror, mmorts
         */

        //short-by: release-date, popularity, alphabetical or relevance

        private const string BaseUrl = "https://www.freetogame.com/api";

        private readonly HttpClient _httpClient;

        /// <summary>
        /// API Constructor
        /// </summary>
        public API()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// List the games in an async task
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Game>> GetGames()
        {
            string url = $"{BaseUrl}/games";
            try
            {
                List<Game> games = await GetJsonAsync<List<Game>>(url);

                return games;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// List the games by platform in asyn task
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public async Task<List<Game>> GetGamesByPlatform(string platform)
        {
            string url = $"{BaseUrl}/games?platform={platform}";
            return await GetJsonAsync<List<Game>>(url);
        }

        /// <summary>
        /// list the games by category in async task
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<List<Game>> GetGamesByCategory(string category)
        {
            string url = $"{BaseUrl}/games?category={category}";
            return await GetJsonAsync<List<Game>>(url);
        }

        /// <summary>
        /// List the games sorted alphabetically in an async task
        /// </summary>
        /// <returns></returns>
        public async Task<List<Game>> GetGamesSortedAlphabetically()
        {
            string url = $"{BaseUrl}/games?sort-by=alphabetical";
            return await GetJsonAsync<List<Game>>(url);
        }

        /// <summary>
        /// List the games by the platform and catefory in an async task
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<List<Game>> GetGamesByPlatformAndCategory(string platform, string category)
        {
            string url = $"{BaseUrl}/games?platform={platform}&category={category}&sort-by=release-date";
            return await GetJsonAsync<List<Game>>(url);
        }

        /// <summary>
        /// List the games by tag filter and platform
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public async Task<List<Game>> GetFilteredGames(string tag, string platform)
        {
            string url = $"{BaseUrl}/filter?tag={tag}&platform={platform}";
            return await GetJsonAsync<List<Game>>(url);
        }

        /// <summary>
        /// Get the game by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Game> GetGameById(int id)
        {
            string url = $"{BaseUrl}/game?id={id}";
            return await GetJsonAsync<Game>(url);
        }

        /// <summary>
        /// Deserialize the JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<T> GetJsonAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(responseStream);
        }


    }
}
