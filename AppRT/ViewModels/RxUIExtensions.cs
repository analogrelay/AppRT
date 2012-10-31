using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRT.ViewModels
{
    public static class RxUIExtensions
    {
        public static IObservable<Unit> ObservableForProperties<T>(this T self, params Expression<Func<T, object>>[] properties) where T : ReactiveObject
        {
            return Observable.Merge(properties.Select(p => self.ObservableForProperty(p).IgnoreValues()));
        }

        public static IObservable<Unit> ObservableForProperties<T>(this T self, bool beforeChange, params Expression<Func<T, object>>[] properties) where T : ReactiveObject
        {
            return Observable.Merge(properties.Select(p => self.ObservableForProperty(p, beforeChange).IgnoreValues()));
        }
    }
}
