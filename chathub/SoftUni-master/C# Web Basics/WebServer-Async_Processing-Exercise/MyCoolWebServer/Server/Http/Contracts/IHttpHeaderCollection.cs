namespace MyCoolWebServer.Server.Http.Contracts
{
    using System.Collections.Generic;

    public interface IHttpHeaderCollection
    {
        void Add(HttpHeader header);

        bool ContainsKey(string key);

        ICollection<HttpHeader> Get(string key);
    }
}
