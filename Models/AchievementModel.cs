namespace Infinity.Player.Models {
	public class AchievementModel {
		public string id { get; }
		public float progress { get; }
		public bool isComplete { get; }

		public AchievementModel (string id, float progress, bool isComplete) {
			this.id = id;
			this.progress = progress;
			this.isComplete = isComplete;
		}
	}
}
