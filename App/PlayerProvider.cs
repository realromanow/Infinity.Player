using Plugins.Infinity.Player.Api;
using Plugins.Infinity.Player.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plugins.Infinity.Player.App {
	public class PlayerProvider : IPlayerProvider {
		private readonly IPlayerApi _playerApi;

		private PlayerModel _playerModel;

		public PlayerProvider (IPlayerApi playerApi) {
			_playerApi = playerApi;
		}

		public Task<PlayerModel> GetPlayer (CancellationToken cancellationToken, IProgress<float> progress = null) {
			if (_playerModel == null)
				return _playerApi.GetPlayer(cancellationToken, progress)
					.ContinueWith(playerDto => {
						_playerModel = new PlayerModel(playerDto.Result.id, playerDto.Result.displayName);
						return _playerModel;
					}, cancellationToken);

			return Task.FromResult(_playerModel);
		}

		public Task<AchievementModel[]> GetAchievements (CancellationToken cancellationToken, IProgress<float> progress = null) {
			return _playerApi.GetAchievements(cancellationToken, progress)
				.ContinueWith(achievesDto => achievesDto.Result.Select(dto => new AchievementModel(dto.id, dto.progress, dto.isComplete)).ToArray(), cancellationToken);
		}

		public Task<LeaderboardModel> GetLeaderboard (CancellationToken cancellationToken, IProgress<float> progress = null) {
			return _playerApi.GetLeaderboard(cancellationToken, progress)
				.ContinueWith(leaderboardDto => {
					var leaderboardPlayers =
						leaderboardDto.Result.players
							.Select(player =>
								new LeaderboardPlayerModel(
									player.displayName,
									player.score,
									player.position))
							.ToArray();

					return new LeaderboardModel(leaderboardPlayers);
				}, cancellationToken);
		}

		public void SubmitLeaderboardScore (int score) {
			_playerApi.SubmitLeaderboardScore(score);
		}
	}
}
