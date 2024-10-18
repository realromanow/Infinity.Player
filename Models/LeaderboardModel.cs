namespace Infinity.Player.Models {
	public class LeaderboardModel {
		public LeaderboardPlayerModel[] players { get; }

		public LeaderboardModel (LeaderboardPlayerModel[] players) {
			this.players = players;
		}
	}
}
