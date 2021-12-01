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
    public class Channel : INotifyPropertyChanged
    {
        public string channelId;
        public string channelName;


        public string ChannelId
        {
            get { return channelId; }
            set { channelId = value; RaisePropertyChanged(); }
        }

        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; RaisePropertyChanged(); }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
