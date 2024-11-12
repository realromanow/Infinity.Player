using Plugins.Infinity.DI.App;
using Plugins.Infinity.DI.Units;
using Plugins.Infinity.Player.App;
using UnityEngine;

namespace Plugins.Infinity.Player.Units {
	[CreateAssetMenu(menuName = "Infinity/Units/Player", fileName = "InfinityPlayerUnit")]
	public class InfinityPlayerUnit : AppUnit {
		public override void SetupUnit (AppComponentsRegistry componentsRegistry) {
			base.SetupUnit(componentsRegistry);

			componentsRegistry.Instantiate<PlayerApi>();
			componentsRegistry.Instantiate<PlayerProvider>();
		}
	}
}
