namespace IFire.Framework.Interfaces {

    public interface IConfigProvider {

        TConfig Get<TConfig>() where TConfig : IConfig, new();
    }
}
