# Scripts Structure

- `Core/`: 매니저/공통 시스템 (`GameManager`, `UIManager`, `SoundManager`, `AdManager` 등)
- `Input/`: 터치/입력 처리
- `Games/`: 게임별 로직
  - `Boom/`, `Card/`, `Coin/`, `Dice/`, `Ladder/`, `Reaction/`, `Roulette/`, `RPS/`, `SpinShot/`, `Timer/`
  - `Common/`: 게임 공통 로직 (`RandomBox`, `CustomInputParser`)
