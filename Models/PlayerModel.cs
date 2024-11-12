namespace Plugins.Infinity.Player.Models {
	public class PlayerModel {
		public string displayName { get; }
		public string id { get; }

		public PlayerModel (string id, string displayName) {
			this.displayName = displayName;
			this.id = id;
		}
	}
}
