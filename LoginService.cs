namespace PoC_CircuitBreaker
{
    public interface ILoginService
    {
        Task LoginAsync();
    }
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task LoginAsync()
        {
            string url = "http://localhost:8086/login";


            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Login bem-sucedido!");
                }
                else
                {
                    Console.WriteLine($"Falha ao fazer login. Código de status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro ao fazer login: {e.Message}");
            }
        }
    }
}
