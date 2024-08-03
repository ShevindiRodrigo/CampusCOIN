using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CampusCOIN.Models
{
    public class ObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
