using GroundCloud.Contracts;
using System;

namespace GroundCloud.Sample
{
    public class CloudObserver : IObserver<Response<string>>
    {
        private IDisposable subscriber;
        private Response<string> res;

        public virtual void Subscribe(IObservable<Response<string>> provider)
        {
            subscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            subscriber.Dispose();
        }

        public virtual void OnCompleted()
        {
            Console.WriteLine("Completed Successfully.");
        }

        public virtual void OnError(Exception error)
        {
            Console.WriteLine("OnError Called." + error);
            if (error.GetType().ToString() == "ArgumentNullException")
            {

            }
            // Do nothing.
        }

        public virtual void OnNext(Response<string> value)
        {
            Console.WriteLine("OnNext Called." + value.ResponseBody);
            res = value;
        }
    }

    public class Employee
    {
        public string id { get; set; }
        public string employee_name { get; set; }
        public string employee_salary { get; set; }
        public string employee_age { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
        public string age { get; set; }
        public Success success { get; set; }
    }

    public class Success
    {
        public string text { get; set; }
    }
}
