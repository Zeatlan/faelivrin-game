namespace BattleSystem.Commands
{
    public interface IOrder
    {
        void Execute();

        void Undo();
    }
}