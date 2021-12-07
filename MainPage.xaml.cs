using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Popups;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Remainders;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Remainders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public InlineCollection Inlines { get; set; }
        public List<Channel> channelsList = new List<Channel>();
        public List<Person> Persons = new List<Person>();
        public ObservableCollection<Person> NotAddedPersons= new ObservableCollection<Person>();
        public ObservableCollection<Remainder> Remainders = new ObservableCollection<Remainder>();
        

        public List<Person> TemperorySelectedPersonList = new List<Person>();
        public ObservableCollection<Remainder> RemaindersForMe = new ObservableCollection<Remainder>();

        public int whichremainder = 0;
        public int whichremainder1
        {
            get
            {
                return whichremainder;
            }
            set
            {
                whichremainder = value;
                RaisePropertyChanged(nameof(whichremainder));
                if(whichremainder==0)
                {
                    BorderForMe.BorderBrush = new SolidColorBrush(Colors.White);
                    BorderForAssignedToOthers.BorderBrush = new SolidColorBrush(Colors.Black);
                    //FormeIsSelected.Visibility = Visibility.Visible;
                    //AssignedtoOthersIsSelected.Visibility = Visibility.Collapsed;
                }
                else
                {
                    BorderForMe.BorderBrush = new SolidColorBrush(Colors.Black);
                    BorderForAssignedToOthers.BorderBrush = new SolidColorBrush(Colors.White);
                    //FormeIsSelected.Visibility = Visibility.Collapsed;
                    //AssignedtoOthersIsSelected.Visibility = Visibility.Visible;
                }
            }
        }
        public string remainderId = "a";
        public string peopleAdded = "";
        public string peopleIncludedforOneRemainder
        {
            get
            {
                return peopleAdded;
            }
            set
            {
                peopleAdded = value;
                RaisePropertyChanged();
            }
        }

        public int channelId = -1;
        public string UIPersonId = "1";
        //public int dateSet = 0;

        public ObservableCollection<Remainder> CompletedRemainders = new ObservableCollection<Remainder>();
        public DateTimeOffset RemainderDateAndTime;

        public DateTimeOffset RemainderDateAndTimeDuplicate
        {
            get
            {
                return RemainderDateAndTime;
            }
            set
            {
                RemainderDateAndTime = value;
                RaisePropertyChanged();
            }
        }
        //public DateTimeOffset helpsInRetrieveback;


        public MainPage()
        {
            this.InitializeComponent();
            Persons.Add(new Person("1", "snehith", "snehith.oddula@zohocorp.com"));
            Persons.Add(new Person("2", "cperson2", "someone@zohocorp.com"));
            Persons.Add(new Person("3", "noone2", "someone2@zohocorp.com"));
            Persons.Add(new Person("4", "anyone3", "someone3@zohocorp.com"));
            Persons.Add(new Person("5", "aperson", "aperson@zohocorp.com"));
            Persons.Add(new Person("6", "bperson", "bperson@zohocorp.com"));
            Persons.Add(new Person("7", "cperson", "cperson@zohocorp.com"));

            foreach (var i in Persons)
            {
                if (i.Name != "snehith")
                {
                    NotAddedPersons.Add(i);
                }
            }
            RaisePropertyChanged(nameof(NotAddedPersons));
            //RemainderDateAndTime = DateTime.Today;
            //Testing.Text = DateTimeOffset.MinValue.ToString();
        }

        private void RemaindersButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void StartTypingPersonNames_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(item.Text))
            {
                MyAutoSuggestBox.ItemsSource = NotAddedPersons.Where(p => p.Name != "snehith");
                item.Text = "";
            }
            else
            {
                MyAutoSuggestBox.ItemsSource = NotAddedPersons.Where(p => p.Name.StartsWith(item.Text, StringComparison.OrdinalIgnoreCase) && p.Name != "snehith");
            }
        }
   
        /* private void MyAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
         {
             Person selectedPerson = (Person)args.SelectedItem;
             StartTypingPersonNames.Text = selectedPerson.Name;


             foreach (var i in Persons)
             {
                 if (i.ZuId == selectedPerson.ZuId)
                 {

                     foreach(var j in NotAddedPersons)
                     {
                         if(j==i)
                         {
                             NotAddedPersons.Remove(j);
                             break;
                         }
                     }
                     TemperorySelectedPersonList.Add(i);    
                     if (peopleAdded != "")
                         peopleAdded += " and " + selectedPerson.Name;
                     else
                         peopleAdded += selectedPerson.Name;
                     RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
                     break;
                 }
             }

             //PeopleRemoveButton.Content = TemperorySelectedPersonList.Count.ToString();
             NumberOrCross.Text = TemperorySelectedPersonList.Count.ToString();

             if (TemperorySelectedPersonList.Count > 0)
             {
                 PeopleRemoveButton.Visibility = Visibility.Visible;
             }
             else
             {
                 PeopleRemoveButton.Visibility = Visibility.Collapsed;
             }

             MyAutoSuggestBox.Text = "";
             MyAutoSuggestBox.IsSuggestionListOpen = true;
             //peopleAdded = "";
         }*/

        private void MyAutoSuggestBox_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            //testing.Text += "a";
            Person selectedPerson = (Person)e.ClickedItem;
            //StartTypingPersonNames.Text = selectedPerson.Name;

            foreach (var i in Persons)
            {
                if (i.ZuId == selectedPerson.ZuId)
                {

                    foreach (var j in NotAddedPersons)
                    {
                        if (j == i)
                        {
                            NotAddedPersons.Remove(j);
                            RaisePropertyChanged(nameof(NotAddedPersons));
                            break;
                        }
                    }
                    TemperorySelectedPersonList.Add(i);
                    if (peopleAdded != "")
                        peopleAdded += " and " + selectedPerson.Name;
                    else
                        peopleAdded += selectedPerson.Name;
                    RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
                    break;
                }
            }
            NumberOrCross.Text = TemperorySelectedPersonList.Count.ToString();

            if (TemperorySelectedPersonList.Count > 0)
            {
                PeopleRemoveButton.Visibility = Visibility.Visible;
            }
            else
            {
                PeopleRemoveButton.Visibility = Visibility.Collapsed;
            }
            if(StartTypingPersonNames.Text!="")
            StartTypingPersonNames.Text = "";
        }

        private void RemainderSetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("Please add remainder description", "Remainder description Empty");
            //Testing.Text += dateSet.ToString();
            if (string.IsNullOrWhiteSpace(RemainderDescription.Text))
            {
                _ = messageDialog.ShowAsync();
                RemainderDescription.Text = "";
                return;
            }


            var newRemainder = new Remainder(RemainderDescription.Text, RemainderDateAndTime, false, null, remainderId + 'a');

            if (TemperorySelectedPersonList.Count == 0)
            {
                RemaindersForMe.Add(newRemainder);
                whichremainder1 = 0;
                //RaisePropertyChanged(nameof(whichremainder));
            }
            else
            {
                foreach (var item in TemperorySelectedPersonList)
                {
                    newRemainder.People.Add(item);
                }
                Remainders.Add(newRemainder);
                whichremainder1 = 1;
                //RaisePropertyChanged(nameof(whichremainder));
            }
            remainderId += 'a';
            RemainderDescription.Text = "";
            DateAndTimeDisplay.Text = "";



            TemperorySelectedPersonList.Clear();
            peopleAdded = "";
            peopleIncludedforOneRemainder = "";
            RemainderDateAndTime = DateTime.Now;
            RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
            RemainderDateAndTime = DateTime.Today;
            //dateSet = 0;
            //Testing.Text += dateSet.ToString();
            PeopleRemoveButton.Visibility = Visibility.Collapsed;
            SetTime1.SelectedTime = null;
            //CalendarListView.SetDisplayDate(DateTimeOffset.Now);
            //CalendarListView.SelectedDates.Clear();
            NotAddedPersons.Clear();
            foreach (var i in Persons)
            {
                if(i.Name!="snehith")
                NotAddedPersons.Add(i);
            }
            RaisePropertyChanged(nameof(NotAddedPersons));
            //RemainderDateAndTime = new DateTimeOffset();
            RemainderDateAndTime = DateTimeOffset.MinValue;
            CalendarListView.SelectedDates.Clear();
            CalendarListView.IsTodayHighlighted = true;
        }

        private void MyRemainders_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ListOfRemainders_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var myValue = (string)(((Button)sender).Tag);
            if (whichremainder == 1)
            {
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == myValue)
                    {
                        Remainders.Remove(i);
                        return;
                    }
                }
            }
            else
            {
                foreach (var i in RemaindersForMe)
                {
                    if (i.RemainderId == myValue)
                    {
                        RemaindersForMe.Remove(i);
                        return;
                    }
                }
            }
        }

        private void MarkAsComplete_Click(object sender, RoutedEventArgs e)
        {
            var myValue = (string)(((Button)sender).Tag);

            if(whichremainder==0)
            {
                foreach (var i in RemaindersForMe)
                {
                    if (i.RemainderId == myValue)
                    {
                        CompletedRemainders.Add(i);
                        RemaindersForMe.Remove(i);
                        //Remainders.Remove(i);
                        break;
                    }
                }
            }
            else
            {
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == myValue)
                    {
                        CompletedRemainders.Add(i);
                        //RemaindersForMe.Remove(i);
                        Remainders.Remove(i);
                        break;
                    }
                }
            }
        }

        private void FlyoutCalender_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {

        }

        private void PeopleRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            peopleAdded = "";
            RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
            TemperorySelectedPersonList.Clear();
            PeopleRemoveButton.Visibility = Visibility.Collapsed;
            NotAddedPersons.Clear();
            foreach (var i in Persons)
            {
                if(i.Name!="snehith")
                NotAddedPersons.Add(i);
            }
   
            RaisePropertyChanged(nameof(NotAddedPersons));
        }

        private void MyAutoSuggestBox1_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var item = (AutoSuggestBox)sender;
            var remainderId = (string)sender.Tag;
            Person selectedPerson = (Person)args.SelectedItem;
            
            foreach (var i in Remainders)
            {
                if(i.RemainderId==remainderId)
                {
                    i.People.Add(selectedPerson);
                    break;
                }
            }
            //if (sender.Text != "")
                item.Text = " ";
                item.Text = "";
            
            item.IsSuggestionListOpen = true;
            //if (sender.Text.Length == 0)
              //  return;

        }

       /* private void MyAutoSuggestBox1_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var remainderId = (string)sender.Tag;
            ObservableCollection<Person> suggestion = new ObservableCollection<Person>();
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == remainderId)
                    {
                        foreach (var j in Persons)
                        {
                            int duplicate = 0;
                            foreach (var k in i.People)
                            {
                                if (k == j)
                                {
                                    duplicate = 1;
                                    break;
                                }
                            }
                            if (duplicate == 0)
                                suggestion.Add(j);
                        }

                        break;
                    }

                }
            if (string.IsNullOrEmpty(sender.Text))
            {
                sender.ItemsSource = suggestion.Where(p => p.Name != "snehith");
            }
            else
            {
                /*int whiteSpaces = 0;
                foreach (var i in sender.Text)
                {
                    if (i == ' ')
                        whiteSpaces++;
                    else
                        break;
                }
                if (whiteSpaces == sender.Text.Length)
                /////////////////////////////////    sender.Text = "";
                if (string.IsNullOrWhiteSpace(sender.Text))
                    sender.Text = "";
                sender.ItemsSource = suggestion.Where(p => p.Name.StartsWith(sender.Text, StringComparison.OrdinalIgnoreCase) && p.Name != "snehith");
            }
        }*/

        public ObservableCollection<Person> tempPersons;
        private void StartTypingPersonNames1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = (TextBox)sender;
            var remainderId = (string)item.Tag;
            tempPersons = new ObservableCollection<Person>();
            foreach (var i in Remainders)
            {
                if (i.RemainderId == remainderId)
                {
                    foreach (var j in Persons)
                    {
                        int duplicate = 0;
                        foreach (var k in i.People)
                        {
                            if (k == j)
                            {
                                duplicate = 1;
                                break;
                            }
                        }
                        if (duplicate == 0)
                            tempPersons.Add(j);
                    }

                    break;
                }

            }
            if (string.IsNullOrEmpty(item.Text))
            {
                foreach(var i in tempPersons)
                {
                    if (i.Name == "snehith")
                    {
                        tempPersons.Remove(i);
                        break;
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(item.Text))
                    item.Text = "";
                List<Person> p = new List<Person>();

                foreach(var i in tempPersons)
                {
                    if(i.Name.StartsWith(item.Text,StringComparison.OrdinalIgnoreCase) && i.Name!="snehith" )
                    {
                        p.Add(i);
                    }
                }
                tempPersons.Clear();
                foreach(var i in p)
                {
                    tempPersons.Add(i);
                }
            }
            RaisePropertyChanged(nameof(tempPersons));
            //foreach (var i in tempPersons)
            //  testing.Text += i.Name;

            tempListView.ItemsSource = tempPersons;
        }

        public ListView tempListView;

        int stackpanelContainingAutoSuggestBoxIsOpen = 0;
        private void StackPanelContainingAutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var item = (StackPanel)sender;
            ListView k = (ListView)VisualTreeHelper.GetChild(item, 1);
            tempListView = k;
            if (stackpanelContainingAutoSuggestBoxIsOpen == 0)
            {
                TextBox t = (TextBox)VisualTreeHelper.GetChild(item, 0);
                var remainderId = (string)item.Tag;
                tempPersons = new ObservableCollection<Person>();
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == remainderId)
                    {
                        foreach (var j in Persons)
                        {
                            int duplicate = 0;
                            foreach (var z in i.People)
                            {
                                if (z == j)
                                {
                                    duplicate = 1;
                                    break;
                                }
                            }
                            if (duplicate == 0)
                                tempPersons.Add(j);
                        }

                        break;
                    }

                }
                if (string.IsNullOrWhiteSpace(t.Text))
                {
                    foreach (var i in tempPersons)
                    {
                        if (i.Name == "snehith")
                        {
                            tempPersons.Remove(i);
                            break;
                        }
                    }
                }
                tempListView.ItemsSource = tempPersons;
                stackpanelContainingAutoSuggestBoxIsOpen = 1;
            }
        }

        private void MyAutoSuggestBox1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (ListView)sender;
            string remainderId = (string)item.Tag;
            Person selectedPerson = (Person)e.ClickedItem;
            foreach(var i in Remainders)
            {
                if(i.RemainderId==remainderId)
                {
                    foreach(var j in tempPersons)
                    {
                        if(j==selectedPerson)
                        {
                            tempPersons.Remove(j);
                            break;
                        }
                    }
                    i.people.Add(selectedPerson);
                    
                    break;
                }
            }
            StackPanel s = (StackPanel)VisualTreeHelper.GetParent(item);
            TextBox t = (TextBox)VisualTreeHelper.GetChild(s, 0);
            t.Text = "";
        }

        private void FlyoutWhichContainsAutosuggestBox_Closed(object sender, object e)
        {
            if(tempPersons!=null)
            tempPersons.Clear();
            stackpanelContainingAutoSuggestBoxIsOpen = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void PeopleRemoveButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //PeopleRemoveButton.Content = "X";
            NumberOrCross.Text = "X";
            //FlyoutBase.ShowAttachedFlyout((Button)sender);
            //PeopleRemoveButton.Background = new SolidColorBrush(Colors.Red);

        }

        private void PeopleRemoveButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            NumberOrCross.Text = TemperorySelectedPersonList.Count.ToString();
            //ThisShowsPeopleIncluded.Hide();
            //PeopleRemoveButton.Content = TemperorySelectedPersonList.Count.ToString();
            // PeopleRemoveButton.Background = new SolidColorBrush(Colors.Red);
        }

        private void MyAutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (peopleButton == 1)
            {
                MyAutoSuggestBox.ItemsSource = NotAddedPersons.Where(p => p.Name != "snehith");
                //MyAutoSuggestBox.IsSuggestionListOpen = true;
                peopleButton = 0;
            }
        }
        public int peopleButton = 0;// avoids loading of suggestions twice when getfocus and textchanged event works at a time in autosuggestbox
        private void People_Click(object sender, RoutedEventArgs e)
        {
            peopleButton = 1;
        }

        private void MyAutoSuggestBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            var item= (AutoSuggestBox)sender;
            var remainderId = (string)item.Tag;
            if (string.IsNullOrEmpty(item.Text) && AddPeopleButton==1)
            {
                AddPeopleButton = 0;
                ObservableCollection<Person> suggestion = new ObservableCollection<Person>();
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == remainderId)
                    {
                        foreach (var j in Persons)
                        {
                            int duplicate = 0;
                            foreach (var k in i.People)
                            {
                                if (k == j)
                                {
                                    duplicate = 1;
                                    break;
                                }
                            }
                            if (duplicate == 0)
                                suggestion.Add(j);
                        }

                        break;
                    }

                }
                if (string.IsNullOrEmpty(item.Text))
                {
                    item.ItemsSource = suggestion.Where(p => p.Name != "snehith");
                }
                else
                {
                    //item.ItemsSource = suggestion.Where(p => p.Name.StartsWith(item.Text, StringComparison.OrdinalIgnoreCase) && p.Name != "snehith");
                }
                item.IsSuggestionListOpen = true;
            }
            
        }

        public int AddPeopleButton = 0;// avoids loading of suggestions twice when getfocus and textchanged event works at a time in edit remainder autosuggestbox
        private void AddMorePeople_Click(object sender, RoutedEventArgs e)
        {
            AddPeopleButton = 1;
        }

        private void CalendarListView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            //Testing.Text = RemainderDateAndTime.ToString();

            List<DateTimeOffset> dateTimeOffsets = args.AddedDates.ToHashSet().ToList();
            RemainderDateAndTime = dateTimeOffsets.FirstOrDefault();
            CalendarListView.IsTodayHighlighted = false;
            DateAndTimeDisplay.Text = RemainderDateAndTime.ToString();
            //RemainderDateAndTime = new DateTimeOffset();
        }

        private void SaveDate_Click(object sender, RoutedEventArgs e)
        {
            //testing.Text = "where";
            if(RemainderDateAndTime==DateTimeOffset.MinValue)
            {
                RemainderDateAndTime = getNextHalfAnHour(DateTimeOffset.Now);
                DateAndTimeDisplay.Text = RemainderDateAndTime.ToString();
                //testing.Text += RemainderDateAndTime.ToString();
                DateAndTimeFlyout.Hide();
                return;
            }
            var date = RemainderDateAndTime;
            var formateddate = date.Date;
            if (SetTime1.SelectedTime==null)
            {
                //testing.Text= (formateddate + DateTime.Now.TimeOfDay).ToString();
                //return;
                RemainderDateAndTime = formateddate + DateTime.Now.TimeOfDay;
                RemainderDateAndTime = getNextHalfAnHour(RemainderDateAndTime);
            }
            DateAndTimeDisplay.Text = RemainderDateAndTime.ToString();
            //testing.Text += RemainderDateAndTime.ToString();
            DateAndTimeFlyout.Hide();
        }
        public DateTimeOffset getNextHalfAnHour(DateTimeOffset dateTime)
        {
            //Testing.Text += "i am dng something";
            if (dateTime.Minute < 30)
            {
                dateTime = dateTime.AddMinutes(30 - dateTime.Minute);
                dateTime = dateTime.AddSeconds(-1 * (dateTime.Second));
                return dateTime;
            }
            else
            {
                dateTime=dateTime.AddMinutes(60 - dateTime.Minute);
                dateTime=dateTime.AddSeconds(-1 * (dateTime.Second));
                return dateTime;
            }
        }

        private void FlyoutCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {

        }

        public DateTimeOffset tempRemainderDateAndTime;
        private void FlyoutCalendarListView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            //testing.Text += "a";
            DateTimeOffset temp;
            string tagRemainderId;
            var calendarr = (CalendarView)sender;
                var item = sender.Tag;
                 tagRemainderId = (string)item;
                List<DateTimeOffset> dateTimeOffsets1 = calendarr.SelectedDates.ToHashSet().ToList();
                temp = dateTimeOffsets1.FirstOrDefault();
            calendarr.IsTodayHighlighted = false;
            if (whichremainder == 1)
            {
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == tagRemainderId)
                    {
                        
                        //i.DateAndTime = new DateTimeOffset(temp.Year, temp.Month, temp.Day, i.DateAndTime.Hour, i.DateAndTime.Minute, i.DateAndTime.Second, new TimeSpan(5, 30, 0));
                        tempRemainderDateAndTime = new DateTimeOffset(temp.Year+1, temp.Month, temp.Day, i.DateAndTime.Hour, i.DateAndTime.Minute, i.DateAndTime.Second, new TimeSpan(5, 30, 0));
                        //testing.Text = tempRemainderDateAndTime.ToString();
                        return;
                    }
                }
            }
            else
            {
                foreach (var i in RemaindersForMe)
                {
                    if (i.RemainderId == tagRemainderId)
                    {
                        //i.DateAndTime = new DateTimeOffset(temp.Year, temp.Month, temp.Day, i.DateAndTime.Hour, i.DateAndTime.Minute, i.DateAndTime.Second, new TimeSpan(5, 30, 0));
                        //Testing.Text = i.DateAndTime.ToString();
                        tempRemainderDateAndTime= new DateTimeOffset(temp.Year+1, temp.Month, temp.Day, i.DateAndTime.Hour, i.DateAndTime.Minute, i.DateAndTime.Second, new TimeSpan(5, 30, 0));
                        return;
                    }
                }
            }
            calendarr.SelectedDates.Clear();
        }

        private void SetTime2_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            var item = sender.Tag;
            var tagRemainderId = (string)item;
            if (whichremainder == 1)
            {
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == tagRemainderId)
                    {
                        //i.DateAndTime = new DateTimeOffset(i.DateAndTime.Year, i.DateAndTime.Month, i.DateAndTime.Day, sender.Time.Hours, sender.Time.Minutes, sender.Time.Seconds, new TimeSpan(5, 30, 0));
                        //Testing.Text = i.DateAndTime.ToString();
                        tempRemainderDateAndTime= new DateTimeOffset(i.DateAndTime.Year, i.DateAndTime.Month, i.DateAndTime.Day, sender.Time.Hours, sender.Time.Minutes, sender.Time.Seconds, new TimeSpan(5, 30, 0));
                        break;
                    }
                }
            }
            else
            {
                foreach(var i in RemaindersForMe)
                {
                    if (i.RemainderId == tagRemainderId)
                    {
                        //i.DateAndTime = new DateTimeOffset(i.DateAndTime.Year, i.DateAndTime.Month, i.DateAndTime.Day, sender.Time.Hours, sender.Time.Minutes, sender.Time.Seconds, new TimeSpan(5, 30, 0));
                        //Testing.Text = i.DateAndTime.ToString();
                        tempRemainderDateAndTime = new DateTimeOffset(i.DateAndTime.Year, i.DateAndTime.Month, i.DateAndTime.Day, sender.Time.Hours, sender.Time.Minutes, sender.Time.Seconds, new TimeSpan(5, 30, 0));
                        break;
                    }
                }
            }
        }

        private void DateSaveButtonInsideFlyout_Click(object sender, RoutedEventArgs e)
        {
            var item = (Button)sender;
            var tagRemainderId = (string)item.Tag;
            if (whichremainder == 1)
            {
                foreach (var i in Remainders)
                {
                    if (i.RemainderId == tagRemainderId)
                    {
                        i.DateAndTime = tempRemainderDateAndTime;
                        return;
                    }
                }
            }
            else
            {
                foreach (var i in RemaindersForMe)
                {
                    if (i.RemainderId == tagRemainderId)
                    {
                        i.DateAndTime = tempRemainderDateAndTime;
                        return;
                    }
                }
            }
            
        }

        public string CurrentRemainderFlyoutOpenedId;

        private void PersonInformation_GotFocus(object sender, RoutedEventArgs e)
        {
            var item = (ListView)sender;
            CurrentRemainderFlyoutOpenedId = (string)item.Tag;
        }

        private void DeleteThisPerson_Click(object sender, RoutedEventArgs e)
        {
            var item = (Button)sender;
            var zuidToBeDeleted =(string)item.Tag;
            foreach(var i in Remainders)
            {
                if(i.RemainderId==CurrentRemainderFlyoutOpenedId)
                {
                    foreach(var j in i.People )
                    {
                        if(j.ZuId==zuidToBeDeleted)
                        {
                            if(i.people.Count==1)
                            {
                                MessageDialog messageDialog = new MessageDialog("You can't delete there should be atleast one person assigned to this remainder", "Remainder description Empty");
                                _ = messageDialog.ShowAsync();
                            }
                            else
                            i.people.Remove(j);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void RemainderDescription_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //Testing.Text = "jhwbkcwbci";
            RemainderDescription.Width = 200;
            PeopleRelativePanel.Visibility = Visibility.Visible;
            SetTime.Visibility = Visibility.Visible;
            RemainderSetButton.Visibility = Visibility.Visible;
        }

        private void MySplitView_PaneClosed(SplitView sender, object args)
        {
            RemainderDescription.Width = 350;
            PeopleRelativePanel.Visibility = Visibility.Collapsed;
            SetTime.Visibility = Visibility.Collapsed;
            RemainderSetButton.Visibility = Visibility.Collapsed;
        }

        private void SetTime1_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            if (RemainderDateAndTime == DateTimeOffset.MinValue)
            {
                RemainderDateAndTime = DateTimeOffset.Now;
                RemainderDateAndTime = getNextHalfAnHour(RemainderDateAndTime);
                var date1 = RemainderDateAndTime;
                var formateddate1 = date1.Date;
                if (SetTime1.SelectedTime != null)
                    RemainderDateAndTime = formateddate1 + SetTime1.Time;
                //DateAndTimeFlyout.Hide();
                DateAndTimeDisplay.Text = RemainderDateAndTime.ToString();
                return;
            }
            var date = RemainderDateAndTime;
            var formateddate = date.Date;
            if (SetTime1.SelectedTime != null)
            {
                RemainderDateAndTime = formateddate + SetTime1.Time;
                //RemainderDateAndTime = getNextHalfAnHour(RemainderDateAndTime);
            }
            else
            {
                RemainderDateAndTime = formateddate + DateTime.Now.TimeOfDay;
                RemainderDateAndTime = getNextHalfAnHour(RemainderDateAndTime);
            }
            DateAndTimeDisplay.Text = RemainderDateAndTime.ToString();
        }

        public ObservableCollection<Remainder> whichRemainders(int rem)
        {
            //Testing.Text += "efdghsvdws";
            if (whichremainder == 0)
                return RemaindersForMe;
            else
                return Remainders;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            whichremainder1 = 0;
            //FormeIsSelected.Visibility = Visibility.Visible;
            //AssignedtoOthersIsSelected.Visibility = Visibility.Collapsed;
            //Forme.BorderBrush = new SolidColorBrush(Colors.White);
            //AssignedToOthers.BorderBrush = new SolidColorBrush(Colors.Black);
            //RaisePropertyChanged(nameof(whichremainder));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            whichremainder1 = 1;
            //FormeIsSelected.Visibility = Visibility.Collapsed;
            //AssignedtoOthersIsSelected.Visibility = Visibility.Visible;
            //AssignedToOthers.BorderBrush = new SolidColorBrush(Colors.White);
            //Forme.BorderBrush = new SolidColorBrush(Colors.Black);
            //RaisePropertyChanged(nameof(whichremainder));
        }


        private void DateAndTimeFlyout1_Closed(object sender, object e)
        {
            tempRemainderDateAndTime = DateTimeOffset.MinValue;
        }

        private void FlyoutCalendarListView_LostFocus(object sender, RoutedEventArgs e)
        {
            //var item = (CalendarView)sender;
            //item.SelectedDates.Clear();
        }

        public bool LettersEntered(Windows.System.VirtualKey ky)
        {
            if(((Windows.System.VirtualKey)65<=ky && ky <= (Windows.System.VirtualKey)92) || ((Windows.System.VirtualKey)97 <= ky && ky <= (Windows.System.VirtualKey)122))
            {
                return true;
            }
            return false;
        }

        private void SuggestionsList1_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var item = (Grid)sender;
        }

        private void StartTypingPersonNames1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var item = (TextBox)sender;
            item.Focus(FocusState.Keyboard);
        }

        private void StartTypingPersonNames_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(LettersEntered(e.Key))
            {
                return;
            }
            
            //MyAutoSuggestBox.Focus(FocusState.Keyboard);
            if (e.Key==Windows.System.VirtualKey.Down)
            {
                //MyAutoSuggestBox.SelectedIndex = 0;
                
                //testing.Text = ((Person)MyAutoSuggestBox.Items.ElementAt(MyAutoSuggestBox.SelectedIndex)).Name.ToString();
                MyAutoSuggestBox.SelectedItem = MyAutoSuggestBox.Items.ElementAt(0);
            }
            else if(e.Key==Windows.System.VirtualKey.Up)
            {
                MyAutoSuggestBox.SelectedItem = MyAutoSuggestBox.Items.ElementAt(MyAutoSuggestBox.Items.Count - 1);
            }
            else if(e.Key==Windows.System.VirtualKey.Back)
            {
                StartTypingPersonNames.Focus(FocusState.Keyboard);
            }
        }

        private void MyAutoSuggestBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(LettersEntered(e.Key))
            {
                StartTypingPersonNames.Focus(FocusState.Keyboard);
                StartTypingPersonNames.Text += e.Key.ToString();
                StartTypingPersonNames.SelectionStart = StartTypingPersonNames.Text.Length;
            }
            else if(e.Key==Windows.System.VirtualKey.Down)
            {
                if(MyAutoSuggestBox.SelectedItem==MyAutoSuggestBox.Items.ElementAt(MyAutoSuggestBox.Items.Count-1))
                {
                    MyAutoSuggestBox.SelectedItem = MyAutoSuggestBox.Items.ElementAt(0);
                }
            }
            else if(e.Key == Windows.System.VirtualKey.Up)
            {
                if (MyAutoSuggestBox.SelectedItem == MyAutoSuggestBox.Items.ElementAt(0))
                {
                    MyAutoSuggestBox.SelectedItem = MyAutoSuggestBox.Items.ElementAt(MyAutoSuggestBox.Items.Count - 1);
                }
            }
        }

        private void StartTypingPersonNames1_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            StackPanel s = (StackPanel)VisualTreeHelper.GetParent(t);
            ListView l = (ListView)VisualTreeHelper.GetChild(s, 1);
            if (LettersEntered(e.Key))
            {
                return;
            }
            l.Focus(FocusState.Keyboard);
            if (e.Key == Windows.System.VirtualKey.Up)
            {
                l.SelectedItem = l.Items.ElementAt(l.Items.Count - 1);
            }
            else if (e.Key == Windows.System.VirtualKey.Down)
            {
                l.SelectedItem = l.Items.ElementAt(0);
            }
            else if (e.Key == Windows.System.VirtualKey.Back)
            {
                t.Focus(FocusState.Keyboard);
            }
        }

        private void MyAutoSuggestBox1_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            ListView l = (ListView)sender;
            StackPanel s = (StackPanel)VisualTreeHelper.GetParent(l);
            TextBox t = (TextBox)VisualTreeHelper.GetChild(s, 0);
            if (LettersEntered(e.Key))
            {
                t.Focus(FocusState.Keyboard);
                t.Text += e.Key.ToString();
                t.SelectionStart = t.Text.Length;
            }
            else if (e.Key == Windows.System.VirtualKey.Down)
            {
                if (l.SelectedItem == l.Items.ElementAt(l.Items.Count - 1))
                {
                    l.SelectedItem = l.Items.ElementAt(0);
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Up)
            {
                if (l.SelectedItem == l.Items.ElementAt(0))
                {
                    l.SelectedItem = l.Items.ElementAt(l.Items.Count - 1);
                }
            }
        }
    }
}




/*ListView k = (ListView)VisualTreeHelper.GetChild(item, 1);
//ItemsPanelTemplate i = (ItemsPanelTemplate)VisualTreeHelper.GetChild(k, 0);
Border b = (Border)VisualTreeHelper.GetChild(k, 0);
ScrollViewer d = (ScrollViewer)VisualTreeHelper.GetChild(b, 0);
Border g = (Border)VisualTreeHelper.GetChild(d, 0);
Grid g1 = (Grid)VisualTreeHelper.GetChild(g, 0);
TextBlock t = (TextBlock)VisualTreeHelper.GetChild(g1, 0);
//t.BorderThickness = new Thickness(2);
t.Foreground = new SolidColorBrush(Colors.Black);*/













/*private void SaveDate_Click(object sender, RoutedEventArgs e)
        {

            if (dateSet == 1)
            {
                var date = Calendar.Date.Value.DateTime;
                var formateddate = date.Date;
                RemainderDateAndTime = formateddate + SetTime1.Time;
                DateAndTimeDisplay.Text = RemainderDateAndTime.ToString();
            }
            else
            {
                //RemainderDateAndTime = DateTime.Now;
                //DateAndTimeDisplay.Text = DateTime.Now.ToString();
            }
            DateAndTimeFlyout.Hide();
            
        }*/






























/*foreach (var i in RemaindersForMe)
{
    if (i.RemainderId == value)
    {
        foreach (var j in i.zuids)
        {//foreach(var i in RemaindersForMe.ToList())
            //{
            //    if(i.RemainderId==value)
            //    { 
            //        if(i.dateandtime.ToString().Count()>0)
            //        RemaindersForMe.Add(new Remainder(i.Description, i.DateAndTime, i.isCompleted, i.zuids, i.channelId, 1, remainderId+1));
            //        else
            //            RemaindersForMe.Add(new Remainder(i.Description, i.DateAndTime, i.isCompleted, i.zuids, i.channelId, 0, remainderId + 1));
            //        remainderId += 1;
            //        RemaindersForMe.Remove(i);
            //    }
            //}
            foreach (var k in Persons)
            {
                if (k.zuid == j)
                {
                    Testing.Text += k.name;
                }
            }
        }
        break;
    }
}*/




//foreach(var i in RemaindersForMe.ToList())
//{
//    if(i.RemainderId==value)
//    {
//        foreach(var j in Persons)
//        {
//            if(j.name==args.SelectedItem.ToString())
//            {
//                i.zuids.Add(j.zuid);
//                break;
//            }
//        }
//        break;
//    }
//}


/*Remainder selectedRemainder = Remainders.FirstOrDefault(a => a.RemainderId == remainderId);
            if (selectedRemainder.People.Where(a => a.ZuId == selectedPerson.ZuId).ToList().Count == 0)
            {
                selectedRemainder.People.Add(selectedPerson);
                RaisePropertyChanged();
            }*/

//foreach (var i in TemperorySelectedPersonList)
//{
//    if (i == UIPersonId)
//        RemaindersForMe.Add(Remainders[Remainders.Count - 1]);
//}



//public HashSet<string> Peoplenames = new HashSet<string>();
//public List<string> TemperoryList = new List<string>();


/*int duplicate = 0;
                    foreach (var j in TemperorySelectedPersonList)
                    {
                        if (j.ZuId == i.ZuId)
                        {
                            duplicate = 1;
                            break;
                        }
                    }
                    if (duplicate == 0)
                    {
                        TemperorySelectedPersonList.Add(i);
                        if (peopleAdded != "")
                            peopleAdded += " and " + selectedPerson.Name;
                        else
                            peopleAdded += selectedPerson.Name;
                        RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
                        break;
                    }*/