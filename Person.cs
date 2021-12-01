using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Remainders;

namespace Remainders
{
    public class Person : INotifyPropertyChanged
    {
        public string name, email;

        public string ZuId { get; set; }
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public string Email
        {
            get { return email; }
            set { email = value; RaisePropertyChanged(); }
        }



        public Person(string id, string name, string email)
        {
            this.ZuId = id;
            this.Name = name;
            this.Email = email;
        }




        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        /*public string PeopleIncluded()
        {
            string ans = "";
            foreach(var i in r.m.peopleAdded)
            {
                ans += i.name;
            }
            //foreach (var i in m.peopleAdded)
            //    ans += i;
            return ans;
        }*/
    }
}
