using System;
namespace Soomla
{
		public interface IManifestTools
    {
#if UNITY_EDITOR
			void UpdateManifest();
			void ClearManifest();
#endif
		}
}

