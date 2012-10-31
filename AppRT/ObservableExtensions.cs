using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AppRT
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> self, Action act)
        {
            return self.Subscribe(_ => act());
        }

        public static IObservable<Unit> IgnoreValues<T>(this IObservable<T> self)
        {
            return self.Select(_ => Unit.Default);
        }

        public static IObservable<T> Select<T>(this IObservable<Unit> self, Func<T> selector)
        {
            return self.Select(_ => selector());
        }
    }
}
