namespace BasketOrderServiceApp.Common;

public interface IHashService {
    Task<string> HashText(string plainText);
}
