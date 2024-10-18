namespace Infinity.Player.Models {
	public class LeaderboardPlayerModel {
		public string displayName { get; }
		public int score { get; }
		public int position { get; }

		public LeaderboardPlayerModel(
			string displayName,
			int score,
			int position) {
			this.displayName = displayName;
			this.score = score;
			this.position = position;
		}
	}
}
