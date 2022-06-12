using PerfTests;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.LocaleIItems;

var timer = new PerfTimer<Signature>(LocaleItemFactory.CreateFromKeyValue, "foo", "Super {This:Is{Very:Nested {As:You} can see}@} nesting");
Console.WriteLine($"{timer.PerformTiming().ElapsedTime.Milliseconds} ms");


internal delegate ILocaleItem Signature(string key, string value);
