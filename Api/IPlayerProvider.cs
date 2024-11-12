using Plugins.Infinity.Player.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plugins.Infinity.Player.Api {
	public interface IPlayerProvider {
		Task<PlayerModel> GetPlayer(CancellationToken cancellationToken, IProgress<float> progress = null);

		Task<AchievementModel[]> GetAchievements (CancellationToken cancellationToken, IProgress<float> progress = null);
		
		Task<LeaderboardModel> GetLeaderboard (CancellationToken cancellationToken, IProgress<float> progress = null);

		void SubmitLeaderboardScore (int score);
	}
}
