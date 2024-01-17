# UnityGamingServices Explorer
## Explore the Unity Gaming Services APIs exposed through UIs

### - Setup
- Go to [https://dashboard.unity3d.com](https://dashboard.unity3d.com), select or create an organization, then select or create a project.
- Configure services:
	- **LiveOps**
		- **Authentication**
			- **Identity providers** : Click on the **Add Identity Provider** button and select **Unity Player Account**
		- **Economy**
			- **Configuration** : Add a currency, add an inventory item, add a virtual purchase that cost some currency amount and reward some item amount.
		- **Leaderboards**
			- **Overview** : Add a leaderboard.
	- **Multiplayer**
		- **Friends**
			- **Setup Guide** : Follow the steps until you can toggle on the Friends service
- Install Unity packages:
	- Go to **Windows** > **Package Manager** > Install the following packages via the + > 'Install package by name' menus and buttons
		- com.unity.services.analytics
		- com.unity.services.cloudcode
		- com.unity.services.cloudsave
		- com.unity.services.economy
		- com.unity.services.friends
		- com.unity.services.leaderboards
		- com.unity.services.lobby
		- com.unity.services.playeraccounts
		- com.unity.services.relay
		- com.unity.services.vivox
		- com.unity.multiplayer.playmode
		- com.unity.multiplayer.tools
		- com.unity.netcode.gameobjects
- Go to Project Settings > Services and link your local project with the previously created project on your dashboard, then go to the Vivox service panel and enable the top right toggle.
