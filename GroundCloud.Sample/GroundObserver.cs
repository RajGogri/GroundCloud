using System;
using System.Collections.Generic;

namespace GroundCloud.Sample
{
    public class GroundObserver : IObserver<Customer>
    {
        private IDisposable subscriber;
        private Customer res;
        public virtual void Subscribe(IObservable<Customer> provider)
        {
            subscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            subscriber.Dispose();
        }
        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.ToString());
        }

        public void OnNext(Customer value)
        {
            res = value;
        }
    }

    public class GroundListObserver : IObserver<IEnumerable<Customer>>
    {
        private IDisposable subscriber;
        private IEnumerable<Customer> res;
        public virtual void Subscribe(IObservable<IEnumerable<Customer>> provider)
        {
            subscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            subscriber.Dispose();
        }
        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.ToString());
        }

        public void OnNext(IEnumerable<Customer> value)
        {
            res = value;
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Phones { get; set; }
        public bool IsActive { get; set; }
    }
}
