using Plugins.Infinity.Player.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plugins.Infinity.Player.Api {
	public interface IPlayerApi {
		Task<PlayerDto> GetPlayer (CancellationToken cancellationToken, IProgress<float> progress = null);
		
		Task<AchievementDto[]> GetAchievements (CancellationToken cancellationToken, IProgress<float> progress = null);
		
		Task<LeaderboardDto> GetLeaderboard (CancellationToken cancellationToken, IProgress<float> progress = null);

		void SubmitLeaderboardScore (int score);
	}
}
