
<link rel="preconnect" href="https://fonts.googleapis.com">

<link rel="preconnect" href="https://fonts.gstatic.com">

<link href="https://fonts.googleapis.com/css2?family=Vazirmatn&display=swap" rel="stylesheet">

<style>

.adtrace-readme{
    font-family: "Vazirmatn", sans-serif;
}

</style>

<span class="adtrace-readme" >
<div dir="rtl" align="right">

<div align="center">

<img src="icon.png" alt="AdTrace Tracker" width="50" />

# Trackers.AdTrace

### 🔍 کتابخانه ردیابی AdTrace برای NET MAUI.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-8.0+-purple.svg)]()
[![Platform](https://img.shields.io/badge/Platform-Android-green.svg)]()
[![Platform](https://img.shields.io/badge/Platform-iOS-black.svg)]()

[![Coverage](https://img.shields.io/badge/coverage-100%25-greenm)]()

</div>

---

## 📋 فهرست مطالب

- [معرفی](#-معرفی)
- [امکانات](#-امکانات)
- [پیش‌نیازها](#-پیشنیازها)
- [نصب](#-نصب)
- [راه‌اندازی](#-راهاندازی)
- [استفاده](#-استفاده)
- [مستندات API](#-مستندات-api)
- [لایسنس](#-لایسنس)

---

## 🎯 معرفی

یک SDK سبک و کارآمد برای یکپارچه‌سازی سرویس ردیابی [AdTrace.io](https://adtrace.io) در اپلیکیشن‌های **.NET MAUI** با پشتیبانی از **Android** و **iOS**.

---

## ✨ امکانات

| قابلیت | وضعیت |
|--------|:------:|
| ردیابی نصب (Install) | ✅ |
| ردیابی نشست (Session) | ✅ |
| ردیابی رویداد (Event) | ✅ |
| اتریبیوشن (Attribution) | ✅ |
| ردیابی درآمد (Revenue) | ✅ |
| دیپ لینک (Deep Link) | ✅ |
| پشتیبانی GDPR | ✅ |
| Android | ✅ |
| iOS | ✅ |

---

## 📦 پیش‌نیازها

- NET 8.0. یا بالاتر
- NET MAUI Workload.
- +Visual Studio 2022 یا JetBrains Rider

---

## 🔧 نصب

### از طریق NuGet:
<div dir="ltr" align="left">

`dotnet add package Trackers.AdTrace`

</div>

</div>

<div dir="rtl" align="right">

### یا از طریق Package Manager:

<div dir="ltr" align="left">

`Install-Package Trackers.AdTrace`

</div>

</div>

<div dir="rtl" align="right">

---

## 🚀 راه‌اندازی

### مقداردهی اولیه SDK

در فایل `MauiProgram.cs` یا `App.xaml.cs`:

</div>


<div dir="ltr" align="left">

```csharp 
using Trackers.AdTrace; 
using Trackers.AdTrace.Models;

var config = new AdTraceConfig("YOUR_APP_TOKEN", AdTraceEnvironment.Sandbox) { LogLevel = LogLevel.Verbose, SendInBackground = false,
// کالبک اتریبیوشن
AttributionChanged = attribution =>
{
    Console.WriteLine($"Network: {attribution.Network}");
    Console.WriteLine($"Campaign: {attribution.Campaign}");
},

// کالبک‌های نشست
SessionTrackingSucceeded = success =>
{
    Console.WriteLine($"Session OK: {success.Message}");
},
SessionTrackingFailed = failure =>
{
    Console.WriteLine($"Session Failed: {failure.Message}");
}

AdTraceSdk.Create(config);
```

</div>
<div dir="rtl" align="right">

---

## 📖 استفاده
<div dir="ltr" align="left">

```csharp 
var event = new AdTraceEvent("EVENT_TOKEN");
AdTraceSdk.TrackEvent(event);
```
</div>

### ردیابی رویداد ساده

<div dir="ltr" align="left">

```csharp 
var purchaseEvent = new AdTraceEvent("PURCHASE_TOKEN"); 
purchaseEvent.SetRevenue(29.99m, "IRR"); 
purchaseEvent.SetTransactionId("txn_123456"); 
purchaseEvent.AddCallbackParameter("product_id", "PRD-100"); 
purchaseEvent.AddPartnerParameter("partner_key", "value");
AdTraceSdk.TrackEvent(purchaseEvent);
```

</div>
</div>


<div dir="rtl" align="right">

### دیپ لینک

<div dir="ltr" align="left">

```csharp 
// غیرفعال کردن
AdTraceSdk.SetEnabled(false) ;

// بررسی وضعیت
bool isActive = AdTraceSdk.IsEnabled();

// دریافت شناسه دستگاه
string? adid = AdTraceSdk.GetAdid();
```

</div>
</div>
<div dir="rtl" align="right">

### GDPR - حق فراموش شدن
<div dir="ltr" align="left">

```csharp 
AdTraceSdk.GdprForgetMe()
```
</div>
</div>

<div dir="rtl" align="right">

---

## 📚 مستندات API

### `AdTraceConfig`

| پراپرتی | نوع | توضیحات |
|---------|------|---------|
| `AppToken` | `string` | توکن اپلیکیشن از پنل AdTrace |
| `Environment` | `AdTraceEnvironment` | محیط (`Sandbox` / `Production`) |
| `LogLevel` | `LogLevel` | سطح لاگ‌گیری |
| `SendInBackground` | `bool` | ارسال در پس‌زمینه |
| `AttributionChanged` | `Action<AdTraceAttribution>?` | کالبک تغییر اتریبیوشن |
| `SessionTrackingSucceeded` | `Action<AdTraceSessionSuccess>?` | کالبک موفقیت نشست |
| `SessionTrackingFailed` | `Action<AdTraceSessionFailure>?` | کالبک خطای نشست |
| `EventTrackingSucceeded` | `Action<AdTraceEventSuccess>?` | کالبک موفقیت رویداد |
| `EventTrackingFailed` | `Action<AdTraceEventFailure>?` | کالبک خطای رویداد |
| `DeferredDeepLinkReceived` | `Action<Uri>?` | کالبک دیپ لینک |

### `AdTraceEvent`

| متد | توضیحات |
|-----|---------|
| `SetRevenue(decimal, string)` | تنظیم مبلغ و واحد پولی |
| `SetTransactionId(string)` | تنظیم شناسه تراکنش |
| `AddCallbackParameter(string, string)` | افزودن پارامتر کالبک |
| `AddPartnerParameter(string, string)` | افزودن پارامتر پارتنر |

### `AdTraceSdk`

| متد | توضیحات |
|-----|---------|
| `Create(AdTraceConfig)` | مقداردهی اولیه SDK |
| `TrackEvent(AdTraceEvent)` | ردیابی رویداد |
| `SetEnabled(bool)` | فعال/غیرفعال کردن |
| `IsEnabled()` | بررسی وضعیت |
| `GetAdid()` | دریافت شناسه دستگاه |
| `GetAttribution()` | دریافت اتریبیوشن فعلی |
| `AppWillOpenUrl(Uri)` | پردازش دیپ لینک |
| `GdprForgetMe()` | درخواست حذف اطلاعات |

---

## 🏗️ ساختار پروژه

</div>

<div dir="ltr" align="left">

```
Trackers.AdTrace/
├── Models/
│ ├── AdTraceConfig.cs
│ ├── AdTraceEvent.cs
│ ├── AdTraceAttribution.cs
│ ├── AdTraceSessionSuccess.cs
│ ├── AdTraceSessionFailure.cs
│ ├── AdTraceEventSuccess.cs
│ └── AdTraceEventFailure.cs
├── Services/
│ └── IAdTracePlatform.cs
├── Platforms/
│ ├── Android/
│ │ └── AdTracePlatformAndroid.cs
│ └── iOS/
|    └── AdTracePlatformiOS.cs
└── AdTrace.cs
```
</div>

<div dir="rtl" align="right">

---

## 📄 لایسنس

این پروژه تحت لایسنس [MIT](LICENSE) منتشر شده است.


<div dir="ltr" align="left">
📄 LICENSE

```ailicense
MIT License

Copyright (c) 2025 Trackers.AdTrace

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
</div>

---
<div align="center">

ساخته شده با ❤️ برای جامعه توسعه‌دهندگان MAUI .NET

</div>

</div>
</span>