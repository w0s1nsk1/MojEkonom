using System;
using Xamarin.Forms;

namespace EkonomApp.Helpers
{
    public static class XFToast
    {
        public static void ShortMessage(string message)
        {
            DependencyService.Get<IMessage>().ShortAlert(message);
        }

        public static void LongMessage(string message)
        {
            DependencyService.Get<IMessage>().LongAlert(message);
        }

        internal static void LongMessage(Func<string> toString)
        {
            throw new NotImplementedException();
        }
    }
}
