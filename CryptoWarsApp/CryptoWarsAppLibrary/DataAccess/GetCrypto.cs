using CryptoWarsAppLibrary.Models;
using Newtonsoft.Json;

namespace CryptoWarsAppLibrary.DataAccess;

public class GetCrypto
{
    private HttpClient _httpClient;

    public GetCrypto()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<CryptoTable>> GetCryptocurrencyRates(string symbol, DateTime startDate, DateTime endDate)
    {
        List<CryptoTable> rates = new List<CryptoTable>();

        try
        {
            // Format the start and end dates in the required format for the API
            string start = startDate.ToString("yyyy-MM-dd");
            string end = endDate.ToString("yyyy-MM-dd");

            // Construct the API URL with the symbol and date range
            string apiUrl = $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval=1d&startTime={start}&endTime={end}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a list of CryptocurrencyRate objects
            List<List<object>> rawRates = JsonConvert.DeserializeObject<List<List<object>>>(json);

            foreach (List<object> rawRate in rawRates)
            {
                // Extract the relevant data from the raw rate
                double open = Convert.ToDouble(rawRate[1]);
                double close = Convert.ToDouble(rawRate[4]);
                double high = Convert.ToDouble(rawRate[2]);
                double low = Convert.ToDouble(rawRate[3]);
                double volume = Convert.ToDouble(rawRate[5]);

                // Create a CryptocurrencyRate object and add it to the list
                CryptoTable rate = new CryptoTable
                {
                    CryptoName = symbol,
                    CryptoTime = DateTimeOffset.FromUnixTimeMilliseconds((long)rawRate[0]).DateTime,
                    CryptoValue = close,
                };

                rates.Add(rate);
            }

            return rates;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}




