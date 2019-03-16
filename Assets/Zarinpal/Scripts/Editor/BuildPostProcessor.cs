using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;


public class BuildPostProcessor {

	[PostProcessBuild(1)]
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		if (target == BuildTarget.iOS)
		{
			// Read.
			string projectPath = PBXProject.GetPBXProjectPath(path);
			PBXProject project = new PBXProject();
			project.ReadFromString(File.ReadAllText(projectPath));
			string targetName = PBXProject.GetUnityTargetName();
			string targetGUID = project.TargetGuidByName(targetName);

			project.SetBuildProperty(targetGUID, "ENABLE_BITCODE", "NO");
			project.SetBuildProperty(targetGUID, "SWIFT_VERSION", "4");
			project.SetBuildProperty(targetGUID, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/Plugins/iOS/ZarinpalUnityPlugin-Bridging-Header.h");
			project.SetBuildProperty(targetGUID, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "ZarinpalUnityPlugin-Swift.h");
			project.AddBuildProperty(targetGUID, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");

			string storyboardFilePath = Application.dataPath +
			                    "/Zarinpal/Templates/IOS/PaymentBoard.storyboard";
			var id = project.AddFile(storyboardFilePath,
				"/Libraries/Plugins/IOS/PaymentBoard.storyboard");
			project.AddFileToBuild(targetGUID,id);

			// Write.
			File.WriteAllText(projectPath, project.WriteToString());
		}
	}
}
