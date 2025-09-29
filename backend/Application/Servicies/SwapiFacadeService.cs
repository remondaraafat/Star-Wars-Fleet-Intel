//using Application.Interfaces;
//using Domain.Models;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Servicies
//{
//    internal class SwapiFacadeService: ISwapiFacadeService
//    {
//        private readonly SwapiClient _client;
//        private readonly ILogger<SwapiFacade> _logger;
//        private readonly IValidator<Starship> _validator;
//        private readonly ObjectPool<ValidationHandler> _validationPool;

//        public SwapiFacade(SwapiClient client, ILogger<SwapiFacade> logger, IValidator<Starship> validator, ObjectPool<ValidationHandler> validationPool)
//        {
//            _client = client ?? throw new ArgumentNullException(nameof(client));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
//            _validationPool = validationPool ?? throw new ArgumentNullException(nameof(validationPool));
//        }

//        public async Task<IEnumerable<Starship>> GetStarshipsAsync(string? search = null, CancellationToken ct = default)
//        {
//            using var scope = _logger.BeginScope(new Dictionary<string, object> { ["RequestId"] = Guid.NewGuid(), ["Search"] = search ?? "none" });
//            _logger.LogInformation("Fetching starships with search: {Search}", search);

//            string endpoint = "starships/";
//            if (!string.IsNullOrEmpty(search)) endpoint += $"?search={Uri.EscapeDataString(search)}";

//            try
//            {
//                var response = await _client.GetAsync<SwapiListResponse<Starship>>(endpoint, ct);
//                var validationHandler = _validationPool.Get();
//                try
//                {
//                    var validated = await validationHandler.HandleAsync(response.Results, ct);
//                    return validated;
//                }
//                finally
//                {
//                    _validationPool.Return(validationHandler);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Failed to fetch starships");
//                throw; // Handled by middleware
//            }
//        }
//    }
//}
