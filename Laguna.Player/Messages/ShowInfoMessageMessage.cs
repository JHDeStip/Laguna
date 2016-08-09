namespace JhDeStip.Laguna.Player.Messages
{
    public class ShowInfoMessageMessage
    {
        public string Message { get; private set; }

        public ShowInfoMessageMessage() { }

        public ShowInfoMessageMessage(string infoMessage)
        {
            Message = infoMessage;
        }
    }
}
