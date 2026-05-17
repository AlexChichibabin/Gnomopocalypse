using System;

public interface IBattleTurnController
{
    BattleTurnPhase CurrentPhase { get; }

    event Action<BattleTurnPhase> PhaseChanged;

    void ChangeTurn();
    void ChangeTurn(BattleTurnPhase turn);
}
