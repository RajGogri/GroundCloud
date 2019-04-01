using System;
using System.Collections.Generic;
using System.Reactive;
using GroundCloud.Contracts;
using GroundCloud.Impl;

namespace GroundCloud.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var testObservable = new CloudObservables();

            var testObserver = new CloudObserver();

            //GET
            testObserver.Subscribe(testObservable.Get<string,Employee>("http://dummy.restapiexample.com/api/v1/employee/5089", 
                null, null, BodySerialization.JSON));

            //POST
            //Employee employee = new Employee();
            //employee.name = "Test Name";
            //employee.salary = "25k";
            //employee.age = "22";

            //testObserver.Subscribe(testObservable.Post<Employee, Employee>("http://dummy.restapiexample.com/api/v1/create", 
                //new KeyValuePair<string, string>("abc", "xyz"), employee, BodySerialization.JSON));

            //PUT

            //Employee employee1 = new Employee();
            //employee1.name = "updated Name";
            //employee1.salary = "20k";
            //employee1.age = "22";

            //testObserver.Subscribe(testObservable.Put<Employee, Employee>("http://dummy.restapiexample.com/api/v1/update/5089",
                    //new KeyValuePair<string, string>("abc", "xyz"), employee1, BodySerialization.JSON));

            //DELETE

            //testObserver.Subscribe(testObservable.Delete<string, Employee>("http://dummy.restapiexample.com/api/v1/delete/5089",
            //new KeyValuePair<string, string>("abc", "xyz"), null, BodySerialization.JSON));



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
    public class CloudObserver : IObserver<Response<Employee>>
    {
        private IDisposable subscriber;
        private bool first = true;
        private Response<Employee> res;

        public virtual void Subscribe(IObservable<Response<Employee>> provider)
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
            Console.WriteLine("OnError Called.");
            if (error.GetType().ToString() == "ArgumentNullException")
            {

            }
            // Do nothing.
        }

        public virtual void OnNext(Response<Employee> value)
        {
            Console.WriteLine("OnNext Called.");
            res = value;
        }
    }
}
