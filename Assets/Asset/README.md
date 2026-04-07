# Asset Structure

정리 기준:

- `Prefabs/`: 프로젝트에서 바로 사용하는 프리팹
- `Models/`: 코인/주사위 등 게임 오브젝트 모델 팩
- `Audio/`: 효과음/BGM
- `Scenes/`: 게임 씬

현재 주요 이동:

- `Touch.prefab` → `Assets/Asset/Prefabs/`
- 기존 `Assets/Coin`, `Assets/Dice` → `Assets/Asset/Models/`
- 기존 `Assets/Sound` → `Assets/Asset/Audio/Sound`
- 기존 `Assets/Scenes` → `Assets/Asset/Scenes/MainScenes`
