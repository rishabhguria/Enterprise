namespace Prana.Interfaces
{
    public interface ILiveFeedFactory
    {

        Prana.Interfaces.IDepthServer DepthServer
        {
            get;
        }

        Prana.Interfaces.ILevelOneServer LevelOneServer
        {
            get;
        }

        Prana.Interfaces.INewsServer NewsServer
        {
            get;
        }
    }
}
