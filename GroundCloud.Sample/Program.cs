using System;
using GroundCloud.Contracts;
using GroundCloud.Impl;

namespace GroundCloud.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Program p = new Program();
            //p.TestCloudObserver();
            p.TestGroundObserver();
        }

        public void TestCloudObserver()
        {
            var testObservable = new CloudObservables();

            var testObserver = new CloudObserver();

            //GET
            testObserver.Subscribe(testObservable.Get<Request, Employee>("http://dummy.restapiexample.com/api/v1/employee/5089",
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

        public void TestGroundObserver()
        {
            var testGroundObserver = new GroundObserver();
            var testGObserver = new GroundListObserver();

            var testGroundObservables = new GroundObservables();

            //Insert

            //var customer = new Customer
            //{
            //    Name = "John Doe",
            //    Phones = new string[] { "8000-0000", "9000-0000" },
            //    IsActive = true
            //};
            //testGroundObserver.Subscribe(testGroundObservables.Insert<Customer>(customer));

            //Update

            //var customer1 = new Customer();
            //customer1.Id = 3;
            //customer1.Name = "komal kadam";
            //testGroundObserver.Subscribe(testGroundObservables.Update<Customer>(customer1));


            //Upsert

            //var customer2 = new Customer();
            //customer2.Id = 2;
            //customer2.Name = "Priya";
            //testGroundObserver.Subscribe(testGroundObservables.Upsert<Customer>(customer2));

            //FetchAll

            //testGObserver.Subscribe(testGroundObservables.FetchAll<Customer>());


            //Fetch By id

            //testGroundObserver.Subscribe(testGroundObservables.FetchById<Customer>("2"));

            //Delete

            //testGroundObserver.Subscribe(testGroundObservables.Delete<Customer>("2"));

        }
    }
   
}
