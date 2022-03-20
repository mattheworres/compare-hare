using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Io;

//This totally faked class is merely used to provide a mockable empty interface
//in order to "mock" a request that AngleSharp then uses to consume a mocked document

namespace CompareHare.Tests.Domain.Services
{
    public class FakeRequester : IRequester
    {
        public event DomEventHandler Requesting;
        public event DomEventHandler Requested;

        public void AddEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new System.NotImplementedException();
        }

        public bool Dispatch(Event ev)
        {
            throw new System.NotImplementedException();
        }

        public void InvokeEventListener(Event ev)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<IResponse> RequestAsync(Request request, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool SupportsProtocol(string protocol)
        {
            throw new System.NotImplementedException();
        }
    }
}