using Apple.GameKit;
using Apple.GameKit.Leaderboards;
using Plugins.Infinity.Extensions;
using Plugins.Infinity.Player.Api;
using Plugins.Infinity.Player.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugins.Infinity.Player.App {
	public class PlayerApi : IPlayerApi {
		private PlayerDto _player;

		public Task<PlayerDto> GetPlayer (CancellationToken cancellationToken, IProgress<float> progress = null) {
			return _player == null
				? Auth(cancellationToken, progress)
					.ContinueWith(applePlayer
						=> {
						var playerDto = new PlayerDto {
							id = applePlayer.Result.GamePlayerId,
							displayName = applePlayer.Result.DisplayName,
						};

						_player = playerDto;
						return playerDto;
					}, cancellationToken)
				: Task.FromResult(_player);
		}

		public Task<AchievementDto[]> GetAchievements (CancellationToken cancellationToken, IProgress<float> progress = null) {
			// return LoadAchievements(cancellationToken, progress)
			// 	.ContinueWith(appleAchievements
			// 			=> {
			// 			if (appleAchievements.Result == null)
			// 				return new AchievementDto[] {
			// 					new() {
			// 						id = "FirstMatchAchieveID",
			// 						progress = 1f,
			// 						isComplete = true,
			// 					},
			//
			// 					new() {
			// 						id = "WinFirstDuelAchieveID",
			// 						progress = 0f,
			// 						isComplete = false,
			// 					},
			//
			// 					new() {
			// 						id = "Win1000PointsAchieveID",
			// 						progress = 0.45f,
			// 						isComplete = false,
			// 					},
			// 				};
			//
			// 			return appleAchievements.Result
			// 				.Select(x
			// 					=> new AchievementDto {
			// 						id = x.Identifier,
			// 						progress = x.PercentComplete,
			// 						isComplete = x.IsCompleted,
			// 					})
			// 				.ToArray();
			// 		},
			// 		cancellationToken);

			return Task.FromResult(new AchievementDto[] {
				new() {
					id = "FirstMatchAchieveID",
					progress = 0.1f,
					isComplete = true,
				},

				new() {
					id = "WinFirstDuelAchieveID",
					progress = 0.1f,
					isComplete = false,
				},

				new() {
					id = "Win1000PointsAchieveID",
					progress = 0.1f,
					isComplete = false,
				},
			});
		}

		public Task<LeaderboardDto> GetLeaderboard (CancellationToken cancellationToken, IProgress<float> progress = null) {
			// return LoadLeaderboard(cancellationToken, progress)
			// 	.ContinueWith(appleLeaderboard => {
			// 		if (appleLeaderboard.Result == null)
			// 			return new LeaderboardDto {
			// 				players = new LeaderboardPlayerDto[] {
			// 					new() {
			// 						displayName = _player != null ? _player.displayName.Truncate(9) : "My name",
			// 						position = 1,
			// 						score = 0,
			// 					},
			// 				},
			// 			};
			//
			// 		return new LeaderboardDto {
			// 			players = appleLeaderboard.Result
			// 				.Select(entry =>
			// 					new LeaderboardPlayerDto {
			// 						displayName = entry.Player.DisplayName.Truncate(9),
			// 						position = (int)entry.Rank,
			// 						score = (int)entry.Score,
			// 					})
			// 				.ToArray(),
			// 		};
			// 	}, cancellationToken);

			return Task.FromResult(new LeaderboardDto {
				players = new LeaderboardPlayerDto[] {
					new() {
						displayName = _player != null ? _player.displayName.Truncate(9) : "My name",
						position = 1,
						score = 0,
					},
				},
			});
		}

		public void SubmitLeaderboardScore (int score) {
			_ = SubmitLeaderboardScoreAsync(score);
		}

		private async Task SubmitLeaderboardScoreAsync (int score) {
			var player = await Auth(CancellationToken.None);
			var leaderboards = await GKLeaderboard.LoadLeaderboards();
			var leaderboard = leaderboards.First();

			await leaderboard.SubmitScore(score, 0, player);
		}

		private async Task<GKLeaderboard.Entry[]> LoadLeaderboard (CancellationToken cancellationToken, IProgress<float> progress = null) {
			try {
				var leaderboards = await GKLeaderboard.LoadLeaderboards();

				const GKLeaderboard.PlayerScope playerScope = GKLeaderboard.PlayerScope.Global;
				const GKLeaderboard.TimeScope timeScope = GKLeaderboard.TimeScope.AllTime;
				const int rankMin = 1;
				const int rankMax = 10;

				var leaderboard = leaderboards.First();
				var entriesResponse = await leaderboard.LoadEntries(playerScope, timeScope, rankMin, rankMax);
				return entriesResponse.Entries.ToArray();
			}
			catch (GameKitException e) {
				Debug.LogException(e);
				return null;
			}
		}

		private async Task<GKAchievement[]> LoadAchievements (CancellationToken cancellationToken, IProgress<float> progress = null) {
			try {
				var achieves = await GKAchievement.LoadAchievements();
				return achieves.ToArray();
			}
			catch (GameKitException e) {
				Debug.LogException(e);
				return null;
			}
		}

		private async Task<GKPlayer> Auth (CancellationToken cancellationToken, IProgress<float> progress = null) {
			try {
				progress?.Report(0.3f);
				var applePlayer = await GKLocalPlayer.Authenticate();

				return applePlayer;
			}
			catch (GameKitException exception) {
				Debug.LogException(exception);
				return GKLocalPlayer.Local;
			}
			finally {
				progress?.Report(1f);
				await Task.Delay(1000, cancellationToken);
			}
		}
	}
}
