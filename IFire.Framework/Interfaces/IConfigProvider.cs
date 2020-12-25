namespace IFire.Framework.Interfaces {

    public interface IConfigProvider {

        TConfig Get<TConfig>(string section = "") where TConfig : IConfig, new();
    }
}
