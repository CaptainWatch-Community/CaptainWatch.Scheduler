using CaptainWatch.API.ServiceClients;

namespace CaptainWatch.Scheduler.Services.Movies
{
	public class MovieWriteService
	{
		#region Declarations

		private readonly IMoviesWriteHttpClient _moviesWriteHttpClient;

		public MovieWriteService(IMoviesWriteHttpClient moviesWriteHttpClient)
		{
			_moviesWriteHttpClient = moviesWriteHttpClient;
		}

		#endregion

		public async Task UpdateWishCount()
		{
			await _moviesWriteHttpClient.UpdateWishCountAsync();
		}
	}
}
