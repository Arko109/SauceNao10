using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SauceNao10.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SauceNao10.Core.Services
{
    public class SauceNaoService
    {
        string _baseUrl = "https://saucenao.com/search.php";
        const int _defaultResultsCount = 16;

        /// <summary>Gets or sets the SauceNao API key.</summary>
        /// <value>The SauceNao API key.</value>
        public string ApiKey { get; set; }

        /// <summary>Gets the sauce for the given image URI asynchronously.</summary>
        /// <param name="uri">The URI of the image.</param>
        /// <param name="resultsCount">The desired results count. If <see cref="ApiKey"/> is not set, the maximum is 16, otherwise it is 32. If the value exceeds the maximum, 6 results are returned.</param>
        /// <returns>A <see cref="IList{Result}"/> containing the results</returns>
        public async Task<IList<Result>> GetSauceAsync(Uri uri, int resultsCount = _defaultResultsCount)
        {
            return _parseResults(JsonConvert.DeserializeObject<JObject>(await (await _baseUrl.SetQueryParams(new
            {
                db = "999",
                output_type = "2",
                api_key = ApiKey ?? "",
                url = uri.AbsoluteUri,
                numres = resultsCount
            }).GetAsync()).Content.ReadAsStringAsync())["results"]);
        }

        /// <summary>Gets the sauce for the given image file stream asynchronously.</summary>
        /// <param name="stream">The stream of the image file.</param>
        /// <param name="name">The file name.</param>
        /// <param name="resultsCount">The desired results count. If <see cref="ApiKey"/> is not set, the maximum is 16, otherwise it is 32. If the value exceeds the maximum, 6 results are returned</param>
        /// <returns>A <see cref="IList{Result}"/> containing the results</returns>
        public async Task<IList<Result>> GetSauceAsync(Stream stream, string name, int resultsCount = _defaultResultsCount)
        {
            return _parseResults(JsonConvert.DeserializeObject<JObject>(await (await _baseUrl.SetQueryParams(new { numres = resultsCount }).PostMultipartAsync(mp => mp.AddStringParts(new
            {
                db = "999",
                output_type = "2",
                api_key = ApiKey ?? ""
            }).AddFile("file", stream, name))).Content.ReadAsStringAsync())["results"]);
        }

        IList<Result> _parseResults(JToken results)
        {
            var rn = new List<Result>();
            foreach (var result in results)
            {
                rn.Add(new Result()
                {
                    DB = Regex.Match(result["header"]["index_name"].ToString(), @".*\d:?(.*) - .*").Groups[1].Value.Trim(),
                    Similarity = double.Parse(result["header"]["similarity"].ToString(), CultureInfo.InvariantCulture),
                    Thumbnail = result["header"]["thumbnail"].ToString(),
                    Author = _getAuthor(result["data"]),
                    Title = _getTitle(result["data"]),
                    Sources = result["data"]["ext_urls"]?.ToObject<List<string>>(),
                    RawData = result
                });
            }

            return rn;
        }

        string _getAuthor(JToken data)
        {
            var rn = data["member_name"] ?? data["author_name"] ?? data["creator"] ?? data["pawoo_user_display_name"] ?? data["author"];
            if (rn is JArray arr)
            {
                rn = arr.First;
            }
            return rn?.ToString();
        }

        string _getTitle(JToken data) => (data["title"] ?? data["eng_name"] ?? data["jp_name"] ?? data["material"] ?? data["source"])?.ToString();
    }
}
