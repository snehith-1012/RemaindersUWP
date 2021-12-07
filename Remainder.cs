using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Remainders;

namespace Remainders
{
    public class Remainder : INotifyPropertyChanged
    {
        public string RemainderId;
        public string _description;
        public DateTimeOffset _dateAndTime;
        public string _dateAndTimeStr;
        public bool _isCompleted;
        //private ObservableCollection<string> _zuids;
        public string _channelId;
        public ObservableCollection<Person> people;
        


        public string Description
        {
            get { return _description; }
            set 
            {
                if(value.Length!=0) 
                    _description = value; 
                RaisePropertyChanged();
            }
        }

        public DateTimeOffset DateAndTime
        {
            get { return _dateAndTime; }
            set { _dateAndTime = value; RaisePropertyChanged(); }
        }


        public string DateAndTimeStr
        {
            get { return _dateAndTimeStr; }
            set { _dateAndTimeStr = value; RaisePropertyChanged(); }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; RaisePropertyChanged(); }
        }

        

        public string ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; RaisePropertyChanged(); }
        }

        Dictionary<string, string> MapOfIdAndName = new Dictionary<string, string>();

        public ObservableCollection<Person> People
        {
            get { return people; }
            set { people = value; RaisePropertyChanged(); }
        }


        public Remainder(string description, DateTimeOffset dateandtime, bool isCompleted, string channelId, string remainderId)
        {
            People = new ObservableCollection<Person>();
            this.RemainderId = remainderId;
            this.Description = description;
            //this.DateAndTime = dateandtime;
            this.IsCompleted = isCompleted;
            this.ChannelId = channelId;
                this.DateAndTime = dateandtime;
                this.DateAndTimeStr = dateandtime.ToString();
        }
 

        public Windows.UI.Xaml.Visibility CheckWhetherAssignedOrNot(ObservableCollection<Person> zuids)
        {
            if (zuids.Count > 0)
                return Windows.UI.Xaml.Visibility.Visible;
            else
                return Windows.UI.Xaml.Visibility.Collapsed;
        }
        public Windows.UI.Xaml.Visibility isDateAndTimeSet(DateTimeOffset dt)
        {
            if (dt != DateTimeOffset.MinValue && dt.Year!=2)
                return Windows.UI.Xaml.Visibility.Visible;
            else
                return Windows.UI.Xaml.Visibility.Collapsed;
        }

        //public ObservableCollection<Person> getRemainingPeople()


        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string getPeople(ObservableCollection<Person> people)
        {
            if (people.Count == 0)
                return "";
            else if(people.Count==1)
            {
                return " "+people.FirstOrDefault().Name;
            }
            else
            {
                return people.Count.ToString() + " members ";
            }
        }

        public Windows.UI.Xaml.Visibility isItPersonalOrAssigned(ObservableCollection<Person> people)
        {
            if (people.Count == 0)
                return Windows.UI.Xaml.Visibility.Collapsed;
            else
                return Windows.UI.Xaml.Visibility.Visible;
        }

        public string dateSetStatus(DateTimeOffset dt)
        {
            if (DateTimeOffset.MinValue == dt || dt.Year==2)
            {
                return "Set due date";
            }
            else
                return dt.DateTime.ToString();
        }

    }
}







//public string getPersonsList(ObservableCollection<string> personZuids)
//{
//    if (MapOfIdAndName.Count == 0)
//    {
//        foreach (var i in m.Persons)
//        {
//            MapOfIdAndName.Add(i.zuid, i.name);
//        }
//    }
//    string res = "";
//    foreach(var i in personZuids.ToHashSet())
//    {
//        if (i == "1")
//            continue;
//        if (res == "")
//            res += MapOfIdAndName[i];
//        else
//            res += " and " + MapOfIdAndName[i];
//    }
//    return res;
//}




//public ObservableCollection<string> ZuIds
//{
//    get { return _zuids; }
//    set { _zuids = value; RaisePropertyChanged(); }
//}